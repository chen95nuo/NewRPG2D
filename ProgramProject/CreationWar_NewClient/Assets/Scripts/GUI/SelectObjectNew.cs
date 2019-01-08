using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Camera))]
public class SelectObjectNew : MonoBehaviour {
	
	public int borderWidth = 3;
	public Color outterColor = new Color(0.133f,1,0,1);
	public Shader compositeShader;
    Material m_CompositeMaterial = null;
	protected Material compositeMaterial {
		get {
			if (m_CompositeMaterial == null) {
                m_CompositeMaterial = new Material(compositeShader);
				m_CompositeMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_CompositeMaterial;
		} 
	}
	
	public Shader blurShader;
    Material m_BlurMaterial = null;
	protected Material blurMaterial {
		get {
			if (m_BlurMaterial == null) {
                m_BlurMaterial = new Material(blurShader);
                m_BlurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_BlurMaterial;
		} 
	}
	
	private Material m_outterLineMat = null;
	protected Material outterLineMat{
		get{
			if(m_outterLineMat == null){
				m_outterLineMat = new Material("Shader\"Hidden/SolidBody1\"{SubShader{Pass{Color("+
					outterColor.r +","+outterColor.g +","+outterColor.b +","+outterColor.a +")}}}");
				m_outterLineMat.hideFlags = HideFlags.HideAndDontSave;
				m_outterLineMat.shader.hideFlags =  HideFlags.HideAndDontSave; 
			}
			return m_outterLineMat;
		}
	}

	public GameObject target;	
	private RenderTexture outterLineTexture = null;	
	Texture2D vit;
	public Material Minimap;

	void Start () {
//		yield return new WaitForSeconds(1);
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
		}
		else{
			if( !blurMaterial.shader.isSupported )
			enabled = false;
		if( !compositeMaterial.shader.isSupported )
			enabled = false;
		if( !outterLineMat.shader.isSupported )
			enabled = false;
		if(target)
		    camera.cullingMask = 1<< target.layer;
		    camera.clearFlags = CameraClearFlags.SolidColor;
	    	camera.farClipPlane = 300;
		    camera.backgroundColor = new Color(0, 0, 0, 0);
	    if(enabled){
		if(!outterLineTexture)
		{
//			outterLineTexture =  new RenderTexture( 256,256, 32);
//			outterLineTexture.hideFlags = HideFlags.DontSave;
		}
		
//		camera.targetTexture = outterLineTexture;
//		camera.RenderWithShader(outterLineMat.shader,"");
		}		
		 vit =renderImage();
		Minimap.mainTexture = vit;
		Destroy(target);
	}
	}
	
//	void OnRenderImage (RenderTexture source, RenderTexture destination)
//	{	if(enabled){
//		RenderTexture bufferFull = RenderTexture.GetTemporary(outterLineTexture.width, outterLineTexture.height, 0);		
//		Graphics.BlitMultiTap (outterLineTexture, bufferFull, blurMaterial,new Vector2(borderWidth,borderWidth));
//Graphics.Blit(source,destination);
//		Graphics.Blit(bufferFull,destination,compositeMaterial);		
//		RenderTexture.ReleaseTemporary(bufferFull);
//		}
//	}
	
	public Texture2D renderImage()
	{
		int screenSize = 256;
		RenderTexture rt = new RenderTexture (screenSize, screenSize, 32);
		camera.targetTexture = rt;
		Texture2D screenShot = new Texture2D (screenSize, screenSize, TextureFormat.ARGB32, false);
		camera.Render();
		RenderTexture.active = rt;
		screenShot.ReadPixels (new Rect (0, 0, screenSize, screenSize), 0, 0);
		screenShot.Apply (true,true);
		RenderTexture.active = null;
		camera.targetTexture = null;
		DestroyImmediate (rt);    
		return screenShot;
	}
	
	
}
