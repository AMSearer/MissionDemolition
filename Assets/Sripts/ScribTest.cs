﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScribTest : MonoBehaviour
{
 public GameObject obc;
//  public GameObject post2;
 void Update () {

        // this.GetComponent<LineRenderer>().positionCount = 3;
             // Set the first vertex position of the line segment (the position of this 3D object)
        this.GetComponent<LineRenderer>().SetPosition(0, this.transform.position);
                 // Set the second vertex position of the line segment (the location of the new 3D object)
        this.GetComponent<LineRenderer>().SetPosition(1, obc.transform.position);

        // this.GetComponent<LineRenderer>().SetPosition(2, post2.transform.position);
	}
}
