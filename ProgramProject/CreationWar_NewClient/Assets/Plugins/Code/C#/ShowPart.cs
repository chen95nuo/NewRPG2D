using UnityEngine;
using System.Collections;

public class ShowPart : MonoBehaviour {

	public static ShowPart showPart;
	public GameObject objPart;
//	public GameObject objAnimation;
	// Use this for initialization
	void Start () {
		showPart = this;
	}

	// Update is called once per frame
	public void ClickBtn()
	{
		StartCoroutine(ShowMePart());
	}

	IEnumerator ShowMePart()
	{
		objPart.SetActive(false);
//		objAnimation.SetActive(false);
		yield return new WaitForSeconds(0.5f);
		objPart.SetActive(true);
//		objAnimation.SetActive(true);
//		yield return new WaitForSeconds(0.6f);
//		objAnimation.SetActive(false);
	}
}
