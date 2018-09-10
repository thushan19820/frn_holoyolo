using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

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
    message_display = msg;
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
            await socket.ConnectAsync(serverHost, "3000");
        
            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            writer = new StreamWriter(streamOut) { AutoFlush = true };
        
            Stream streamIn = socket.InputStream.AsStreamForRead();
            reader = new StreamReader(streamIn);
            exchangeTask = Task.Run(() => ExchangePackets());
        }
        catch (Exception e)
        {
            // Debug.Log("On client connect exception " + e);
            ShowDebugInGame("On client connect exception:" + e);
        }
    }


    public void ExchangePackets()
    {
        ShowDebugInGame("ThreadStart::ExchangePackets");
        while(true){
            if (writer == null || reader == null) continue;
            ShowDebugInGame("write x to network");
            writer.Write("X\n");
            ShowDebugInGame("write x to network.Done");
            string received = null;
            received = reader.ReadLine();
            // Debug.Log("Read data: " + received);
            ShowDebugInGame("Read data: " + received);

            // TODO: verteilen der msg auf display-boxen
            // Das hier spaeter testen, wenn wir  wissen das der obere code funktioniert.
            /*

            //parsecode
            string pattern = @"#message-(.+):(.*)#";
            MatchCollection matches = Regex.Matches(received, pattern);

            foreach (Match match in matches)
            {
                // Debug.Log("ist von:        " + match.Groups[1].Value);
                // Debug.Log("nachricht:        " + match.Groups[2].Value);
                string tempstr = match.Groups[2].Value.Replace("\\n", "\n");
                ShowDebugInGame("tempstr: " + tempstr)
                // Debug.Log("tempstr:        " + tempstr);
                if (match.Groups[1].Value == "display")
                {
                    message_display = tempstr;
                } else if(match.Groups[1].Value == "convoyer")
                {
                    message_convoyer = tempstr;
                }
                } else if(match.Groups[1].Value == "camera")
                {
                    message_camera = tempstr;
                }
                } else if(match.Groups[1].Value == "roboter")
                {
                    message_roboter = tempstr;
                }

                */
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