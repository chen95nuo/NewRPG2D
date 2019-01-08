/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipEnhance : MonoBehaviour 
{
    public static EquipEnhance instance;
    public Transform parent;

    void Awake()
    {
        instance = this;
    }

    public void ShowEquipEnhanceItem(bool isSuccess)
    {
        GameObject item = ObjectPool.Instance.Create(0, Vector3.zero, Quaternion.identity);
        item.transform.parent = parent;
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        item.transform.localScale = Vector3.one;
        EquipEnhanceItem equipItem = item.GetComponent<EquipEnhanceItem>();
        equipItem.SetAlpha();
        equipItem.ChangeSprite(isSuccess);
    }

	public void ShowMyItem(string a,string MySprite)
	{
		GameObject item = ObjectPool.Instance.Create(1, Vector3.zero, Quaternion.identity);
		item.transform.parent = parent;
		item.transform.localPosition = Vector3.zero;
		item.transform.localRotation = Quaternion.identity;
		item.transform.localScale = Vector3.one;
		ShowMyItem equipItem = item.GetComponent<ShowMyItem>();
		equipItem.SetAlpha();
		equipItem.ChangeSpriteNow(MySprite);
	}


}
