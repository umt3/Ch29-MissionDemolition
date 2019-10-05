using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// TODO: You must set the values for the enum
public enum GameMode
{
    idle,
    playing,
    levelEnd,

}

// TODO: implement the MissionDemolition script
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;  // a private singleton 

    [Header("Set in Inspector")]

    public Text uitlevel;
    public Text uitshots;
    public Text uitButton;
    public Vector3 castlePos;   //place to put castles
    public GameObject[] castles; //array of castles  




    [Header("Set Dynamically")]

    public int level; //current level
    public int levelMax; //number of levels 
    public int shotsTaken;
    public GameObject castle; //current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //FollowCame mode


    // Use this for initialization
    void Start()
    {
        S = this; //define the Singleton
        level = 0;
        levelMax = castles.Length;
        StartLevel();

    }

    void StartLevel()
    {
        //get rid of old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //destroy old projectiles if they exist


        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }
        //instantiate the new castle 

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;


        //reset camera

        SwitchView("wShow Both");
        ProjectileLine.S.Clear();

        //reset the goal
        Goal.goalMet = false;
    }

    void UpdateGUI()
    {

        //uitlevel.text = "Level:" + (level1 + 1) + "of" + levelMax;
        // uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update()
    {
        UpdateGUI();
        //check for level end

        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            //zoom out
            SwitchView("Show Both");
            //start the level in 2 seconds
            Invoke("Next level", 2f);

        }

    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }

        StartLevel();

    }



    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }



        showing = eView;
        switch (showing) { 
       case "Show Significant":
            FollowCam.POI = null;
            uitButton.text = "Show Castle";

            break;


        case "Show Castle":
            FollowCam.POI = S.castle;
            uitButton.text = "Show Both";

            break;


        case "Show Both":
            FollowCam.POI =GameObject.Find("View Both");
            uitButton.text = "Show Slingback";
            break;


        }

    }

    //Static method that allows code anywhere to increment shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
