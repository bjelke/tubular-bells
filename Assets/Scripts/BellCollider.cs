using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        //TODO: put controllers on their own layer, then check if other.gameObject.layer is that layer

        // if a controller has collided with a chime, play the appropriate sound

        Debug.Log("COLLISION!");

        if (other.gameObject.layer == LayerMask.NameToLayer("ControllerLayer"))
        {
            this.gameObject.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 200);
            this.gameObject.GetComponent<AudioSource>().Play();
            Debug.Log("inside method");
        }
    }
}
