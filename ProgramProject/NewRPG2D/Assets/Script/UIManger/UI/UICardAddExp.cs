using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UICardAddExp : MonoBehaviour
{
    public Text playerName;
    public Text playerLevel;
    public Slider playerExpSlider;
    public Text playerExp;
    public Button btn_back;
    public Text playerNowExp;
    public Text levelUp;

    public UICardGrid grid;
    private List<UICardGrid> grids;

    private bool needRun = false;
    private bool closeGain = false;

    private int currentLevel = 0;
    private float currentExp = 0;
    private float maxExp = 0;
    private float currentMaxExp = 0;
    private float currentAddExp = 0;
    private float currentValue = 0;
    private float startTime = 0;

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

    private void OnEnable()
    {
        levelUp.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (needRun)
        {
            btn_back.interactable = false;
            currentValue = Mathf.Lerp(0, currentAddExp, (Time.time - startTime) * 0.5f);
            float index = (currentValue - currentMaxExp) + currentExp;

            if (index >= maxExp)
            {
                float _index = currentExp;
                currentExp = 0;
                index = 0;
                currentLevel++;
                currentMaxExp += (maxExp - _index);
                levelUp.gameObject.SetActive(true);
                levelUp.rectTransform.DOAnchorPos(Vector2.zero, 1.0f).From();

                maxExp = GetPlayerExpData.Instance.GetItem(currentLevel).NeedExp;
            }

            playerLevel.text = "Lv." + currentLevel;
            playerExpSlider.value = index;
            playerExpSlider.maxValue = maxExp;
            playerExp.text = "Exp+ " + (currentAddExp - currentValue).ToString("#0.0");
            playerNowExp.text = index.ToString("#0") + " / " + maxExp + "    " + currentMaxExp;

            if (currentValue == currentAddExp)
            {
                currentMaxExp = 0;
                currentValue = 0;

                needRun = false;
                levelUp.gameObject.SetActive(false);
                btn_back.interactable = true;
            }
        }
    }

    public void ChickBack()
    {
        if (closeGain)
        {
            TinyTeam.UI.TTUIPage.ClosePage<UIGainTipPage>();
            TinyTeam.UI.TTUIPage.ClosePage<UICardAddExpPage>();
            Globe.nextSceneName = GoFightMgr.instance.mainScene;
            SceneManager.LoadScene("FightLoadin");
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
        maxExp = GetPlayerExpData.Instance.GetItem(level).NeedExp;
        playerExpSlider.maxValue = maxExp;
        playerExpSlider.value = exp;
        playerExp.text = "Exp+ " + addExp;
        currentAddExp = addExp;
        currentLevel = level;
        currentExp = exp;
        needRun = true;
        startTime = Time.time;
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
