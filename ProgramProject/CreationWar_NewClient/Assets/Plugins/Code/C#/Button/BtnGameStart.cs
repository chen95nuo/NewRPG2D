using UnityEngine;
using System.Collections;

public class BtnGameStart : MonoBehaviour {

    public yuan.YuanMemoryDB.YuanRow yr;
    public GameObject song;
	public GameObject CollderPlayer;
	public GameObject BtnEnter;
	public UILabel TiSlable;

    void OnClick()
    {
        song.SendMessage("StartGameYuan", yr, SendMessageOptions.DontRequireReceiver);
    }
	
	void OnDisable(){
		if(null!=BtnEnter){
		Vector3 btnp = BtnEnter.transform.localScale;
		BtnEnter.transform.localScale = new Vector3(1.1f,1.1f,1.1f);
		}
		if(null!=CollderPlayer){
		CollderPlayer.SetActive(false);
		}
		if(null!=TiSlable){
		TiSlable.enabled = true;
	}
	}
	void OnEnable(){
		if(null!=BtnEnter){
		Vector3 btnp = BtnEnter.transform.localScale;
		BtnEnter.transform.localScale = new Vector3(0,0,0);
			}
		if(null!=CollderPlayer){
		CollderPlayer.SetActive(true);
			}
		if(null!=TiSlable){
		TiSlable.enabled = false;
		}
	}
}
