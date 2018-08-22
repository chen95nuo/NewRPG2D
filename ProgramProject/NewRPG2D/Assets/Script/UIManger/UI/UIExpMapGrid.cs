using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExpMapGrid : MonoBehaviour
{
    public Button btn_map;
    public Text mapName;
    public Text needTime;
    public Text NeedFatigue;
    public Button btn_Start;
    public Image lockImage;
    public Image BG;
    public GameObject timeBG;

    public Sprite lockSprite;

    public UIExplore explore;


    private void Awake()
    {
        btn_map.onClick.AddListener(ChickBtn);
        btn_Start.onClick.AddListener(ChickStart);
    }

    public void UpdateMapGrid()
    {
        timeBG.SetActive(false);
        BG.gameObject.SetActive(false);
        btn_Start.gameObject.SetActive(false);
        mapName.gameObject.SetActive(false);
        lockImage.gameObject.SetActive(true);
        btn_map.image.sprite = lockSprite;
    }

    public void UpdateMapGrid(ExploreData data, Sprite mapSprite)
    {
        timeBG.SetActive(true);
        BG.gameObject.SetActive(true);
        mapName.gameObject.SetActive(true);
        lockImage.gameObject.SetActive(false);
        btn_Start.gameObject.SetActive(false);
        mapName.text = data.Name;
        needTime.text = data.NeedTime.ToString();
        NeedFatigue.text = data.NeedFatigue.ToString();
        btn_map.image.sprite = mapSprite;
    }
    public void ChickMap(bool isTrue)
    {
        BG.gameObject.SetActive(!isTrue);
        btn_Start.gameObject.SetActive(isTrue);
    }
    private void ChickBtn()
    {
        explore.UpdateMap(this);
    }
    private void ChickStart()
    {

    }



}
