using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    // fields set in the Unity Inspector pane
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3       launchPos;
    public GameObject      projectile;
    public bool aimingMode; 
    private GameObject leftArm;
    private GameObject rightArm;
    private LineRenderer leftSling;
    private LineRenderer rightSling;
    private Rigidbody projectileRigidbody; 

    static public Vector3 LAUNCH_POS {
        get {
            if (S == null ) return Vector3.zero;
            return S.launchPos;
        }
    }

    void Awake() {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive( false );
        launchPos = launchPointTrans.position;  

        leftArm =  GameObject.Find("LeftArm");
        rightArm =  GameObject.Find("RightArm");
        leftSling = leftArm.GetComponent<LineRenderer>();
        rightSling = rightArm.GetComponent<LineRenderer>();   

        leftSling.SetPosition(0, leftArm.transform.position);
        leftSling.SetPosition(1, rightArm.transform.position);
        leftSling.enabled = true;

        rightSling.enabled = false;   
    }
    void OnMouseEnter() {
        // print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive( true );
    }

    void OnMouseExit() {
        // print("Slingshot:OnMouseExit()");
        launchPoint.SetActive( false );
    }

    void OnMouseDown() {
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        // Instantiate a Projectile
        projectile = Instantiate( prefabProjectile ) as GameObject;
        // Start it at the launchPoint
        projectile.transform.position = launchPos;
        // Set it to isKinematic for now

        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

 
    void Update() {
        // If Slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return;

        // Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint( mousePos2D );

        // Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D-launchPos;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }   


        // Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        leftSling.SetPosition(0, leftArm.transform.position);
        leftSling.SetPosition(1, projectile.transform.position);
        rightSling.SetPosition(0, rightArm.transform.position);
        rightSling.SetPosition(1, projectile.transform.position);

        rightSling.enabled = true;

        if ( Input.GetMouseButtonUp(0) ) {
            // The mouse has been released
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile; 

            leftSling.SetPosition(0, leftArm.transform.position);
            leftSling.SetPosition(1, rightArm.transform.position);
            leftSling.enabled = true;

            rightSling.enabled = false; 
        }
    }
}
