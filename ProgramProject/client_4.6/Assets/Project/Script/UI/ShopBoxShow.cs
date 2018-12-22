using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShopBoxShow : MonoBehaviour {

    public GameObject dragPanel;            //draggable panel
    public GameObject cellParent;           //draggable panel parent
    public UILabel labelTitle;              //title

    public GameObject prefabBoxItem;        //prefab of GameBoxItem//
    public GameObject prefabBoxItem2;       //prefab of ItemIcon//

    List<GameObject> boxItems = new List<GameObject>();

    bool savePos;
    Vector3 dragPanelPos;
    Vector4 dragPanelClip;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //商店显示宝箱//
    public void show(int itemId)
    {
        adaptBox(1);

        labelTitle.text = TextsData.getData(593).chinese;

        List<GameBoxData> boxData = GameBoxData.getListData(itemId);
        foreach (GameBoxData data in boxData)
        {
            GameObject obj = Instantiate(prefabBoxItem) as GameObject;
            obj.transform.parent = cellParent.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(0.9f, 0.9f, 1);

            SimpleCardInfo2 card = obj.transform.FindChild("Card").GetComponent<SimpleCardInfo2>();
            UILabel labName = obj.transform.FindChild("Name").GetComponent<UILabel>();
            if (data.goodstpye == 1)			//item//
            {
                card.setSimpleCardInfo(data.itemid, GameHelper.E_CardType.E_Item);
                ItemsData itemD = ItemsData.getData(data.itemid);
                labName.text = itemD.name;
            }
            else if (data.goodstpye == 2)		//equip//
            {
                card.setSimpleCardInfo(data.itemid, GameHelper.E_CardType.E_Equip);
                EquipData ed = EquipData.getData(data.itemid);
                labName.text = ed.name;
            }
            else if (data.goodstpye == 3)		//card//
            {
                card.setSimpleCardInfo(data.itemid, GameHelper.E_CardType.E_Hero);
                CardData cd = CardData.getData(data.itemid);
                labName.text = cd.name;
            }

            boxItems.Add(obj);
        }

        if (!savePos)
            saveDragPanelPos();
        else
            resetDragPanelPos();
        if (boxItems.Count <= 9)
            enableDragContents();

        gameObject.SetActive(true);
    }

    //黑市显示精华物品//
    public void show()
    {
        adaptBox(2);

        labelTitle.text = TextsData.getData(622).chinese;

        List<BlackMarketData> boxData = BlackMarketData.getBlackTop();
        foreach (BlackMarketData data in boxData)
        {
            GameObject obj = Instantiate(prefabBoxItem2) as GameObject;
            obj.transform.parent = cellParent.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(0.6f, 0.6f, 1);

            obj.GetComponent<ItemIcon>().Init(data.goodstype, data.itemId, data.number, ItemIcon.ItemUiType.NameDown);

            boxItems.Add(obj);
        }

        if (!savePos)
            saveDragPanelPos();
        else
            resetDragPanelPos();
        if (boxItems.Count <= 9)
            enableDragContents();

        gameObject.SetActive(true);
    }

    void OnBtnCloseBox()
    {
        MusicManager.playEffectById(STATE.SOUND_EFFECT_ID_COMMON);

        for (int i = boxItems.Count - 1; i >= 0; i--)
        {
            GameObject obj = boxItems[i];
            boxItems.RemoveAt(i);
            Destroy(obj);
        }
        gameObject.SetActive(false);
    }

    void saveDragPanelPos()
    {
        savePos = true;
        dragPanelPos = dragPanel.transform.localPosition;
        dragPanelClip = dragPanel.GetComponent<UIPanel>().clipRange;
    }

    void resetDragPanelPos()
    {
        dragPanel.GetComponent<SpringPanel>().enabled = false;
        dragPanel.transform.localPosition = dragPanelPos;
        dragPanel.GetComponent<UIPanel>().clipRange = dragPanelClip;

        cellParent.GetComponent<UIGrid>().repositionNow = true;
    }

    void enableDragContents(bool flag = false)
    {
        foreach (GameObject obj in boxItems)
        {
            obj.GetComponent<UIDragPanelContents>().enabled = flag;
        }
    }

    void adaptBox(int type)
    {
        if (type == 1)
        {
            Vector4 vec4 = dragPanel.GetComponent<UIPanel>().clipRange;
            dragPanel.GetComponent<UIPanel>().clipRange = new Vector4(vec4.x, -5, vec4.z, vec4.w);

            Vector3 vec3 = cellParent.transform.localPosition;
            cellParent.transform.localPosition = new Vector3(vec3.x, 100, vec3.z);
        }
        else if (type == 2)
        {
            Vector4 vec4 = dragPanel.GetComponent<UIPanel>().clipRange;
            dragPanel.GetComponent<UIPanel>().clipRange = new Vector4(vec4.x, -10.5f, vec4.z, vec4.w);

            Vector3 vec3 = cellParent.transform.localPosition;
            cellParent.transform.localPosition = new Vector3(vec3.x, 80, vec3.z);
        }
    }
}
