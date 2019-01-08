

// QualityManager sets shader LOD's and enabled/disables special effects
// based on platform and/or desired quality settings.

// Disable 'autoChoseQualityOnStart' if you want to overwrite the quality
// for a specific platform with the desired level.

#pragma strict

@script ExecuteInEditMode
@script RequireComponent (Camera)
@script RequireComponent (ShaderDatabase)

// Quality enum values will be used directly for shader LOD settings

enum Quality {
	Lowest = 100,
	Low = 200,
	Medium = 210,
	High = 250,
	Mataas = 300
}

public var autoChoseQualityOnStart : boolean = true;
public var currentQuality : Quality = Quality.High;

public var bloom : MobileBloom;
public var shaders : ShaderDatabase;

public static var quality : Quality = Quality.High;

function Awake () {
	if (!bloom)
		bloom = GetComponent.<MobileBloom> ();
	if (!shaders)
		shaders = GetComponent.<ShaderDatabase> ();

	if (autoChoseQualityOnStart) 
		AutoDetectQuality ();	

	ApplyAndSetQuality (currentQuality);
}

// we support dynamic quality adjustments if in edit mode

#if UNITY_EDITOR

function Update () {
	var newQuality : Quality = currentQuality; 
	if (newQuality != quality) 
		ApplyAndSetQuality (newQuality);	
}

#endif

private function AutoDetectQuality ()
// Some special quality settings cases for various platforms
{
	#if UNITY_IPHONE
	
		switch (iPhone.generation)
		{
			case iPhoneGeneration.iPad1Gen:
			case iPhoneGeneration.iPodTouch4Gen:
			case iPhoneGeneration.iPhone4:
			case iPhoneGeneration.iPhone3GS:
				currentQuality = Quality.Low;
			break;
			
			case iPhoneGeneration.iPad2Gen:
			case iPhoneGeneration.iPad4Gen:
			case iPhoneGeneration.iPad5Gen:
			case iPhoneGeneration.iPadMini2Gen:
			case iPhoneGeneration.iPhone5:
			case iPhoneGeneration.iPhone5C:
			case iPhoneGeneration.iPhone5S:
			case iPhoneGeneration.iPodTouch5Gen:			
				currentQuality = Quality.High;
			break;

			case iPhoneGeneration.iPhone4S:
			case iPhoneGeneration.iPadMini1Gen:
			case iPhoneGeneration.iPad3Gen:
				currentQuality = Quality.Medium;
			break;
			case iPhoneGeneration.iPhoneUnknown:
				currentQuality = Quality.Mataas;
			break;
			
			default:
				currentQuality = Quality.High;
			break;
		}
		
	#elif UNITY_ANDROID

//		try{
//			Start_getArguments.getQuality();
//		}catch(e){
			currentQuality = Quality.Medium;		
//		}

	#else
	// Desktops/consoles
	
		switch (Application.platform)
		{
			case RuntimePlatform.NaCl:
				currentQuality = Quality.High;
			break;
			case RuntimePlatform.FlashPlayer:
				currentQuality = Quality.Low;
			break;
			default:
				currentQuality = SystemInfo.graphicsPixelFillrate < 2800 ? Quality.High : Quality.Mataas;
			break;
		}

	#endif

	Debug.Log (String.Format (
		"Blade of Darkness: Quality set to '{0}'{1}",
		currentQuality,
		#if UNITY_IOS
			" (" + iPhone.generation + " class iOS)"
		#elif UNITY_ANDROID
			" (Android)"
		#else
			" (" + Application.platform + ")"
		#endif
	));
}

function returnAndroid(qua : String){
//	print("AndroidQuality == " + qua);
		switch (qua)
		{
			case "1" :
				currentQuality = Quality.Lowest;
				break;	
			case "2":			
				currentQuality = Quality.Lowest;
				break;
			case "3":
				currentQuality = Quality.Low;
				break;
			case "4":
				currentQuality = Quality.Medium;
				break;
			default:
				currentQuality = Quality.Low;
			break;
		}
}

private function ApplyAndSetQuality (newQuality : Quality) {	
	quality = newQuality;
	// default states	
//	camera.cullingMask = -1 & ~(1 << LayerMask.NameToLayer ("Adventure"));	
			
	if (quality == Quality.Lowest) {
		DisableAllFx ();					
		EnableFx (bloom, false);	
        RenderSettings.fog = false;											
		camera.depthTextureMode = DepthTextureMode.None;
		QualitySettings.SetQualityLevel (0, true);
	}
	else if (quality == Quality.Low) {				
		EnableFx (bloom, false);		
        RenderSettings.fog = false;	
		camera.depthTextureMode = DepthTextureMode.None;	
		QualitySettings.SetQualityLevel (0, true);					
	} 
	else if (quality == Quality.Medium) {				
		EnableFx (bloom, false);	
       RenderSettings.fog = true;						
		camera.depthTextureMode = DepthTextureMode.None;
		QualitySettings.SetQualityLevel (1, true);										
	} 
	else if (quality == Quality.High) {				
		EnableFx (bloom, false);		
       RenderSettings.fog = true;	
		camera.depthTextureMode = DepthTextureMode.None;
		QualitySettings.SetQualityLevel (2, true);							
	} 	
	else if (quality == Quality.Mataas) {				
		EnableFx (bloom, true);	
       RenderSettings.fog = true;	
		camera.depthTextureMode = DepthTextureMode.None;
		QualitySettings.SetQualityLevel (3, true);							
	} 	
//	Debug.Log ("Blade of Darkness: setting shader LOD to " + quality);
	
//	Shader.globalMaximumLOD = 200;
//	for (var s : Shader in shaders.shaders) {
//		s.maximumLOD = 200;	
//	}
}

private function DisableAllFx () {
	camera.depthTextureMode = DepthTextureMode.None;			
	EnableFx (bloom, false);
    RenderSettings.fog = false;					
}

private function EnableFx (fx : MonoBehaviour, enable : boolean) {
	if (fx)
		fx.enabled = enable;
}