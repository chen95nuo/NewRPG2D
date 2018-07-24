using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHatcheryGrid : MonoBehaviour
{

    public Button btn_hatchery;
    public Button btn_speed;
    public Button btn_complete;
    public Text timeText;
    public Image lockImage;

    public void Awake()
    {
        btn_hatchery.onClick.AddListener(ChickHatcheryPool);
        btn_speed.onClick.AddListener(ChickAddSpeed);
        btn_complete.onClick.AddListener(ChickComplete);

    }

    public void UpdateGrid()
    {
        lockImage.gameObject.SetActive(true);
        btn_speed.gameObject.SetActive(false);
        btn_complete.gameObject.SetActive(false);
    }

    public void UpdateGrid(HatcheryData data)
    {

    }

    public void ChickHatcheryPool()
    {

    }

    public void ChickAddSpeed() { }
    public void ChickComplete() { }

}
