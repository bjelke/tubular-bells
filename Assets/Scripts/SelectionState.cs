using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SelectionState : MonoBehaviour {

    // This class handles most input from the Vive controllers and turns things on/off appropriately.
    // It is attached to the left controller - button input might not always work from the right controller.
    // There is a known bug with the top menu button / particle effect toggle not working consistently.
    // Because of the way input from SteamVR works now, the button input could be remapped to correspond to different actions.
    // The code would work the same way, but our comments refer to the default mapping we chose.

    public bool selectionMode = true; // controller model and ability to select chimes
    public bool visResponseMode = true; // particle effects

    AudioMixer mixer;

    public GameObject hammer;
    public GameObject colorPicker;
    public GameObject controllerModel;
    public Text text; // This object is referenced by the other scripts during runtime, but it's set in the editor so we had to do it here.

    // Use this for initialization
    void Start () {
        mixer = Resources.Load("MasterMixer") as AudioMixer;
    }

    // Update is called once per frame
    void Update () {
        // Toggle between hammer and chord color selection when left trigger is pressed
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

        // Toggle particle effects on and off with menu button on either controller. Default is on.
        if (SteamVR_Input._default.inActions.ToggleFunMode.GetStateDown(SteamVR_Input_Sources.Any))
        {
            visResponseMode = visResponseMode ? false : true;
        }

        // Press grip to lower volume while playing
        //TODO: implement cooler dampening that reflects how sound works in the real world
        if (SteamVR_Input._default.inActions.Dampen.GetStateDown(SteamVR_Input_Sources.Any))
        {
            mixer.SetFloat("masterVolume", -10f);
        }
        else if (SteamVR_Input._default.inActions.Dampen.GetStateUp(SteamVR_Input_Sources.Any))
        {
            mixer.SetFloat("masterVolume", 0f);
        }
	}
}
