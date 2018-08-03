using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardAddExp : MonoBehaviour
{
    public Text playerName;
    public Text playerLevel;
    public Slider playerExpSlider;
    public Text playerExp;
    public Button btn_back;

    public UICardGrid grid;
    private List<UICardGrid> grids;

    private bool needRun = false;
    private bool closeGain = false;


    private void Awake()
    {
        grid.gameObject.SetActive(false);
        grids = new List<UICardGrid>();
        btn_back.onClick.AddListener(ChickBack);
        UIEventManager.instance.AddListener<CardGainData[]>(UIEventDefineEnum.UpdateGainTipEvent, UpdateCardExp);
    }
    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<CardGainData[]>(UIEventDefineEnum.UpdateGainTipEvent, UpdateCardExp);
    }

    private void Update()
    {
        if (needRun)
        {

        }
    }

    public void ChickBack()
    {
        if (closeGain)
        {
            TinyTeam.UI.TTUIPage.ClosePage<UIGainTipPage>();
            TinyTeam.UI.TTUIPage.ClosePage<UICardAddExpPage>();
        }
        else
        {
            TinyTeam.UI.TTUIPage.ClosePage<UICardAddExpPage>();
        }
    }

    public void UpdatePlayerExp(int level, float exp, float addExp)
    {
        playerName.text = GetPlayData.Instance.player[0].Name;
        playerLevel.text = "Lv." + level;
        playerExpSlider.maxValue = GetPlayerExpData.Instance.GetItem(level).NeedExp;
        playerExpSlider.value = exp;
        playerExp.text = "Exp+ " + addExp;
        needRun = true;
    }

    public void UpdateCardExp(CardGainData[] datas)
    {
        GridsControl(datas.Length);
        UpdatePlayerExp(datas[0].PlayerLevel, datas[0].PlayerExp, datas[0].PlayerAddExp);
        for (int i = 0; i < datas.Length; i++)
        {
            grids[i].UpdateCardGrid(datas[i]);
        }

        closeGain = true;
    }


    private void GridsControl(int number)
    {
        if (number > grids.Count)
        {
            for (int i = 0; i < grids.Count; i++)
            {
                grids[i].gameObject.SetActive(true);
            }
            int index = grids.Count;
            for (int i = index; i < number; i++)
            {
                GameObject go = Instantiate(grid.gameObject, grid.transform.parent.transform) as GameObject;
                go.SetActive(true);
                grids.Add(go.GetComponent<UICardGrid>());
            }
        }
        else if (number < grids.Count)
        {
            for (int i = number; i < grids.Count; i++)
            {
                grids[i].gameObject.SetActive(false);
            }
        }

    }
}
