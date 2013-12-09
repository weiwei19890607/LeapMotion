using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



public class testServer : MonoBehaviour
{
	private System.Threading.Thread myThread;
    public bool doit = false;
    public float upsi = 0.0F;
    float posi = 0.0F;
    // Use this for initialization
	public static bool running = true;
    void Start()
    {
        TCPServerThread myTCPServer = new TCPServerThread(this);
        myThread = new System.Threading.Thread(new ThreadStart(myTCPServer.myTCPServer));
        myThread.Start();

        GlobalClass.EDA = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (doit)
        {
            Debug.Log("Eyecoordinates are: " + GlobalClass.eyeCoordinate_x + " " + GlobalClass.eyeCoordinate_y);
            doit = false;
        }   
    }
	void OnApplicationQuit()
	{
		Debug.Log ("Application quit");
		running = false;
	}
}

public class TCPServerThread
{

    testServer trackerServer;

    public TCPServerThread(testServer c)
    {
        trackerServer = c;
    }

    public void myTCPServer()
    {
        float tmp_EDA = 0f;
        string input, stringData;
        byte[] message = new byte[128];
        int bytesRead;
		TcpClient myClient;
		myClient = null;
Debug.Log("running tcp s");
        TcpListener myListener = new TcpListener(IPAddress.Any, 9998);
		Debug.Log("running tcp s2");
		myListener.Start();
        
		while(testServer.running) {
			
			if(!myListener.Pending ())
			{
				System.Threading.Thread.Sleep (500);
				continue;
			}			
			myClient = myListener.AcceptTcpClient();
			break;
		}
		if(myClient==null){
			myListener.Stop();
			return;
		}
		Debug.Log("running tcp s3");
        NetworkStream myClientStream = myClient.GetStream();
		Debug.Log("running tcp s4");
        while (true)
        {
            bytesRead = 0;
            try
            {

                bytesRead = myClientStream.Read(message, 0, 128);
            }
            catch (Exception e)
            {
                Debug.Log("Caugth exception : " + e);
                break;
            }
            // If we received 0 bytes the connection was closed.

            if (bytesRead == 0)
            {
                Debug.Log("Connection closed.");
                break;
            }
            stringData = Encoding.ASCII.GetString(message, 0, bytesRead);
            string[] words = stringData.Split(':');
            try
            {
                GlobalClass.eyeCoordinate_x = (float)Convert.ToDouble(words[0]);
                GlobalClass.eyeCoordinate_y = (float)Convert.ToDouble(words[1]);
                Debug.Log("tracker coordinates are: " + GlobalClass.eyeCoordinate_x + "," + GlobalClass.eyeCoordinate_y);
            }
            catch (Exception e)
            {
                Debug.Log("Vituiks man" + e.ToString());
				continue;
            }
        }
		myClient.Close();
   //     Console.WriteLine("Disconnecting...");
 //     myListener.Shutdown(SocketShutdown.Both);
   //    myListener.Close();
    }
}
