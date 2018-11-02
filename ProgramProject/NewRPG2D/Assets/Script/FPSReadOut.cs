using UnityEngine;
using System.Collections;

namespace DPCommon
{
	/// <summary>
	/// Component to (very) roughly measure framerate and show it in attached GUIText.
	/// </summary>

	public class FPSReadOut : MonoBehaviour {
		public TextMesh text;
		private float updateInterval = 0.5f;
		private float timeleft; // Left time for current interval
		private float accum = 0.0f; // FPS accumulated over the interval
		private int frames = 0;
		
		// Use this for initialization
		void Start () 
		{
			if(FindObjectsOfType(typeof(FPSReadOut)).Length > 1)
			{
				Destroy(gameObject);	
			}
			else
			{
				Object.DontDestroyOnLoad(gameObject);
				timeleft = updateInterval;  
			}
		}
		
		// Update is called once per frame
		void Update () {
			timeleft -= Time.deltaTime;
		    accum += Time.timeScale/Time.deltaTime;
		    ++frames;
			
			// Interval ended - update GUI text and start new interval
		    if( timeleft <= 0.0f )
		    {
		        // display two fractional digits (f2 format)
		        text.text = "FPS: " + (accum/frames).ToString("f2");
		        timeleft = updateInterval;
		        accum = 0.0f;
		        frames = 0;
		    }
		}
	}
}