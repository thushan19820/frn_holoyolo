using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Networking.Connectivity;

#endif
public class ClientTCP : MonoBehaviour
{
    public String message_display;
#if !UNITY_EDITOR
private async void Start()
    {

        Windows.Networking.Sockets.StreamSocket socket = new Windows.Networking.Sockets.StreamSocket();
        Windows.Networking.HostName server = new Windows.Networking.HostName("172.18.0.137");
        string serverPort = "8000";
        await socket.ConnectAsync(server, serverPort);

        Stream streamOut = socket.OutputStream.AsStreamForWrite();
        StreamWriter writer = new StreamWriter(streamOut);
        string message = "Hallo Server";
        message_display = message;
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();



    }
#else
    // Use this for initialization
    void Start()
    {
        Debug.Log("hallo wolrd");
        Debug.Log(message_display);
    }
#endif
}