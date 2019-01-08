using UnityEngine;
using System.Collections;

public class LoginSDKManager : MonoBehaviour {

    private static bool canSDKLogin=false;
	public static bool CanSDKLogin {
		get {
			return canSDKLogin;
		}
		set {
			canSDKLogin = value;
		}
	}
	
	private static string sdkID=string.Empty;
	public static string SdkID {
		get {
			return sdkID;
		}
		set {
			sdkID = value;
		}
	}
	
	public static string sdkToken=string.Empty;

	public static string SdkToken {
		get {
			return sdkToken;
		}
		set {
			sdkToken = value;
		}
	}
}
