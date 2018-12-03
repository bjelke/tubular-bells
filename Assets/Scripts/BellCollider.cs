using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class BellCollider : MonoBehaviour {
    //private SteamVR_Controller.Device rightHand;
    //private SteamVR_Controller.Device leftHand;

    UnityEngine.Audio.AudioMixer mixer;
    public SelectionState selection;

    static ArrayList greenGroup = new ArrayList();
    static ArrayList blueGroup = new ArrayList();
    static ArrayList orangeGroup = new ArrayList();

    const float greenTop = 1.109318f;
    const float greenBottom = 0.9686817f;

    const float blueTop = 1.477318f;
    const float blueBottom = 1.336617f;

    const float orangeTop = 1.293318f;
    const float orangeBottom = 1.152682f;

    GameObject greenMarker;
    GameObject blueMarker;
    GameObject orangeMarker;

    // Use this for initialization
    void Start () {
        this.selection = GameObject.Find("Controller (left)").GetComponent<SelectionState>();
        mixer = Resources.Load("MasterMixer") as UnityEngine.Audio.AudioMixer;
        greenMarker = Instantiate(Resources.Load("GreenMarker") as GameObject);
        greenMarker.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, greenMarker.gameObject.transform.position.y, this.gameObject.transform.position.z);
        greenMarker.transform.parent = this.gameObject.transform;
        greenMarker.SetActive(false);

        blueMarker = Instantiate(Resources.Load("BlueMarker") as GameObject);
        blueMarker.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, blueMarker.gameObject.transform.position.y, this.gameObject.transform.position.z);
        blueMarker.transform.parent = this.gameObject.transform;
        blueMarker.SetActive(false);

        orangeMarker = Instantiate(Resources.Load("OrangeMarker") as GameObject);
        orangeMarker.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, orangeMarker.gameObject.transform.position.y, this.gameObject.transform.position.z);
        orangeMarker.transform.parent = this.gameObject.transform;
        orangeMarker.SetActive(false);

        Debug.Log("Green " + (greenMarker.gameObject.transform.position.y + greenMarker.gameObject.transform.localScale.y / 2) + " " + (greenMarker.gameObject.transform.position.y - greenMarker.gameObject.transform.localScale.y / 2));
        Debug.Log("Blue " + (blueMarker.gameObject.transform.position.y + blueMarker.gameObject.transform.localScale.y / 2) + " " + (blueMarker.gameObject.transform.position.y - blueMarker.gameObject.transform.localScale.y / 2));
        Debug.Log("Orange " + (orangeMarker.gameObject.transform.position.y + orangeMarker.gameObject.transform.localScale.y / 2) + " " + (orangeMarker.gameObject.transform.position.y - orangeMarker.gameObject.transform.localScale.y / 2));
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
                //this.gameObject.GetComponent<AudioSource>().Play();
                hitChime(other.gameObject, this.gameObject);
            }
            else if (other.gameObject.name.Equals("LeftCube")){ // left hand only in hammer mode
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.LeftHand); //delay Sec, Duration sec, freq 1-320 Hz, amplitude 0-1, Input Source
                Debug.Log("LEFT collide");
                //this.gameObject.GetComponent<AudioSource>().Play();
                hitChime(other.gameObject, this.gameObject);
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
                    if (orangeMarker.activeSelf)
                    {
                        orangeGroup.Remove(this.gameObject);
                        orangeMarker.SetActive(false);
                    } else
                    {
                        orangeGroup.Add(this.gameObject);
                        orangeMarker.SetActive(true);
                    }
                }
                else if (pos.x > 0 && angle < 36 && angle > -90) //green
                {
                    if (greenMarker.activeSelf)
                    {
                        greenGroup.Remove(this.gameObject);
                        greenMarker.SetActive(false);
                    }
                    else
                    {
                        greenGroup.Add(this.gameObject);
                        greenMarker.SetActive(true);
                    }
                }
                else //blue
                {
                    if (blueMarker.activeSelf)
                    {
                        blueGroup.Remove(this.gameObject);
                        blueMarker.SetActive(false);
                    }
                    else
                    {
                        blueGroup.Add(this.gameObject);
                        blueMarker.SetActive(true);
                    }
                }
            }
        }
    }

    private void hitChime(GameObject collider, GameObject chime)
    {
        Vector3 collisionPos = collider.gameObject.transform.position;
        if(collisionPos.y > blueBottom && collisionPos.y < blueTop && blueMarker.activeSelf)
        {
            foreach (GameObject go in blueGroup)
            {
                go.GetComponent<AudioSource>().Play();
            }
        } else if (collisionPos.y > greenBottom && collisionPos.y < greenTop && greenMarker.activeSelf)
        {
            foreach (GameObject go in greenGroup)
            {
                go.GetComponent<AudioSource>().Play();
            }
        } else if (collisionPos.y > orangeBottom && collisionPos.y < orangeTop && orangeMarker.activeSelf)
        {
            foreach (GameObject go in orangeGroup)
            {
                go.GetComponent<AudioSource>().Play();
            }
        } else
        {
            chime.GetComponent<AudioSource>().Play();
        }
    }
}
