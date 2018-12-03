using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SelectionState : MonoBehaviour {

    public bool selectionMode = true;
    public GameObject hammer;
    public GameObject colorPicker;
    public GameObject controllerModel;

    //public GameObject testCube;

    public SteamVR_Action_Boolean toggleHammer; // technically not needed

    private Color blue = new Color(46, 130, 255)/255f;
    private Color green = new Color(46, 255, 171)/255f;
    private Color orange = new Color(255, 171, 46)/255f;

    // Use this for initialization
    void Start () {
        selectionMode = true;
	}
	
	// Update is called once per frame
	void Update () {
        // when trigger is pressed, change selection mode to false
        // turn off model and color picker, turn on hammer
        // if already false, do opposite

        if (SteamVR_Input._default.inActions.ToggleHammer.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
            if (!selectionMode)
            {
                selectionMode = true;
             
                hammer.SetActive(false);
                controllerModel.SetActive(true);
                colorPicker.SetActive(true);
            }
            else
            {
                selectionMode = false;
             
                hammer.SetActive(true);
                controllerModel.SetActive(false);
                colorPicker.SetActive(false);
                
            }
        }

        //if (SteamVR_Input._default.inActions.PickColor.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
        //    if (selectionMode) { 
        //        //Debug.Log("Pressed Wheel");
        //        SteamVR_Action_Vector2 trackpadPos = SteamVR_Input._default.inActions.TouchPosition;
        //        Vector2 pos = trackpadPos.GetAxis(SteamVR_Input_Sources.LeftHand);
        //        double angle = Mathf.Rad2Deg * (Mathf.Atan(pos.y / pos.x)); // might be something weird with negatives
        //        //Debug.Log(pos.x + ", " + pos.y + ", " + angle);

        //        if(pos.x < 0 && angle > -36 && angle < 90)
        //        {
        //            testCube.gameObject.GetComponent<MeshRenderer>().material.color = orange;
        //        } else if (pos.x > 0 && angle < 36 && angle > -90)
        //        {
        //            testCube.gameObject.GetComponent<MeshRenderer>().material.color = green;
        //        } else
        //        {
        //            testCube.gameObject.GetComponent<MeshRenderer>().material.color = blue;
        //        }


        //    }
        //}
	}
}
