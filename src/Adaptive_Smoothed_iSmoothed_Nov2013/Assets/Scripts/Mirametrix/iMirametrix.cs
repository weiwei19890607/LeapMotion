/*Imtiaj Ahmed
 * University of Helsinki
 * 2013
 * 
 * access data from mirametrix eye tracker
 * */
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class iMirametrix : MonoBehaviour {
	
	private bool running = true;

    private String str;

    private  TcpClient m_client;
    private  NetworkStream m_stream;
    private  StreamWriter m_stream_write;
	// Use this for initialization
	IEnumerator Start () {
		Debug.Log("before mira");
	StartCoroutine(UpdateTrackingData());
		
		Debug.Log("after mira");
		yield return null;
	}
	
	IEnumerator tes()
	{
		Debug.Log("tes");
		yield return null;
	}
	
	IEnumerator UpdateTrackingData()
	{
		// Connect to server if possible
      try
      {
        m_client = new TcpClient("127.0.0.1", 4242);
      }
      catch (Exception e)
      {
        Debug.Log("Unable to connect to server: {0}"+ e);
        running = false;
      }
	if(running !=false)
		{

      Debug.Log("Connected to server");

      m_stream = m_client.GetStream();
      m_stream_write = new StreamWriter(m_stream);

      // Setup server data stream
      m_stream_write.Write("<SET ID=\"ENABLE_SEND_COUNTER\" STATE=\"1\" />\r\n");
      m_stream_write.Flush();
      m_stream_write.Write("<SET ID=\"ENABLE_SEND_POG_FIX\" STATE=\"1\" />\r\n");
      m_stream_write.Flush();

      // Start sending
      m_stream_write.Write("<SET ID=\"ENABLE_SEND_DATA\" STATE=\"1\" />\r\n");
      m_stream_write.Flush();

      str = "";

      do
      {
		
        /*
          if (Input.GetKeyUp(KeyCode.Q)) { running = false; }

          // Calibrate
          if (Input.GetKeyUp(KeyCode.M))
          {
            m_stream_write.Write("<SET ID=\"CALIBRATE_SHOW\" STATE=\"1\" />\r\n");
            m_stream_write.Flush();
            m_stream_write.Write("<SET ID=\"CALIBRATE_START\" STATE=\"1\" />\r\n");
            m_stream_write.Flush();
          }
        }
*/
        // Show data
        int i = m_stream.ReadByte(); // Inefficient to do byte by byte, better to read in bulk 

        if (i == -1) { running = false; } // Other side has closed socket 
         else
        {
          // Add char to build up data string
          str = str + (char)i;

          // If string ends in \r\n then output
          if (str.Length > 2 && str.Substring(str.Length-2, 2) == "\r\n")
          {
				int xIndx = str.IndexOf("FPOGX=");
				if(xIndx>-1)
				{
					float x =(float)Convert.ToDouble(str.Substring(xIndx+7,7));
					Debug.Log("FPOGX="+x);
					GlobalClass.eyeCoordinate_x = x;
				}
				int yIndx = str.IndexOf("FPOGY=");
				if(xIndx>-1)
				{
					float y =(float)Convert.ToDouble(str.Substring(yIndx+7,7));
					Debug.Log("FPOGY="+y);
					GlobalClass.eyeCoordinate_y = y;
				}
            // process string (print and clear)
            Debug.Log(str);
            str = ""; 
          }
        }
			yield return null;
      }
      while (running == true);

      m_stream_write.Close();
      m_stream.Close();
			}
      m_client.Close();
		
		
		yield return null;
    }
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
