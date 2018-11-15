using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateChimes : MonoBehaviour {

    public GameObject chimes;

    //List<GameObject> cylinders = new List<GameObject>();
    float[] chimeLengths = new float[18] { 38f, 39f, 41.5f, 44f, 45.5f, 48.5f, 51.5f, 54.5f, 56f, 59.5f, 63f, 40f, 43f, 46.5f, 49.5f, 52.5f, 57.5f, 61f};
    string[] soundFileNames = {"0F", "1E", "2D", "3C", "4B", "5A", "6G", "7lowF", "8lowE", "9lowD", "10lowC", "11Eflat", "12Csharp", "13Bflat", "14Aflat", "15Fsharp", "16lowEflat", "17lowCsharp"};

	// Use this for initialization
	void Start () {
        //convert chime lengths from inches to meters & create cylinder primitives.
        for (int i = 0; i < 18; i++){
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.parent = chimes.transform;
            chimeLengths[i] = chimeLengths[i] / 39.7301f *2 / 4;

            cylinder.AddComponent<Rigidbody>();
            cylinder.AddComponent<CapsuleCollider>();
            cylinder.AddComponent<BellCollider>();
            cylinder.AddComponent<AudioSource>();

            AudioClip note = Resources.Load<AudioClip>("ChimeNotes/spaceHarpsicord/"+soundFileNames[i]);
            cylinder.gameObject.GetComponent<AudioSource>().clip = note;
            //cylinder.gameObject.GetComponent<AudioSource>().PlayOneShot(noteG, 0.6f);


            cylinder.gameObject.GetComponent<CapsuleCollider>().transform.localScale = cylinder.gameObject.transform.localScale;
            cylinder.gameObject.GetComponent<CapsuleCollider>().transform.localPosition = cylinder.gameObject.transform.localPosition;
            cylinder.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;

            cylinder.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            cylinder.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }

       // GameObject.CreatePrimitive(PrimitiveType.Plane).transform.localPosition = new Vector3(0, 2f, 0);
        PositionCylinders();
        SaveCylinders();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PositionCylinders()
    {
        //for(int i = 0; i < 18; i++){
        //    cylinders[i].transform.localScale = new Vector3(0.0254f, chimeLengths[i], 0.0254f);
        //}
        int chimeCount = 0;
        foreach( Transform cylinder in chimes.transform){
            cylinder.transform.localScale = new Vector3(0.0354f, chimeLengths[chimeCount], 0.0354f); //0.0554f -> these are the x & z terms for the width of the chimes
            if (chimeCount < 11)
            {
                cylinder.transform.localPosition = new Vector3(0 + chimeCount * 0.2f, 2f - chimeLengths[chimeCount], 0);
            }
            else if (chimeCount >= 16)
            {
                cylinder.transform.localPosition = new Vector3(0.1f + (chimeCount - 8) * 0.2f, 2.1f - chimeLengths[chimeCount], 0);
                cylinder.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            } else if (chimeCount >= 13)
            {
                cylinder.transform.localPosition = new Vector3(0.1f + (chimeCount - 9) * 0.2f, 2.1f - chimeLengths[chimeCount], 0);
                cylinder.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            }
            else
            {
                cylinder.transform.localPosition = new Vector3(0.1f + (chimeCount-10) * 0.2f, 2.1f - chimeLengths[chimeCount], 0);
                cylinder.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            }
            chimeCount++;
        }

    }


    public void SaveCylinders()
    {

    }

}
