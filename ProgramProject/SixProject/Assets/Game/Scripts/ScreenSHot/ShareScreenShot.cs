using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace MadFireOn
{

    public class ShareScreenShot : MonoBehaviour
    {
        public static ShareScreenShot instance;

        private bool isProcessing = false;

#if UNITY_ANDROID
        public string AndroidUrl = "";
#elif UNITY_IOS
        public string iosUrl = "";
#endif
        public string subject = "Your game name";
        public GameObject sharePanel;
        public Text shareScoreText;
        private string shareScore;

        string destination;

        void Awake()
        {
            if (instance == null)
                instance = this;
        }

        void Start()
        {

        }

        public void CurrentScoreShareBtn()
        {
            shareScoreText.text = "" + GameManager.instance.lastScore;
            shareScore = "" + GameManager.instance.lastScore;
            sharePanel.SetActive(true);
        }

        public void BestScoreShareBtn()
        {
            shareScoreText.text = "" + GameManager.instance.bestScore;
            shareScore = "" + GameManager.instance.bestScore;
            sharePanel.SetActive(true);
        }

        //function called from a button
        public void ButtonShare()
        {
            if (!isProcessing)
            {
                StartCoroutine(ShareScreenshot());
            }
        }


        public IEnumerator ShareScreenshot()
        {

            isProcessing = true;
            // wait for graphics to render
            yield return new WaitForEndOfFrame();
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
            // create the texture
            Texture2D screenTexture = new Texture2D(Screen.width, (int)(Screen.height / 2.5f), TextureFormat.RGB24, true);
            // put buffer into texture
            screenTexture.ReadPixels(new Rect(0f, Screen.height * 0.3f, Screen.width, (Screen.height / 2.5f)), 0, 0);
            // apply
            screenTexture.Apply();
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
            byte[] dataToSave = screenTexture.EncodeToPNG();
            destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
            File.WriteAllBytes(destination, dataToSave);
            ShareMethode();
            isProcessing = false;
        }

        void ShareMethode()
        {
            if (!Application.isEditor)
            {
#if UNITY_ANDROID
                // block to open the file and share it ------------START
                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
                AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
                intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

                intentObject.Call<AndroidJavaObject>("setType", "text/plain");
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "Bet my Score " + shareScore + " download " + AndroidUrl);
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");

                intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

                currentActivity.Call("startActivity", intentObject);
#elif UNITY_IOS
	CallSocialShareAdvanced("Bet my Score " + shareScore, subject, iosUrl, destination);
#else
	Debug.Log("No sharing set up for this platform.");
#endif
            }
        }

#if UNITY_IOS
public struct ConfigStruct
{
	public string title;
	public string message;
}

[DllImport ("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);
	
public struct SocialSharingStruct
{
	public string text;
	public string url;
	public string image;
	public string subject;
}
	
[DllImport ("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);
	
public static void CallSocialShare(string title, string message)
{
	ConfigStruct conf = new ConfigStruct();
	conf.title  = title;
	conf.message = message;
	showAlertMessage(ref conf);
}

public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url, string img)
{
	SocialSharingStruct conf = new SocialSharingStruct();
	conf.text = defaultTxt; 
	conf.url = url;
	conf.image = img;
	conf.subject = subject;
		
	showSocialSharing(ref conf);
}
#endif

        void Update()
        {
            //this is for the android default back button *Important google feature requirement
            if (Input.GetKeyDown(KeyCode.Escape) && sharePanel.activeInHierarchy)
            {
                sharePanel.SetActive(false);
            }
        }

        public void HomeBtn()
        {
            sharePanel.SetActive(false);
        }

    }//class
}//namespace