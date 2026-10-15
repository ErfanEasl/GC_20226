using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startpos;
    public GameObject cam;
    public float parallaxEffect; // The speed at which the background should move relative to the camera
    
    void Start()
    {
        startpos = transform.position.x;
    }

    void FixedUpdate() 
    {
        // Calculate distance background move baseed on cam movment
        float distance = cam.transform.position.x * parallaxEffect; //0 = no movement, 1 = won't move , 0.5 = half the speed of the camera
        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);
    }
}
