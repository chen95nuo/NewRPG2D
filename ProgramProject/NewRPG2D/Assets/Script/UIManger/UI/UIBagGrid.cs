using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBagGrid : MonoBehaviour
{

    public int itemID = -1;
    public ItemType itemType;

    public GameObject Type;

    public Image itemImage;
    public Text itemNumber;

    // Use this for initialization
    void Awake()
    {
        UpdateItem(-1, "", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateItem(int itemID, string spriteName, int itemNumber)
    {
        this.itemID = itemID;

        if (itemID >= 0)//存在物品
        {
            itemImage.gameObject.SetActive(true);
            Sprite sp = Resources.Load<Sprite>("UITexture/Icon/" + spriteName);
            itemImage.sprite = sp;
            this.itemNumber.text = itemNumber.ToString();
        }
        else
        {
            Type.gameObject.SetActive(false);
        }
    }
}
