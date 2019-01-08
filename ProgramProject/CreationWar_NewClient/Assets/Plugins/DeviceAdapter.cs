using UnityEngine;
using System.Collections;

public class DeviceAdapter : MonoBehaviour
{

    public static  bool deviceTypeFit = false;
    private string phoneID = "";
#if UNITY_IOS
    private static iPhoneGeneration phoneType;
#endif
    void Awake()
    {
        #if UNITY_IOS
        phoneID = SystemInfo.deviceUniqueIdentifier;
        phoneType = iPhone.generation;
        #endif
    }

	void Start () {
        // 测试代码
        // phoneType = type;
//		if (iPhone.generation == iPhoneGeneration.iPad3Gen ) {
//			Screen.SetResolution (800,600,true);
//		}
		//针对ipad 2 、3 、4 进行分泌率限制 1024*768
#if UNITY_IOS
		switch (iPhone.generation) {
		case iPhoneGeneration.iPad2Gen:
			Screen.SetResolution(1024,768,true);
			break;
		case iPhoneGeneration.iPad3Gen:
			Screen.SetResolution(1024,768,true);
			break;
		case iPhoneGeneration.iPad4Gen:
			Screen.SetResolution(1024,768,true);
			break;
		}
#endif
	}
	
	// Update is called once per frame
	void Update () {
        if (deviceTypeFit)
        { 
            deviceTypeFit = false;
            ShowPanel();
        }
	}

    public static bool DetectDevice()
    {
        bool result = false;

        #if UNITY_IOS
        if (iPhoneGeneration.Unknown < phoneType && phoneType < iPhoneGeneration.iPad2Gen)
        {
            result = true;
        }
        else if (phoneType ==  iPhoneGeneration.Unknown || phoneType >= iPhoneGeneration.iPad2Gen)
        {
            result = false;
        }
        #endif   

        return result;
    }

    void ShowPanel()
    {
        #if UNITY_IOS
        YuanUnityPhoton.GetYuanUnityPhotonInstantiate().DeviceType(phoneID, phoneType.ToString());
        BtnManager.my.warnings.warningAllEnter.btnEnter.target = this.gameObject;
        BtnManager.my.warnings.warningAllEnter.btnEnter.functionName = "EscapeApp";
        BtnManager.my.warnings.warningAllEnter.Show(StaticLoc.Loc.Get("buttons620"), StaticLoc.Loc.Get("info1066"));
       #endif
    }

    void EscapeApp()
    {
        Application.Quit();
    }
}
