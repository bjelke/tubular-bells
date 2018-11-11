using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateChimes : MonoBehaviour {

    public GameObject chimes;

    //List<GameObject> cylinders = new List<GameObject>();
    float[] chimeLengths = new float[18] { 38f, 39f, 41.5f, 44f, 45.5f, 48.5f, 51.5f, 54.5f, 56f, 59.5f, 63f, 40f, 43f, 46.5f, 49.5f, 52.5f, 57.5f, 61f};

	// Use this for initialization
	void Start () {
        //convert chime lengths from inches to meters & create cylinder primitives.
        for (int i = 0; i < 18; i++){
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.parent = chimes.transform;
            chimeLengths[i] = chimeLengths[i] / 39.7301f *2 / 3;

            cylinder.AddComponent<Rigidbody>();
            cylinder.AddComponent<CapsuleCollider>();
            cylinder.AddComponent<BellCollider>();

            cylinder.gameObject.GetComponent<CapsuleCollider>().transform.localScale = cylinder.gameObject.transform.localScale;
            cylinder.gameObject.GetComponent<CapsuleCollider>().transform.localPosition = cylinder.gameObject.transform.localPosition;
            cylinder.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;

            cylinder.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            cylinder.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }

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
            cylinder.transform.localScale = new Vector3(0.0554f, chimeLengths[chimeCount], 0.0554f); //0.0254f
            cylinder.transform.localPosition = new Vector3(0 + chimeCount*0.1f, 1.5f - chimeLengths[chimeCount]/2, 0);
            chimeCount++;
        }

    }


    public void SaveCylinders()
    {

    }

}
