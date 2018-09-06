using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Text.RegularExpressions;

//extra1
#if !UNITY_EDITOR
 //using System.IO;
 using Windows.Networking.Sockets;
 //using Windows.Storage.Streams;
 //using Windows.Networking;
 //using Windows.Networking.Connectivity;
#else
using System.Net.Sockets;
#endif

//extra 1 ende


public class ClientTCP : MonoBehaviour
{

    #region private members 	
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    public string message_display;
    public string message_convoyer;
    public string message_camera;
    public string message_roboter;
    #endregion
    // Use this for initialization 		
    void Start()
    {
        
        ConnectToTcpServer();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessage();
        }
    }
    /// <summary> 	
    /// Setup socket connection. 	
    /// </summary> 	
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }
    /// <summary> 	
    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            socketConnection = new TcpClient("localhost", 87);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading 				
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary. 					
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message. 						
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        message_display = serverMessage;
                        Debug.Log("server message received as: " + serverMessage);
                        
                        //parsecode
                        string pattern = @"#message-(.+):(.*)#";
                        //string input = "senden an client: test:abc#message-display:asdbasdasdasdasdasndasdasd\\nhallo\\nworld#";
                        MatchCollection matches = Regex.Matches(serverMessage, pattern);

                        foreach (Match match in matches)
                        {
                            Debug.Log("ist von:        " + match.Groups[1].Value);
                            Debug.Log("nachricht:        " + match.Groups[2].Value);
                            string tempstr = match.Groups[2].Value.Replace("\\n", "\n");
                            Debug.Log("tempstr:        " + tempstr);
                            if (match.Groups[1].Value == "display")
                            {
                                message_display = tempstr;
                            }
                            /*else if(match.Groups[1].Value == "display2")
                            {
                                message_display2 = tempstr;
                            }*/
                        }

                        //parscode ende


                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    /// <summary> 	
    /// Send message to server using socket connection. 	
    /// </summary> 	
    private void SendMessage()
    {
        if (socketConnection == null)
        {
            return;
        }
        try
        {
            // Get a stream object for writing. 			
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string clientMessage = "This is a message from one of your clients.";
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                Debug.Log("Client sent his message - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public override bool Equals(object obj)
    {
        var tCP = obj as ClientTCP;
        return tCP != null &&
               base.Equals(obj) &&
               EqualityComparer<TcpClient>.Default.Equals(socketConnection, tCP.socketConnection);
    }

    public override int GetHashCode()
    {
        var hashCode = 1703058482;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<TcpClient>.Default.GetHashCode(socketConnection);
        return hashCode;
    }
}
