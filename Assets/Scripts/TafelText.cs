using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TafelText : MonoBehaviour {

    public ClientTCP tcp_handler;
    // Use this for initialization
    void Start()
    {
        GetComponent<TextMesh>().text = "Display: offline";
    }

    // Update is called once per frame
	void Update () {
        GetComponent<TextMesh>().text = tcp_handler.message_display;
    }
    
}
