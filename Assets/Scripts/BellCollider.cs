using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class BellCollider : MonoBehaviour {
    //private SteamVR_Controller.Device rightHand;
    //private SteamVR_Controller.Device leftHand;

    UnityEngine.Audio.AudioMixer mixer;
    public SelectionState selection;

    ArrayList greenGroup = new ArrayList();
    ArrayList blueGroup = new ArrayList();
    ArrayList orangeGroup = new ArrayList();

    GameObject greenMarker;
    GameObject blueMarker;
    GameObject orangeMarker;

    // Use this for initialization
    void Start () {
        this.selection = GameObject.Find("Controller (left)").GetComponent<SelectionState>();
        mixer = Resources.Load("MasterMixer") as UnityEngine.Audio.AudioMixer;
        greenMarker = Resources.Load("GreenMarker") as GameObject;
        blueMarker = Resources.Load("BlueMarker") as GameObject;
        orangeMarker = Resources.Load("OrangeMarker") as GameObject;
        Debug.Log("loaded marker prefabs");
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
            //Debug.Log(other.gameObject.GetComponent<Rigidbody>().angularVelocity);
           


            if (other.gameObject.name.Equals("RightCube")){ // right hand
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.RightHand);
                Debug.Log("collide RIGHT");
                this.gameObject.GetComponent<AudioSource>().Play();
            }
            else if (other.gameObject.name.Equals("LeftCube")){ // left hand only in hammer mode
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.LeftHand); //delay Sec, Duration sec, freq 1-320 Hz, amplitude 0-1, Input Source
                Debug.Log("LEFT collide");
                this.gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        GameObject collider = other.gameObject;
        if (collider.name == "Controller (left)" && selection.selectionMode == true)
        {
            //Debug.Log("selecting ");
            if (SteamVR_Input._default.inActions.PickColor.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                Debug.Log("Pressed Wheel");
                SteamVR_Action_Vector2 trackpadPos = SteamVR_Input._default.inActions.TouchPosition;
                Vector2 pos = trackpadPos.GetAxis(SteamVR_Input_Sources.LeftHand);
                double angle = Mathf.Rad2Deg * (Mathf.Atan(pos.y / pos.x)); // might be something weird with negatives
                                                                            //Debug.Log(pos.x + ", " + pos.y + ", " + angle);
                if (pos.x < 0 && angle > -36 && angle < 90) //orange
                {
                    orangeGroup.Add(this.gameObject);
                    GameObject marker = Instantiate(orangeMarker);
                    marker.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, marker.gameObject.transform.position.y, this.gameObject.transform.position.z);
                    marker.transform.parent = this.gameObject.transform;
                    Debug.Log(marker.activeSelf);

                }
                else if (pos.x > 0 && angle < 36 && angle > -90) //green
                {
                    greenGroup.Add(this.gameObject);
                    GameObject marker = Instantiate(greenMarker);
                    marker.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, marker.gameObject.transform.position.y, this.gameObject.transform.position.z);
                    marker.transform.parent = this.gameObject.transform;
                }
                else //blue
                {
                    blueGroup.Add(this.gameObject);
                    GameObject marker = Instantiate(blueMarker);
                    marker.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, marker.gameObject.transform.position.y, this.gameObject.transform.position.z);
                    marker.transform.parent = this.gameObject.transform;
                }
            }
        }
    }
}
