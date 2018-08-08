using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.EventSystems;

public class UIBagPage : TTUIPage
{

    private SpriteAtlas BagImage;

    public UIBagPage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UIBag";
    }

    public override void Awake(GameObject go)
    {
        BagImage = Resources.Load<SpriteAtlas>("UISpriteAtlas/BagImage");
        SetImage();
    }

    private void SetImage()
    {
        Image[] images = transform.GetComponentsInChildren<Image>(true);
        GetSpriteAtlas.insatnce.SetImage(images);
        GetSpriteAtlas.insatnce.SetImage(images, BagImage);
    }

}
