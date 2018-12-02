using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SelectionState : MonoBehaviour {

    public bool selectionMode = true;
    public GameObject hammer;
    public GameObject colorPicker;
    public GameObject controllerModel;

    public GameObject testCube;

    public SteamVR_Action_Boolean toggleHammer; // technically not needed

    private Color blue = new Color(0,0,200);
    private Color green = new Color(0, 200, 0);
    private Color orange = new Color(200, 30, 0);

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

        if (SteamVR_Input._default.inActions.PickColor.GetStateDown(SteamVR_Input_Sources.LeftHand)) {
            if (selectionMode) { 
                Debug.Log("Pressed Wheel");
                SteamVR_Action_Vector2 trackpadPos = SteamVR_Input._default.inActions.TouchPosition;
                Vector2 pos = trackpadPos.GetAxis(SteamVR_Input_Sources.LeftHand);
                double angle = Mathf.Rad2Deg * (Mathf.Atan(pos.y / pos.x)); // might be something weird with negatives
                Debug.Log(angle);

                //OPTION 1: figure out color region based on angle
                // OPTION 2: figure out color from texture (need to convert position)
                if ((pos.x < 0 && pos.y < 0 && angle > 60) || (pos.x > 0 && pos.y > 0 && angle > 60)) // just to test if we can get input
                {
                    testCube.gameObject.GetComponent<MeshRenderer>().material.color = blue;
                }
                else if (pos.x > 0 && pos.y < 0)
                {
                    testCube.gameObject.GetComponent<MeshRenderer>().material.color = green;
                }
                else {
                    testCube.gameObject.GetComponent<MeshRenderer>().material.color = orange;
                }

            }
        }
	}
}
