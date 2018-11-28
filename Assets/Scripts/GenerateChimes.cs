using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateChimes : MonoBehaviour {

    public GameObject chimes;

   
    float[] chimeLengths = new float[18] {38f, 39f, 40f, 41.5f, 43f, 44f, 45.5f, 46.5f, 48.5f, 49.5f, 51.5f, 52.5f, 54.5f, 56f, 57.5f, 59.5f, 61f, 63f};
    float[] chimeHeight = new float[18] {2f, 2f, 2.1f, 2f, 2.1f, 2f, 2f, 2.1f, 2f, 2.1f, 2f, 2.1f, 2f, 2f, 2.1f, 2f, 2.1f, 2f};
    string[] soundFileNames = { "0F", "1E", "11Eflat", "2D", "12Csharp", "3C", "4B", "13Bflat", "5A", "14Aflat", "6G", "15Fsharp", "7lowF", "8lowE", "16lowEflat", "9lowD", "17lowCsharp", "10lowC"};


    // Use this for initialization
    void Start() {


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
        foreach (Transform cylinder in chimes.transform) {
            cylinder.transform.localScale = new Vector3(0.0354f, chimeLengths[chimeCount], 0.0354f); //0.0554f -> these are the x & z terms for the width of the chimes

            cylinder.transform.localPosition = new Vector3(0.75f * Mathf.Cos(0.1954f * chimeCount), chimeHeight[chimeCount] - chimeLengths[chimeCount], 0.75f * Mathf.Sin(0.1954f * chimeCount));

            int[] flats = { 2, 4, 7, 9, 11, 14, 16 };
            ArrayList sharps = new ArrayList(flats);
            if (sharps.Contains(chimeCount)){
                cylinder.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            }
            
            //if (chimeCount < 11)
            //{
            //    cylinder.transform.localPosition = new Vector3(0 + chimeCount * 0.2f, 2f - chimeLengths[chimeCount], 0);
            //}
            //else if (chimeCount >= 16)
            //{
            //    cylinder.transform.localPosition = new Vector3(0.1f + (chimeCount - 8) * 0.2f, 2.1f - chimeLengths[chimeCount], 0);
            //    cylinder.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            //} else if (chimeCount >= 13)
            //{
            //    cylinder.transform.localPosition = new Vector3(0.1f + (chimeCount - 9) * 0.2f, 2.1f - chimeLengths[chimeCount], 0);
            //    cylinder.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            //}
            //else
            //{
            //    cylinder.transform.localPosition = new Vector3(0.1f + (chimeCount-10) * 0.2f, 2.1f - chimeLengths[chimeCount], 0);
            //    cylinder.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            //}
            chimeCount++;
        }

    }


}
