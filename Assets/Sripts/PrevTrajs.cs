using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrevTrajs : MonoBehaviour
{
    static public PrevTrajs S; // Dfine a singleton

    [Header( "Set in Inspector" )]

    public LineRenderer line1Ago;
    public LineRenderer line2Ago;
    public LineRenderer line3Ago;

    // [Header( "Set Dynamically" )]

    // Awake is called
    void Awake()
    {
        S = this; // Define the singleton

        line1Ago.enabled = false;
        line2Ago.enabled = false;
        line3Ago.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Removes/Null's each previous render
    void Clear()
    {
        line1Ago = null;
    }

    // Takes a new line and pushes previous back 1
    public void Push( LineRenderer newLine )
    {
        Vector3[] tempV3;

        line3Ago.positionCount = line2Ago.positionCount;
        tempV3 = new Vector3[ line2Ago.positionCount ];
        line2Ago.GetPositions( tempV3 );
        line3Ago.SetPositions( tempV3 );

        line2Ago.positionCount = line1Ago.positionCount;
        tempV3 = new Vector3[ line1Ago.positionCount ];
        line1Ago.GetPositions( tempV3 );
        line2Ago.SetPositions( tempV3 );

        line1Ago.positionCount = newLine.positionCount;
        tempV3 = new Vector3[ line1Ago.positionCount ];
        newLine.GetPositions( tempV3 );
        line1Ago.SetPositions( tempV3 );

        line3Ago.enabled = true;
        line2Ago.enabled = true;
        line1Ago.enabled = true;
    }
}
