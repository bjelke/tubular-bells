using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class BellCollider : MonoBehaviour {
    //private SteamVR_Controller.Device rightHand;
    //private SteamVR_Controller.Device leftHand;

    UnityEngine.Audio.AudioMixer mixer; 

    // Use this for initialization
    void Start () {

        mixer = Resources.Load("MasterMixer") as UnityEngine.Audio.AudioMixer;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {

        // if a controller has collided with a chime, play the appropriate sound

        //Debug.Log("COLLISION!");

        if (other.gameObject.layer == LayerMask.NameToLayer("ControllerLayer"))
        {
            //this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);

            //UnityEngine.Audio.AudioMixerGroup master = mixer.FindMatchingGroups("Master")[0];

            //this.gameObject.GetComponent<AudioSource>().volume = Mathf.Clamp01(other.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
            Debug.Log(other.gameObject.GetComponent<Rigidbody>().angularVelocity);
            this.gameObject.GetComponent<AudioSource>().Play();


            if (other.gameObject.name.Equals("RightCube")){ // right hand
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.RightHand);
                Debug.Log("collide RIGHT");
            }
            else { // left hand ("LeftCube")
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.LeftHand); //delay Sec, Duration sec, freq 1-320 Hz, amplitude 0-1, Input Source
                Debug.Log("LEFT collide");
            }
        }
    }
}
