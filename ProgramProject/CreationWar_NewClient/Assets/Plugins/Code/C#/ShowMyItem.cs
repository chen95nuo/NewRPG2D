using UnityEngine;
using System.Collections;

public class ShowMyItem : MonoBehaviour {

	public float delayTime ;
	public float speed ;
	public float moveDistance ;

	public UISprite showSpriteItem;
	public UILabel MyLbl ; 
	
	void OnEnable()
	{
		//StartCoroutine(DelayHide());
	}
	
	IEnumerator DelayHide(bool isSuccess)
	{
		yield return new WaitForSeconds(1.2f);
		float totalTime = 0;
		float alpha = 0;
		Vector3 endPos = Vector3.zero;
		if (isSuccess)
		{
			endPos = this.transform.localPosition + Vector3.up * moveDistance;
		}
		
		while(totalTime <= delayTime)
		{
			this.transform.localPosition = Vector3.Lerp(transform.localPosition, endPos, Time.deltaTime);
			alpha = showSpriteItem.alpha;
			alpha = MyLbl.alpha;
			alpha = Mathf.Lerp(alpha, 0, Time.deltaTime * 0.8f);
			showSpriteItem.alpha = alpha;
			MyLbl.alpha = alpha;
			
			totalTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		
		gameObject.SetActive(false);
		showSpriteItem.alpha = 1;
		MyLbl.alpha = 1;
	}

	public void ChangeSpriteNow(string SprName)
	{

//		showSpriteItem.spriteName = SprName;
		MyLbl.text = SprName;
		StartCoroutine(DelayHide(true));
	}

	public void SetAlpha()
	{
		showSpriteItem.alpha = 1;
		MyLbl.alpha = 1;
	}
}
