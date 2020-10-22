using UnityEngine;
using System.Collections;
using UnityEngine.UI;                             // a

public enum GameMode {                            // b
    idle, 
    playing,
    levelEnd
}

public class MissionDemolition: MonoBehaviour {
    static private MissionDemolition S; // a private Singleton

    [Header( "Set in Inspector" )]
    public Text                 uitLevel; // The UIText_Level Text
    public Text                 uitShots; // The UIText_Shots Text
    public Text                 uitBestScore;  // Displays stored best score per level
    public Text                 uitButton; // The Text on UIButton_View
    public Vector3              castlePos; // The place to put castles
    public GameObject[]         castles;   // An array of the castles

    public bool resetScores = false;   // flag to reset stored scores for testing
    public int newBestScores = 25;  // value to reset stored scores to
    public GameObject[] trajectories = new GameObject[3]; // An array of previous trajectories

    [Header( "Set Dynamically" )]
    public int                  level;     // The current level
    public int                  levelMax;  // The number of levels
    public int                  shotsTaken;
    public GameObject          castle;    // The current castle
    public GameMode            mode = GameMode.idle;
    public string               showing = "Show Slingshot" ; // FollowCam mode

    public int[] bestScores;// = new int[castles.Length];  // list of best scores per level

    private LineRenderer currLine;  // Stores, accesses the projectile line for the current shot
    private GameObject pastLines;  // Object holding up to the past 3 trajectories

/*     void Awake() {
        for (int lvl = 0; lvl < castles.Length; lvl++) {
            if (PlayerPrefs.HasKey("MD_BestScore_Lvl" + lvl)) {
                bestScores[lvl] = PlayerPrefs.GetInt("MD_BestScore_Lvl" + lvl);
            }
            else {
                PlayerPrefs.SetInt("MD_BestScore_Lvl" + lvl, 25);
                bestScores[lvl] = 25; // dummy best score for unplayed levels
            }

        }

    } */

    void Start() {

        // Get the current trajectory from projectile line
        currLine = GameObject.Find("ProjectileLine").GetComponent<LineRenderer>();

        pastLines = GameObject.Find("PreviousTrajectories");

        S = this; // Define the Singleton

        level = 0;
        levelMax = castles.Length;

        bestScores = new int[ levelMax ];

        for (int lvl = 0; lvl < castles.Length; lvl++) {
            if (PlayerPrefs.HasKey("MD_BestScore_Lvl" + lvl)) {
                if ( resetScores ) {
                    PlayerPrefs.SetInt("MD_BestScore_Lvl" + lvl, newBestScores);
                }
                bestScores[lvl] = PlayerPrefs.GetInt("MD_BestScore_Lvl" + lvl);
            }
            else {
                PlayerPrefs.SetInt("MD_BestScore_Lvl" + lvl, 25);
                bestScores[lvl] = 25; // dummy best score for unplayed levels
            }

        }
        StartLevel();
    }

    void StartLevel() {
        // Get rid of the old castle if one exists
        if (castle != null ) {
            // Destroy( castle );
            castle.SetActive(false);
        }

        // Destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag( "Projectile" );
        foreach (GameObject pTemp in gos) {
            Destroy( pTemp );
            
        }

        // Instantiate the new castle
        castle = Instantiate< GameObject>( castles[level] );
        castle.transform.position = castlePos;
        castle.SetActive(true);
        shotsTaken = 0 ;

        // Reset the camera
        SwitchView("wShow Both" );

        // pastLines.PrevTrajs.Push(ProjectileLine);

        // PrevTrajs.S.Push(ProjectileLine.S.line);

        // PrevTrajs.S.Push(ProjectileLine.S.line);

        ProjectileLine.S.Clear();

        // Reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI() {
        // Show the data in the GUITexts
        uitLevel.text = "Level: " +(level+1 )+ "of " +levelMax;
        uitShots.text = "Shots Taken: " +shotsTaken;
        uitBestScore.text = "BestScore: " + bestScores[ level ];
    }


    void Update() {
        UpdateGUI();

        // Check for level end
        if ( (mode == GameMode.playing) && Goal.goalMet ) {
            // Change mode to stop checking for level end
            // print("LevelEnd");
            mode = GameMode.levelEnd;
            // Zoom out
            SwitchView("Show Both" );

            // Write to PlayerPrefs
            if ( shotsTaken < bestScores[ level ] ) {
                uitBestScore.text = shotsTaken.ToString();
                PlayerPrefs.SetInt("MD_BestScore_Lvl" + level, shotsTaken);
                bestScores[ level ] = shotsTaken;  // Update for multiple rounds this "play"
            }

            // Start the next level in 2 seconds
            Invoke("NextLevel" ,2f );
        }
    }

    void NextLevel() {
        level++;
        if (level == levelMax) {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView( string eView ="" ) {                                  // c
        if (eView =="") {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing) {
            case"Show Slingshot" :
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case"Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both" ;
                break;

            case"Show Both" :
                FollowCam.POI = GameObject.Find("ViewBoth" );
                uitButton.text = "Show Slingshot";
                break;


            }
        }

        // Static method that allows code anywhere to increment shotsTaken
        public static void ShotFired() {                                        // d
            S.shotsTaken++;
            PrevTrajs.S.Push(ProjectileLine.S.line);
    }
}