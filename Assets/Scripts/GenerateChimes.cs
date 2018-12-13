using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;

public class GenerateChimes : MonoBehaviour {
    // Handles all elements needed to initialize chimes, including game objects, audio components, and particle effects.
    // This code is executed once at the start of the program.

    public GameObject chimes;
    private GameObject particles;

    float[] chimeLengths = new float[18] {38f, 39f, 40f, 41.5f, 43f, 44f, 45.5f, 46.5f, 48.5f, 49.5f, 51.5f, 52.5f, 54.5f, 56f, 57.5f, 59.5f, 61f, 63f}; // measurements taken from chimes in the music department
    float[] chimeHeight = new float[18] {2f, 2f, 2.1f, 2f, 2.1f, 2f, 2f, 2.1f, 2f, 2.1f, 2f, 2.1f, 2f, 2f, 2.1f, 2f, 2.1f, 2f}; // relative height off the ground
    string[] soundFileNames = { "0F", "1E", "11Eflat", "2D", "12Csharp", "3C", "4B", "13Bflat", "5A", "14Aflat", "6G", "15Fsharp", "7lowF", "8lowE", "16lowEflat", "9lowD", "17lowCsharp", "10lowC"};
    string[] noteNames = { "F", "E", "E♭", "D", "C♯", "C", "B", "B♭", "A", "A♭", "G", "F♯", "F", "E", "E♭", "D", "C♯", "C" };


    // Use this for initialization
    void Start() {
        AudioMixer mixer = Resources.Load("MasterMixer") as AudioMixer;
        particles = Resources.Load("particlePrefab") as GameObject;
        
        //convert chime lengths from inches to meters & create cylinder primitives.
        for (int i = 0; i < 18; i++)
        {
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.transform.parent = chimes.transform;
            chimeLengths[i] = chimeLengths[i] / 39.7301f / 2.5f;
            chimeHeight[i] = chimeHeight[i] - 0.3f;

            //we kept these components in this for loop because they depend on the index i to find important values.
            cylinder.AddComponent<AudioSource>();   
            AudioClip note = Resources.Load<AudioClip>("ChimeNotes/spaceHarpsicord/" + soundFileNames[i]); // Space Harpsichord sounds the coolest, but playing church bells is also an option if the next line is uncommented
            //AudioClip note = Resources.Load<AudioClip>("ChimeNotes/longChurchBells/" + soundFileNames[i]);
            cylinder.gameObject.GetComponent<AudioSource>().clip = note;
            cylinder.gameObject.GetComponent<AudioSource>().outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[0];  //routes sound through the audioMixer so we can dampen the volume.

            cylinder.AddComponent<BellCollider>();
            cylinder.GetComponent<BellCollider>().noteName = noteNames[i];
        }

        PositionCylinders();        //need to position cylinders before you add components with position because some of them won't automatically move with the gameObject if it is moved later.

        // Add particle effect systems to each chime object
        foreach(Transform chime in chimes.transform) {
            GameObject cylinder = chime.gameObject;
            cylinder.AddComponent<Rigidbody>();

            GameObject cyParticles = Instantiate(particles, cylinder.transform.position, Quaternion.identity) as GameObject;
            cyParticles.transform.parent = cylinder.transform;
            cyParticles.transform.localScale = new Vector3(0.02f, 0.5f, 0.02f);

            cylinder.gameObject.GetComponent<CapsuleCollider>().transform.localScale = cylinder.gameObject.transform.localScale;
            cylinder.gameObject.GetComponent<CapsuleCollider>().transform.localPosition = cylinder.gameObject.transform.localPosition;
            cylinder.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;

            cylinder.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            cylinder.gameObject.GetComponent<Rigidbody>().useGravity = false;
        }
    }
	
	// Update is called once per frame
	void Update () {}

    // Place chimes in scene with appropriate materials.
    public void PositionCylinders()
    {
        int chimeCount = 0;
        foreach (Transform cylinder in chimes.transform)
        {
            cylinder.transform.localScale = new Vector3(0.0354f, chimeLengths[chimeCount], 0.0354f); // these are the x & z terms for the width of the chimes
            cylinder.transform.localPosition = new Vector3(0.75f * Mathf.Cos(0.1954f * chimeCount), chimeHeight[chimeCount] - chimeLengths[chimeCount], 0.75f * Mathf.Sin(0.1954f * chimeCount));

            int[] flats = { 2, 4, 7, 9, 11, 14, 16 };
            ArrayList sharps = new ArrayList(flats);

            //applies brass material to sharps & flats, silver to other notes.
            cylinder.gameObject.GetComponent<MeshRenderer>().material = sharps.Contains(chimeCount) ? Resources.Load("[free] Brass common") as Material : Resources.Load("silver") as Material;   

            chimeCount++;
        }

    }

}
