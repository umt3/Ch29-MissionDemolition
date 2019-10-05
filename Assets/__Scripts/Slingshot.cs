using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// YOU must implement the Slingshot

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;


    [Header("Set in Inspector")]
    public GameObject prefabProjectile;

    private Rigidbody projectileRigidbody;

    static public Vector3 Launch_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }


    public float velocityMult = 8f;
    // all fields set dynamically


    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimimgMode;

    // public GameObject launchPoint;// 


    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }


    void OnMouseEnter()
    {
        //print("Slingshot: OnMouseEnter() ");

        //launchPoint.SetActive(true;
        print("Slingshot:OnMouseEnter()");

    }

    // Place class variables here

    void OnMouseExit()
    {
        //print("Slingshot: OnMouseExit() ");
        //launchPoint.SetActive(false);
        print("slingshot:OnMouseExit()");
    }



    void OnMouseDown()
    {
        //player has pressed the mouse button while over Slingshot
        aimimgMode = true;
        //instantiate a projectile
        projectile = Instantiate(prefabProjectile) as GameObject;
        // start it at its launchPoint
        projectile.transform.position = launchPos;
        //Set iskinematic for now

        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;

    }



    void Update()
    {


        //if Slingshot is not in aimimg mode, don't run this code

        if (!aimimgMode) return;

        //get current mouse coordinates in 2D

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //find the delta from launchPos to mousePos3d
        Vector3 mouseDelta = mousePos3D - Launch_POS;
        //limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // move projectile to new position

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            //the mouse has been released

            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;



            aimimgMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;

        }
    }

}


