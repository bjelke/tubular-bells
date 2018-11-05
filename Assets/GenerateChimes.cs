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
        //convert chime lengths from inches to meters
        for (int i = 0; i < 18; i++){
            chimeLengths[i] = chimeLengths[i] * 0.0254f;
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
            cylinder.transform.localScale = new Vector3(0.0254f, chimeLengths[chimeCount], 0.0254f);
            chimeCount++;
        }

    }


    public void SaveCylinders()
    {

    }

}
