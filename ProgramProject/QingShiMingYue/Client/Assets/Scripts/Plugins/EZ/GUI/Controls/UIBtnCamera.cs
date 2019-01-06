using UnityEngine;
using System.Collections;


/// <remarks>
/// Button camera class that allows you to render object to this button.
/// </remarks>
[AddComponentMenu("EZ GUI/Controls/ButtonCamera")]
public class UIBtnCamera : UIButton
{
	public Camera captureCamera;
	public int cameraTexureWidth = 4;
	public int cameraTexureHeight = 4;
	//public bool releaseRenderTextureWhenDisabled = true;

	public override void Start()
	{
		if ( m_started )
			return;
		
		base.Start();
		
		// Runtime init stuff:
		if( Application.isPlaying )
		{
			if ( captureCamera == null )
			{
				captureCamera = gameObject.AddComponent<Camera>();
				captureCamera.backgroundColor = new Color( 0, 0, 0, 0 );
				captureCamera.clearFlags = CameraClearFlags.SolidColor;
				captureCamera.orthographic = false;
				captureCamera.orthographicSize = height / 2.0f;
				captureCamera.fieldOfView = 64f;
			}

			if (renderer.material == null)
			{
				renderer.material = new Material(Shader.Find("Extra/UITransparent"));
			}

			captureCamera.aspect = (float)width / (float)height;
			SetTexSize(cameraTexureWidth, cameraTexureHeight);
		}
	}

	//protected override void OnDisable()
	//{
	//    base.OnDisable();

	//    if (releaseRenderTextureWhenDisabled && captureCamera != null && captureCamera.targetTexture != null)
	//    {
	//        Debug.Log("Release Render Texture : " + gameObject.name);
	//        captureCamera.targetTexture.Release();
	//    }
	//}

	public void SetTexSize(int w, int h)
	{
		if (renderer.material == null)
			return;

		RenderTexture tex = new RenderTexture(w, h, 16, RenderTextureFormat.ARGB32);
		renderer.material.mainTexture = tex;
		captureCamera.targetTexture = tex;

		Rect rt = new Rect(0, 0, 1, 1);
		SetUVs(rt);

		// Update animation.
		if (animations != null && animations.Length > 0)
		{
			foreach (UVAnimation uvAnim in animations)
			{
				SPRITE_FRAME[] frms = new SPRITE_FRAME[uvAnim.GetFrameCount()];

				for (int i = 0; i < uvAnim.GetFrameCount(); i++)
				{
					frms[i] = uvAnim.GetFrame(i);
					frms[i].uvs = rt;
				}

				uvAnim.SetAnim(frms);
			}

			bool active = gameObject.activeInHierarchy;
			if (!active)
				gameObject.SetActive(true);

			PlayAnim(0);

			if (!active)
				gameObject.SetActive(true);
		}
	}
}

