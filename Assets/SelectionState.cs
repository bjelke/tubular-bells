using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SelectionState : MonoBehaviour {

    public bool selectionMode = true;
    public GameObject hammer;
    public GameObject colorPicker;
    public GameObject controllerModel;

    public SteamVR_Action_Boolean toggleHammer; // technically not needed

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
	}
}
