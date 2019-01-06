//#define USE_APPLICATION_CAPTURE_SCREEN
#define USE_RENDERTARGET_CAPTURE_SCREEN
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenCapture : MonoBehaviour
{
	public delegate void AfterCaptureScreenDelegate(string screenShotPath, object param);

	public int maxScreenshotImageSize = 512;

	public bool Capturing { get { return capturing; } }
	private bool capturing = false;
	private AfterCaptureScreenDelegate afterCaptureScreenDelegate;
	private object param;

#if !UNITY_WEBPLAYER
	public bool CaptureScreen(string imageName, AfterCaptureScreenDelegate del, object param)
	{
		if (Capturing == true)
			return false;

		// Free memory for capture screen shot (need 8m memory)
		GameObjectPoolManager.Singleton.FreeMemory();
		System.GC.Collect();

		this.afterCaptureScreenDelegate = del;
		this.param = param;
		this.capturing = true;

		StopCoroutine("DoCaptureScreen");
		StartCoroutine("DoCaptureScreen", imageName);

		return true;
	}

#if USE_APPLICATION_CAPTURE_SCREEN
	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	IEnumerator DoCaptureScreen(string imageName)
	{
		string filePath = Application.persistentDataPath + "/" + imageName;
		if (System.IO.File.Exists(filePath))
			System.IO.File.Delete(filePath);
		Debug.Assert(System.IO.File.Exists(filePath) == false, "Internal Error");
		
		Application.CaptureScreenshot(imageName);
		
		const int MaxWaitFrameCount = 100;
		int waitFrameCount = 0;
		
		
		while (true)
		{
			if (System.IO.File.Exists(filePath))
				break;
			
			if (waitFrameCount >= MaxWaitFrameCount)
			{
				Debug.Log("File not exists");
				break;
			}
			
			waitFrameCount++;
//			yield return null;
			yield return new WaitForEndOfFrame();
		}
		
		// Call degelgate
		if (afterCaptureScreenDelegate != null)
			afterCaptureScreenDelegate(param);
		
		afterCaptureScreenDelegate = null;
		param = null;
		capturing = false;
	}
	
#elif USE_RENDERTARGET_CAPTURE_SCREEN
	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	IEnumerator DoCaptureScreen(string imageName)
	{
		yield return new WaitForEndOfFrame();

		// Make texture size to fix max size
		int resWidth = Screen.width;
		int resHeight = Screen.height;

		if (resWidth > resHeight)
		{
			if (resWidth > maxScreenshotImageSize)
			{
				resHeight = (int)((float)resHeight * maxScreenshotImageSize / resWidth);
				resWidth = maxScreenshotImageSize;
			}
		}
		else
		{
			if (resHeight > maxScreenshotImageSize)
			{
				resWidth = (int)((float)resWidth * maxScreenshotImageSize / resHeight);
				resHeight = maxScreenshotImageSize;
			}
		}

		// Get valid cameras
		List<Camera> cameras = new List<Camera>();
		cameras.Add(Camera.main);

		GameObject[] gos = GameObject.FindGameObjectsWithTag("Camera");
		foreach (var go in gos)
		{
			Camera camera = go.GetComponent<Camera>();
			if (camera != null)
				cameras.Add(camera);
		}

		cameras.Sort(CompareCamera);

		// Render to texture
		RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);

		foreach (var camera in cameras)
		{
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
			if (camera.gameObject.activeInHierarchy == false)
#else
			if (camera.gameObject.active == false)
#endif
				continue;

			if (camera.enabled == false)
				continue;

			camera.targetTexture = rt;
			camera.Render();
			camera.targetTexture = null;
		}

		// Get screen shot
		RenderTexture.active = rt;
		Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
		screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

		// Release render texture
		RenderTexture.active = null; // JC: added to avoid errors
		Resources.UnloadAsset(rt);
		rt = null;

		// Save to file
		byte[] bytes = screenShot.EncodeToPNG();

		// Release screen shot memory
		Resources.UnloadAsset(screenShot);
		screenShot = null;

		string filePath = Application.persistentDataPath + "/" + imageName;
		Debug.Log("Saving screen shot : " + filePath);

		try
		{
			System.IO.File.WriteAllBytes(filePath, bytes);
		}
		catch (System.Exception e)
		{
			Debug.LogError(string.Format("Save screen shot failed ({0}): {1}.", e.Message, filePath));
		}

		// Collect memeory
		bytes = null;
		System.GC.Collect();

		// Call degelgate
		if (afterCaptureScreenDelegate != null)
		{
#if UNITY_IPHONE
			afterCaptureScreenDelegate(imageName, param);
#else
			afterCaptureScreenDelegate(filePath, param);
#endif
		}

		afterCaptureScreenDelegate = null;
		param = null;
		capturing = false;
	}

	int CompareCamera(Camera camera1, Camera camera2)
	{
		return (int)(camera1.depth - camera2.depth);
	}

#else
	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	IEnumerator DoCaptureScreen(string imageName)
	{
		yield return new WaitForEndOfFrame();
	
		// Save screenshot
		int width = Screen.width;
		int height = Screen.height;
		
		Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
		screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		screenShot.Apply();
		
		// Save to file
		byte[] bytes = screenShot.EncodeToPNG();
		
		// Release screen shot memory
		Resources.UnloadAsset(screenShot);
		screenShot = null;
				
		string filePath = Application.persistentDataPath + "/" + imageName;
		Debug.Log("Saving screen shot : " + filePath);
		
		try 
		{
			System.IO.File.WriteAllBytes(filePath, bytes);
		}
		catch (System.Exception e)
		{
			Debug.LogError(string.Format("Save screen shot failed ({0}): {1}.", e.Message, filePath));
		}
		
		// Collect memeory
		bytes = null;
		System.GC.Collect();
		
		// Call degelgate
		if (afterCaptureScreenDelegate != null)
			afterCaptureScreenDelegate(param);
		
		afterCaptureScreenDelegate = null;
		param = null;
		capturing = false;
	}
#endif

	private enum TextureRotateType
	{
		Rotate_90,
		Rotate_180,
		Rotate_270
	}

	private Texture2D RotateImage(Texture2D srcTexture, TextureRotateType rotateType)
	{
		if (rotateType == TextureRotateType.Rotate_90)
		{
			Texture2D rotatedTexture = new Texture2D(srcTexture.height, srcTexture.width);
			for (int y = 0; y < rotatedTexture.height; ++y)
			{
				for (int x = 0; x < rotatedTexture.width; ++x)
				{
					rotatedTexture.SetPixel(x, y, srcTexture.GetPixel(srcTexture.width - y, x));
				}
			}

			rotatedTexture.Apply();

			return rotatedTexture;
		}
		else if (rotateType == TextureRotateType.Rotate_180)
		{
			Texture2D rotatedTexture = new Texture2D(srcTexture.width, srcTexture.height);
			for (int y = 0; y < rotatedTexture.height; ++y)
			{
				for (int x = 0; x < rotatedTexture.width; ++x)
				{
					rotatedTexture.SetPixel(x, y, srcTexture.GetPixel(x, srcTexture.height - y));
				}
			}

			rotatedTexture.Apply();

			return rotatedTexture;
		}
		else if (rotateType == TextureRotateType.Rotate_270)
		{
			Texture2D rotatedTexture = new Texture2D(srcTexture.height, srcTexture.width);
			for (int y = 0; y < rotatedTexture.height; ++y)
			{
				for (int x = 0; x < rotatedTexture.width; ++x)
				{
					rotatedTexture.SetPixel(x, y, srcTexture.GetPixel(y, srcTexture.height - x));
				}
			}

			rotatedTexture.Apply();

			return rotatedTexture;
		}
		else
		{
			return null;
		}
	}
#else

	public bool CaptureScreen(string imageName, AfterCaptureScreenDelegate del, object param)
	{
		return false;
	}
	
#endif
}
