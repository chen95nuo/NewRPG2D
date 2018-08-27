using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UICardGrid : MonoBehaviour
{
    public Image quality;
    public Image cardImage;
    public Text cardLevel;
    public Slider cardExpSlider;
    public Text addExp;
    public Text nowExp;
    public Text levelUp;

    private bool needRun = false;

    private int currentLevel = 0;
    private float currentExp = 0;
    private float maxExp = 0;
    private float currentMaxExp = 0;
    private float currentAddExp = 0;
    private float currentValue = 0;
    private float startTime = 0;

    public void UpdateCardGrid(CardGainData data)
    {
        quality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        cardImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        cardLevel.text = "lv." + data.Level;
        maxExp = GameCardExpData.Instance.GetItem(data.Level).NeedExp;
        cardExpSlider.maxValue = data.MaxExp;
        cardExpSlider.value = data.CurrentExp;
        addExp.text = "+" + data.AddExp;
        currentAddExp = data.AddExp;
        currentLevel = data.Level;
        currentExp = data.CurrentExp;
        needRun = true;
        startTime = Time.time;
    }

    private void OnEnable()
    {
        levelUp.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (needRun)
        {
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

                maxExp = GameCardExpData.Instance.GetItem(currentLevel).NeedExp;
            }

            cardLevel.text = "Lv." + currentLevel;
            cardExpSlider.value = index;
            cardExpSlider.maxValue = maxExp;
            addExp.text = "Exp+ " + (currentAddExp - currentValue).ToString("#0.0");
            nowExp.text = index.ToString("#0") + " / " + maxExp;

            if (currentValue == currentAddExp)
            {
                currentMaxExp = 0;
                currentValue = 0;

                needRun = false;
                levelUp.gameObject.SetActive(false);
            }
        }
    }

}
