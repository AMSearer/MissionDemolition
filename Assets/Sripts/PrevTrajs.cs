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
        
        Vector3[] newLinePos = new Vector3[ newLine.positionCount ];
        print("positionCount at puch " + newLine.positionCount);
        newLine.GetPositions( newLinePos );
        // int newLineEnd = newLine;
        line1Ago.positionCount = newLine.positionCount;
        line1Ago.SetPositions(newLinePos);
        // line1Ago.SetPositions(newLine.GetPositions());
        line1Ago.enabled = true;
    }
}
