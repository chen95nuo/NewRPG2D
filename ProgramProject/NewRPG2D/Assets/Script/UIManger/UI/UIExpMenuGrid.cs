using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExpMenuGrid : MonoBehaviour
{
    public Button btn_menu;
    public Image typeIcon;
    public Text teamNumber;
    public Slider timeSlider;
    public GameObject timeBG;
    public Text timeNumber;

    public Sprite[] Icon;
    public UIExplore explore;
    [System.NonSerialized]
    public ExpeditionTeam expedData;
    private bool isRun = false;

    private void Awake()
    {
        btn_menu.onClick.AddListener(ChickBtn);
    }
    public void UpdateMenu()
    {
        gameObject.SetActive(false);
    }
    public void UpdateMenu(ExpeditionTeam data)
    {
        gameObject.SetActive(true);
        expedData = data;

        switch (data.Id)
        {
            case 1: teamNumber.text = "A"; break;
            case 2: teamNumber.text = "B"; break;
            case 3: teamNumber.text = "C"; break;
            default:
                break;
        }
        switch (data.ExploreType)
        {
            case ExploreType.Nothing:
                typeIcon.sprite = Icon[0];
                timeBG.SetActive(false);
                timeSlider.value = 0;
                break;
            case ExploreType.Run:
                typeIcon.sprite = Icon[1];
                timeBG.SetActive(false);
                isRun = true;
                break;
            case ExploreType.End:
                isRun = false;
                timeSlider.value = timeSlider.maxValue;

                break;
            default:
                break;
        }
    }
    public void ChickBtn()
    {

    }
    public void ChickBtn(bool isTrue)
    {

    }
}
