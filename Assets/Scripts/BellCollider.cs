using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class BellCollider : MonoBehaviour
{
    // This script is attached to each chime object and handles collision from the controller/hammer.

    public SelectionState selection;

    // pretty much everything regarding groups is hard coded right now. This would need to change if we added more groups.
    static ArrayList greenGroup = new ArrayList();
    static ArrayList blueGroup = new ArrayList();
    static ArrayList orangeGroup = new ArrayList();

    const float greenTop = 1.109318f;           // y positions of each group marker to check if a hammer collision is playing a group or a chime
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

        //every chime has all 3 marker components loaded in from prefabs. they are set to be active or unactive as needed
        greenMarker = Instantiate(Resources.Load("GreenMarker") as GameObject);
        blueMarker = Instantiate(Resources.Load("BlueMarker") as GameObject);
        orangeMarker = Instantiate(Resources.Load("OrangeMarker") as GameObject);
        setMarker(blueMarker);
        setMarker(orangeMarker);
        setMarker(greenMarker);

        //setup colors for different particle effects
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
    }

    private void setMarker(GameObject marker)
    {
        marker.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, marker.gameObject.transform.position.y, this.gameObject.transform.position.z);
        marker.transform.parent = this.gameObject.transform;
        marker.SetActive(false);
    }

    void Update()
    {}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ControllerLayer"))
        {   //play notes during hammer collision, show highlight and notename during controller collision
            if (other.gameObject.name.Equals("RightCube"))
            { // right hand
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.RightHand);
                hitChime(other.gameObject, this.gameObject);
            }
            else if (other.gameObject.name.Equals("LeftCube"))
            { // left hand only in hammer mode
                SteamVR_Input.__actions_default_out_Haptic.Execute(0, 0.7f, 50, 0.5f, SteamVR_Input_Sources.LeftHand); //delay Sec, Duration sec, freq 1-320 Hz, amplitude 0-1, Input Source
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
        {   //revert highlight and notename after controller collision
            this.gameObject.GetComponent<MeshRenderer>().material = original;
            selection.text.text = ""; //ouch
        }
    }


    private void OnTriggerStay(Collider other)
    {   //do group collision during controller collision
        GameObject collider = other.gameObject;
        if (collider.name == "Controller (left)" && selection.selectionMode == true)
        {
            if (SteamVR_Input._default.inActions.PickColor.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {   //respond to trackpad clicks
                SteamVR_Action_Vector2 trackpadPos = SteamVR_Input._default.inActions.TouchPosition;
                Vector2 pos = trackpadPos.GetAxis(SteamVR_Input_Sources.LeftHand);
                double angle = Mathf.Rad2Deg * (Mathf.Atan(pos.y / pos.x));

                if (pos.x < 0 && angle > -36 && angle < 90) //orange
                {
                    toggleActive(orangeMarker, orangeGroup);
                }
                else if (pos.x > 0 && angle < 36 && angle > -90) //green
                {
                    toggleActive(greenMarker, greenGroup);
                }
                else //blue
                {
                    toggleActive(blueMarker, blueGroup);
                }
            }
        }
    }

    private void toggleActive(GameObject marker, ArrayList group) {
        //control group markers
        if (marker.activeSelf) {
            group.Remove(this.gameObject);
        }
        else {
            group.Add(this.gameObject);
        }
        marker.SetActive(marker.activeSelf ? false : true);
    }

    private void hitChime(GameObject collider, GameObject chime)
    {
        // if the hammer hits the chime w/in bounds of one group marker & that marker is active, play all the chimes in that group
        Vector3 collisionPos = collider.gameObject.transform.position;
        if (collisionPos.y > blueBottom && collisionPos.y < blueTop && blueMarker.activeSelf)
        {
            playAll(blueGroup, blueGrad);
        }
        else if (collisionPos.y > greenBottom && collisionPos.y < greenTop && greenMarker.activeSelf)
        {
            playAll(greenGroup, greenGrad);
        }
        else if (collisionPos.y > orangeBottom && collisionPos.y < orangeTop && orangeMarker.activeSelf)
        {
            playAll(orangeGroup, orangeGrad);
        }
        else
        {
            chime.GetComponent<AudioSource>().Play();
            if (selection.visResponseMode){
                playVisResponse(chime, soloGrad);
            }
        }
    }

    private void playAll(ArrayList group, Gradient grad) {
        foreach (GameObject go in group) {
            go.GetComponent<AudioSource>().Play();
            if (selection.visResponseMode)
            {
                playVisResponse(go, grad);
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
}
