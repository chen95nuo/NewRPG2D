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
        btn_menu.onClick.AddListener(ChickMenu);
    }
    private void Update()
    {
        if (isRun)
        {
            SystemTime.insatnce.TimeNormalized((int)expedData.NowTime, timeNumber);
            timeSlider.value = -expedData.NowTime;
        }
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
                timeSlider.value = timeSlider.minValue;
                break;
            case ExploreType.Run:
                typeIcon.sprite = Icon[1];
                timeBG.SetActive(true);
                timeSlider.maxValue = 0;
                timeSlider.minValue = -expedData.MaxTime;
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
    public void ChickMenu()
    {
        explore.UpdateMenu(this);
    }
    public void ChickMenu(bool isTrue)
    {
        btn_menu.interactable = isTrue;
    }
}
