using UnityEngine;
using System.Collections;

public class AnnocementContents : MonoBehaviour {
	//private string url = "http://noah.cmge.com/cp/GameBulletin_Throne.html";
	//private string url = "http://47.92.130.102:80/cp/GameBulletin_Throne.html";

#if UNITY_IOS
    #if SDK_ZSYIOS
        private string url = "http://47.92.130.102:80/cp/AppleGameBulletin.htm";
    #else
        private string url = "http://47.92.130.102:80/cp/GameBulletin_Throne.htm";
    #endif
#elif UNITY_ANDROID
	private string url = "http://47.92.130.102:80/cp/1.2.6Android/GameBulletin.htm";
#else
		private string url = "http://47.92.130.102:80/cp/GameBulletin_Throne.htm";
#endif

    public UILabel annocementLabel;
	// Use this for initialization
	void Start () {
        StartCoroutine(ShowAnnocementContents());
	}

    IEnumerator ShowAnnocementContents()
    {
        Debug.Log("url------------------------------" + url);
        WWW www = new WWW(url);
        yield return www;
        annocementLabel.text = www.text;
    }
}
