using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class BellCollider : MonoBehaviour {
    //private SteamVR_Controller.Device rightHand;
    //private SteamVR_Controller.Device leftHand;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //TODO: put controllers on their own layer, then check if other.gameObject.layer is that layer

        // if a controller has collided with a chime, play the appropriate sound

        Debug.Log("COLLISION!");

        if (other.gameObject.layer == LayerMask.NameToLayer("ControllerLayer"))
        {
            //this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            this.gameObject.GetComponent<AudioSource>().Play();
            SteamVR_Input.__actions_default_out_Haptic.Execute(0,0.7f,50,0.5f,SteamVR_Input_Sources.Any); //delay Sec, Duration sec, freq 1-320 Hz, amplitude 0-1, Input Source
        }
    }
}
