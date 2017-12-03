using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject camera;
    public GameObject light;
    private Material material;

    // Use this for initialization
    void Start ()
    {
        material = GetComponent<Renderer>().material;		
	}
	
	// Update is called once per frame
	void Update ()
    {
        material.SetVector("_CameraPosition", camera.transform.position);
        material.SetVector("_LightDirection", light.transform.forward);
    }
}
