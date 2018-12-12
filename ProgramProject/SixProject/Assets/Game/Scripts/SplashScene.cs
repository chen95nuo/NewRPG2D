using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SplashScene : MonoBehaviour {

    public float changeTime;
    public string mainMenuScene = "";

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(ChangeScene());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(changeTime);
        SceneManager.LoadScene(mainMenuScene);
    }
}
