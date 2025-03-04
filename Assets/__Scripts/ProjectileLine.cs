﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Singleton

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake()
    {
        S = this; // Set the singleton
        // Get a reerence to the LineRenderer
        line = GetComponent<LineRenderer>();
        // Disable the LineRenderer until it is needed
        line.enabled = false;
        // Initialize the points List
        points = new List<Vector3>();
    }

    // This is a property (a method masquerating as a field)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                // When _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    // This can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }


    //TODO: Implement the AddPoint function
    public void AddPoint()
    {
        // *** Implement this code ***

        Vector3 pt = poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //if point isn't far enough from the last point, it returns
            return;

        }

        if (points.Count == 0)  //if this is the launch point...
        {
            Vector3 launchPosDiff = pt - Slingshot.Launch_POS;
            //to be defined... its adds extea bit of line to add aimimg later

            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //enables the LineRenderer 
            line.enabled = true;
        }
        else
        {
            // normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }


    }

    // Returns the location of the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                // If there are no points, returns Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    void FixedUpdate()
    {
        if (poi == null)
        {
            // If there is no poi, search for one
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; // Return if it is not the Projectile
                }
            }
            else
            {
                return; // Return if we didn't find a poi
            }
        }
        //If there is a poi, its loc is added every FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            // Once FollowCam.POI is null, make the local poi null too
            poi = null;
        }
    }
}
