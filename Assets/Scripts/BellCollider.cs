using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class BellCollider : MonoBehaviour
{
    //private SteamVR_Controller.Device rightHand;
    //private SteamVR_Controller.Device leftHand;
    
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

    private GameObject greenMarker;
    private GameObject blueMarker;
    private GameObject orangeMarker;

    Gradient blueGrad;
    Gradient greenGrad;
    Gradient orangeGrad;
    Gradient soloGrad;

    private Material highlight;
    private Material original;
    private Vector3 targetPos;

    public string noteName;

    // Use this for initialization
    void Start()
    {
        this.selection = GameObject.Find("Controller (left)").GetComponent<SelectionState>();

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

        blueGrad = new Gradient();
        orangeGrad = new Gradient();
        greenGrad = new Gradient();
        soloGrad = new Gradient();

        blueGrad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(46 / 255f, 130 / 255f, 1f), 0f), new GradientColorKey(Color.white, 1f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f)});
        greenGrad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(46/255f, 1f, 171/255f), 0f), new GradientColorKey(Color.white, 1f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        orangeGrad.SetKeys(new GradientColorKey[] { new GradientColorKey(new Color(1f, 171/255f, 46/255f), 0f), new GradientColorKey(Color.white, 1f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        soloGrad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });


        highlight = new Material(Resources.Load("HighlightMat") as Material);
        original = this.gameObject.GetComponent<MeshRenderer>().material;
        targetPos = this.transform.position;

        //Debug.Log("Green " + (greenMarker.gameObject.transform.position.y + greenMarker.gameObject.transform.localScale.y / 2) + " " + (greenMarker.gameObject.transform.position.y - greenMarker.gameObject.transform.localScale.y / 2));
        //Debug.Log("Blue " + (blueMarker.gameObject.transform.position.y + blueMarker.gameObject.transform.localScale.y / 2) + " " + (blueMarker.gameObject.transform.position.y - blueMarker.gameObject.transform.localScale.y / 2));
        //Debug.Log("Orange " + (orangeMarker.gameObject.transform.position.y + orangeMarker.gameObject.transform.localScale.y / 2) + " " + (orangeMarker.gameObject.transform.position.y - orangeMarker.gameObject.transform.localScale.y / 2));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        // if a controller has collided with a chime, play the appropriate sound

        //Debug.Log("COLLISION!");

        if (other.gameObject.layer == LayerMask.NameToLayer("ControllerLayer"))
        {
            //Debug.Log(other.gameObject.GetComponent<Rigidbody>().angularVelocity);

            if (other.gameObject.name.Equals("RightCube"))
            { // right hand
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.RightHand);
                //Debug.Log("collide RIGHT");
                hitChime(other.gameObject, this.gameObject);
            }
            else if (other.gameObject.name.Equals("LeftCube"))
            { // left hand only in hammer mode
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.LeftHand); //delay Sec, Duration sec, freq 1-320 Hz, amplitude 0-1, Input Source
                //Debug.Log("LEFT collide");
                hitChime(other.gameObject, this.gameObject);
            }
            else if (other.gameObject.name == "Controller (left)" && selection.selectionMode == true)
            {
                this.gameObject.GetComponent<MeshRenderer>().material = highlight;
                selection.text.text = noteName;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Controller (left)")
        {
            this.gameObject.GetComponent<MeshRenderer>().material = original;
            selection.text.text = "";
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
                double angle = Mathf.Rad2Deg * (Mathf.Atan(pos.y / pos.x));
                //Debug.Log(pos.x + ", " + pos.y + ", " + angle);

                if (pos.x < 0 && angle > -36 && angle < 90) //orange
                {
                    if (orangeMarker.activeSelf)
                    {
                        orangeGroup.Remove(this.gameObject);
                        orangeMarker.SetActive(false);
                    }
                    else
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
        if (collisionPos.y > blueBottom && collisionPos.y < blueTop && blueMarker.activeSelf)
        {
            foreach (GameObject go in blueGroup)
            {
                playChime(go);
                if (selection.visResponseMode)
                {
                    playVisResponse(go, blueGrad);
                }
            }
        }
        else if (collisionPos.y > greenBottom && collisionPos.y < greenTop && greenMarker.activeSelf)
        {
            foreach (GameObject go in greenGroup)
            {
                playChime(go);
                if (selection.visResponseMode)
                {
                    playVisResponse(go, greenGrad);
                }
            }
        }
        else if (collisionPos.y > orangeBottom && collisionPos.y < orangeTop && orangeMarker.activeSelf)
        {
            foreach (GameObject go in orangeGroup)
            {
                playChime(go);
                if (selection.visResponseMode)
                {
                    playVisResponse(go, orangeGrad);
                }
            }
        }
        else
        {
            playChime(chime);
            if (selection.visResponseMode)
            {
                playVisResponse(chime, soloGrad);
            }
        }
    }

    private void playVisResponse(GameObject chime, Gradient grad)
    {
        ParticleSystem ps = chime.GetComponentInChildren<ParticleSystem>();
        var col = ps.colorOverLifetime;
        col.color = grad;
        ps.Emit(30);
    }

    private void playChime(GameObject chime)
    {
        chime.GetComponent<AudioSource>().Play();
       // Debug.Log(chime.GetComponent<AudioSource>().clip.name);
        //chime.gameObject.GetComponent<Animator>().applyRootMotion = true;
        //chime.GetComponent<Animator>().SetTrigger("hitChime");
    }
}
