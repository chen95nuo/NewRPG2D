/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class TrainingViplevel : MonoBehaviour 
{
	public static TrainingViplevel instance;
	public UILabel vip1;
	public UILabel vip2;
    //public UILabel lblShiBing;
    //public UILabel lblYongShi;
    //public UILabel lblQiShi;
    //public UILabel lblDuJun;

	void Awake()
	{
		instance = this;
	}

	void OnEnable()
	{
        PanelStatic.StaticBtnGameManager.RunOpenLoading(() => InRoom.GetInRoomInstantiate().ShowTrainingInfo());
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

    public int v1 = 0;
    public int v2 = 0;
    public void SetVipLabel(int v1, int v2)
    {
        vip1.text = v1.ToString();
        vip2.text = v2.ToString();
        this.v1 = v1;
        this.v2 = v2;

        gameObject.SendMessage("ShowTrainingVip", SendMessageOptions.DontRequireReceiver);
    }

    public int shiBing = 0;
    public int yongShi = 0;
    public int qiShi = 0;
    public int duJun = 0;
    public void SetBtnInfo(int shiBing, int yongShi, int qiShi, int duJun)
    {
        //lblShiBing.text = shiBing.ToString();
        //lblYongShi.text = yongShi.ToString();
        //lblQiShi.text = qiShi.ToString();
        //lblDuJun.text = duJun.ToString();
        this.shiBing = shiBing;
        this.yongShi = yongShi;
        this.qiShi = qiShi;
        this.duJun = duJun;
    }
}
