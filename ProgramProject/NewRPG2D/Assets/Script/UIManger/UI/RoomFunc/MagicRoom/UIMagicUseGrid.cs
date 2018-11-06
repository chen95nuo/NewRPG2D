using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMagicUseGrid : MonoBehaviour
{

    public Image Icon;
    public Button btn_Click;
    public RealMagic currentMagic;
    private int type = 0;

    private void Awake()
    {
        btn_Click = GetComponent<Button>();
        btn_Click.onClick.AddListener(ChickClick);
    }

    public void UpdateInfo()
    {
        Icon.enabled = false;
        currentMagic = null;
    }

    public void UpdateInfo(RealMagic data, int type)
    {
        currentMagic = data;
        Icon.enabled = true;
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.magic.magicName.ToString());

    }

    private void ChickClick()
    {
        UIMagicWorkShop.instance.ChickUseBtn(currentMagic);
    }
}
