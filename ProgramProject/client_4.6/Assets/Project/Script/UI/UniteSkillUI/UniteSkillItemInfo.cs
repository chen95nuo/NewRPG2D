using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UniteSkillItemInfo : MonoBehaviour {
	
	public UILabel skillDes;
	
	public GameObject[] cardIcon;
	
	public UISprite uniteSkillIcon;
	
	public UILabel uniteSkillName;
	
	public UILabel angerNum;
	
	public UIButtonMessage ubm;
	
	private bool isUse;
	
	private int uniteSkillId;
	
	public TweenAlpha isUseTween;
	
	private bool isUnlockUniteSkill;
	/// <summary>
	/// 选择技能按钮//
	/// </summary>/
	public GameObject selectUniteSkill;
	/// <summary>
	/// 查看出处按钮//
	/// </summary>
	public GameObject whereHaveNeedCard;
	
	private Color grey = new Color(0.5f,0.5f,0.5f,1);
	
	void Awake()
	{
		skillDes.text = string.Empty;
		uniteSkillName.text = string.Empty;
		angerNum.text = string.Empty;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public bool getIsUse()
	{
		return isUse;
	}
	
	public void setIsUse(bool isUse)
	{
		this.isUse = isUse;
	}
	
	public int getUniteSkillId()
	{
		return uniteSkillId;
	}
	
	public bool GetIsCanUnlock(int uniteSkillId)
	{
		List<int> cardIdList = UnitSkillData.getUniteSkillAllNeedCardId(uniteSkillId);
		for(int i = 0;i<cardIdList.Count;i++)
		{
			if(cardIdList[i]!=0)
			{
				bool mark;
				try
				{
					mark = UniteSkillInfo.cardUnlockTable[cardIdList[i]];
				}
				catch(KeyNotFoundException)
				{
					return false;
				}
				if(mark == false)
				{
					return false;
				}
			}
		}
		return true;
	}
	
	/// <summary>
	/// Sets the data.
	/// </summary>
	/// <param name='uniteSkillId'>
	/// Unite skill identifier.
	/// </param>
	/// <param name='target'>
	/// Target.
	/// </param>
	/// <param name='curUniteSkill'>
	/// 判断是不是现在正在用的技能//
	/// </param>
	public void SetData(int uniteSkillId,GameObject target,bool curUniteSkill = false)
	{
		UnitSkillData usd =  UnitSkillData.getData(uniteSkillId);
		List<int> cardIdList = UnitSkillData.getUniteSkillAllNeedCardId(uniteSkillId);
		isUnlockUniteSkill = GetIsCanUnlock(uniteSkillId);
		for(int i=0;i<cardIdList.Count;i++)
		{
			bool mark;
			int cardId = cardIdList[i];
			try
			{
				if(cardId == 0)
				{
					cardIcon[i].SetActive(false);	
				}
				else
				{
					cardIcon[i].SetActive(true);
					cardIcon[i].GetComponent<SimpleCardInfo2>().setSimpleCardInfo(cardId,GameHelper.E_CardType.E_Hero);
				}
				mark = UniteSkillInfo.cardUnlockTable[cardId];
			}
			catch(KeyNotFoundException)
			{
				mark = false;
			}
			catch(IndexOutOfRangeException)
			{
				break;
			}
			if(mark)
			{
				cardIcon[i].transform.FindChild("Child/HeroIcon").GetComponent<UISprite>().color = Color.white;
				cardIcon[i].transform.FindChild("Child/Frame").GetComponent<UISprite>().color = Color.white;
				//解锁显示头像为原色//
			}
			else
			{
				cardIcon[i].transform.FindChild("Child/HeroIcon").GetComponent<UISprite>().color = grey;
				cardIcon[i].transform.FindChild("Child/Frame").GetComponent<UISprite>().color = grey;
				//未解锁显示头像灰色//
			}
		}
		
		if(isUnlockUniteSkill)
		{
			selectUniteSkill.SetActive(true);
			whereHaveNeedCard.SetActive(false);
			uniteSkillIcon.color = Color.white;
		}
		else
		{
			selectUniteSkill.SetActive(false);
			whereHaveNeedCard.SetActive(true);
			whereHaveNeedCard.GetComponent<UIButtonMessage>().target = target;
			whereHaveNeedCard.GetComponent<UIButtonMessage>().param = uniteSkillId;
			whereHaveNeedCard.GetComponent<UIButtonMessage>().functionName = "uniteSkillClick";
			angerNum.gameObject.SetActive(false);
			//uniteSkillIcon.color = grey;
		}
		
		if(curUniteSkill)
		{
			isUseTween.from = 0;
			isUseTween.to = 1;
			isUseTween.PlayForward();
			isUse = true;
		}
		else
		{
			isUseTween.from = 1;
			isUseTween.to = 0;
			isUseTween.PlayForward();
		}
		
		uniteSkillIcon.spriteName = usd.icon;
		skillDes.text = usd.description;
		uniteSkillName.text = usd.name;
		angerNum.text = usd.cost.ToString();
		ubm.param = uniteSkillId;
		ubm.target = target;
		ubm.functionName = "OnSelectUniteSkill";
		this.uniteSkillId = uniteSkillId;
	}
}
