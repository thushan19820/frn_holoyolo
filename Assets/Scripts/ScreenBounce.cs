using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [MIT License](https://opensource.org/licenses/MIT)
 
 
// Makes objects float up & down while gently spinning.
public class ScreenBounce : MonoBehaviour
{
    // User Inputs
    //public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public GameObject screen;
    private Vector3 offset;
    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start()
    {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        offset = transform.position - screen.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Spin object around Y-Axis
        //transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude * 0.01f;

        transform.position = screen.transform.position + tempPos;
    }
}
