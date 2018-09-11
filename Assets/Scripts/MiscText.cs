using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscText : MonoBehaviour {

    public ClientTCP tcp_handler;

	// Use this for initialization
	void Start () {
        GetComponent<TextMesh>().text = "b";
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<TextMesh>().text = tcp_handler.message_misc;
    }
   
}
