using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FubiNET;

public class DepthImageVisualizer : MonoBehaviour
{	
	private bool m_disableTrackingImage = true;
	private bool reduceToHalfFramerate = true, depthImageUpdateCompleted = true;
	
	  // The depth texture
    private Texture2D m_depthMapTexture;
    // And its pixels
    Color[] m_depthMapPixels;
    // The raw image from Fubi
    byte[] m_rawImage;
    // m_factor for clinching the image
    int m_factor = 2;
    // Depthmap resolution
    int m_xRes = 640;
    int m_yRes = 480;

	private GameObject cam;
	
   // Initialization
    void Start()
    {       // Initialize debug image
        m_depthMapTexture = new Texture2D((int)(m_xRes / m_factor), (int)(m_yRes / m_factor), TextureFormat.RGBA32, false);
        m_depthMapPixels = new Color[(int)((m_xRes / m_factor) * (m_yRes / m_factor))];
        m_rawImage = new byte[(int)(m_xRes * m_yRes * 4)];
    }
	
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.D))
		{
			m_disableTrackingImage = !m_disableTrackingImage;
		}
	}
	
    // Update is called once per frame
    void FixedUpdate() 
    {
		if (m_disableTrackingImage) return;
		//display depth imgae for 1/2 times of frame rate
        reduceToHalfFramerate = !reduceToHalfFramerate;
		if(!reduceToHalfFramerate) return;
		
		if(!depthImageUpdateCompleted) return;
		
		SendMessage("UpdateDepthImageTexture",SendMessageOptions.DontRequireReceiver);		
    }

 
	void UpdateDepthImageTexture()
	{
		depthImageUpdateCompleted = false;
		
		// Get debug image
    	Fubi.getImage(m_rawImage, FubiUtils.ImageType.Depth, FubiUtils.ImageNumChannels.C4, FubiUtils.ImageDepth.D8);
        UpdateDepthMapTexture();
		
		depthImageUpdateCompleted = true;
	}
	
    // Upload the depthmap to the texture
    void UpdateDepthMapTexture()
    {
        int YScaled = m_yRes / m_factor;
        int XScaled = m_xRes / m_factor;
        int i = XScaled * YScaled - 1;
        int depthIndex = 0;
        for (int y = 0; y < YScaled; ++y)
        {
            depthIndex += (XScaled - 1) * m_factor * 4; // Skip lines
            for (int x = 0; x < XScaled; ++x, --i, depthIndex -= m_factor * 4)
            {
                m_depthMapPixels[i] = new Color(m_rawImage[depthIndex + 2] / 255.0f, m_rawImage[depthIndex + 1] / 255.0f, m_rawImage[depthIndex] / 255.0f, m_rawImage[depthIndex + 3] / 255.0f);
            }
            depthIndex += m_factor * (m_xRes + 1) * 4; // Skip lines
        }
        m_depthMapTexture.SetPixels(m_depthMapPixels);
        m_depthMapTexture.Apply();
    }

    // Called for rendering the gui
    void OnGUI()
    {
		if (!m_disableTrackingImage)
		{
	        // Debug image
	        
			GUI.depth = -4;
	        //GUI.Box(new Rect(Screen.width-m_xRes/m_factor, Screen.height-m_yRes/m_factor, m_xRes / m_factor, m_yRes / m_factor), m_depthMapTexture);
			GUI.Box(new Rect(0, Screen.height / 3, m_xRes / m_factor, m_yRes / m_factor), m_depthMapTexture);
		}
    }
}