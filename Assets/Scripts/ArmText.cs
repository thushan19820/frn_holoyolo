using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmText : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponent<TextMesh>().text = "Roboterarm: offline";
    }

    /* Update is called once per frame
	void Update () {
		
	}
    */
}
