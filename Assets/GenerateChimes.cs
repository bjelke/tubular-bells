using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateChimes : MonoBehaviour {

    public GameObject leftmostCylinder;

    List<GameObject> cylinders = new List<GameObject>();

	// Use this for initialization
	void Start () {
        cylinders.Add(leftmostCylinder);

        MakeCylinders();
        SaveCylinders();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MakeCylinders()
    {
        for (int i = 1; i <= 18; i++)
        {
            GameObject newCylinder = ObjectFactory.CreateGameObject("cylinder" + i.ToString());

            cylinders.Add(newCylinder);
        }

    }

    public void copyMesh()
    {
        Mesh copyingMesh = leftmostCylinder.GetComponent<MeshFilter>().mesh;
        for (int i = 1; i <= 18; i++)
        {
            cylinders[i].AddComponent<MeshFilter>();
            cylinders[i].GetComponent<MeshFilter>().mesh.vertices = copyingMesh.vertices;
            cylinders[i].GetComponent<MeshFilter>().mesh.triangles = copyingMesh.triangles;
            cylinders[i].GetComponent<MeshFilter>().mesh.uv = copyingMesh.uv;
            cylinders[i].GetComponent<MeshFilter>().mesh.normals = copyingMesh.normals;
            cylinders[i].GetComponent<MeshFilter>().mesh.colors = copyingMesh.colors;
            cylinders[i].GetComponent<MeshFilter>().mesh.tangents = copyingMesh.tangents;
        }
    }

    public void SaveCylinders()
    {

    }

}
