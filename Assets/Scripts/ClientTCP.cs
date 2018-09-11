using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

using System.Text.RegularExpressions;

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Networking.Connectivity;
using System.Threading.Tasks;
#endif
public class ClientTCP : MonoBehaviour
{
    public String message_display;
    public String message_roboter;
    public String message_convoyer;
    public String message_camera;
    public String message_misc;

#if !UNITY_EDITOR
    private StreamSocket socket;
    private Task exchangeTask;

    private Byte[] bytes = new Byte[256];
    private StreamWriter writer;
    private StreamReader reader;

    //public string message_display; // debug output here. using function ShowDebugInGame
    //public string message_convoyer;
    //public string message_camera;
    //public string message_roboter;
    
private async void ShowDebugInGame(String msg){
    //message_display = msg;
    message_misc = msg;
}
private async void Start()
    {
        ConnectToTcpServer();
    }

    private async void ConnectToTcpServer() {
        ShowDebugInGame("ConnectToTcpServer");
         try
        {
            socket = new StreamSocket();
            Windows.Networking.HostName serverHost = new Windows.Networking.HostName("192.168.137.1");
            await socket.ConnectAsync(serverHost, "4001");
            ShowDebugInGame("ConnectAsync");


            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            writer = new StreamWriter(streamOut) { AutoFlush = true };
        
            Stream streamIn = socket.InputStream.AsStreamForRead();
            
            reader = new StreamReader(streamIn);
            exchangeTask = Task.Run(() => ExchangePackets());

            //writer.Write("hololens here\n");
        }
        catch (Exception e)
        {
            // Debug.Log("On client connect exception " + e);
            ShowDebugInGame("On client connect exception:" + e);
        }
    }


    public async void ExchangePackets()
    {
        // ShowDebugInGame("ThreadStart::ExchangePackets");
        while(true){
            if (writer == null || reader == null) continue;

            string received = null;
            
            received = reader.ReadLine();
            
            ShowDebugInGame("Read data: " + received);


            writer.Write("ok\n");
  

            //parsecode
            string pattern = @".*#message-(.+):(.*)#.*";
            MatchCollection matches = Regex.Matches(received, pattern);

            foreach (Match match in matches)
            {

                string tempstr = match.Groups[2].Value.Replace("\\n", "\n");

                if (match.Groups[1].Value == "display")
                {
                    message_display = tempstr;
                } else if(match.Groups[1].Value == "convoyer")
                {
                    message_convoyer = tempstr;
                } else if(match.Groups[1].Value == "camera")
                {
                    message_camera = tempstr;
                } else if(match.Groups[1].Value == "roboter")
                {
                    message_roboter = tempstr;
                }
            }

                
        }
            
    }
#else
    // Use this for initialization
    void Start()
    {
        Debug.Log("hallo world");
        Debug.Log(message_display);
    }
#endif
}