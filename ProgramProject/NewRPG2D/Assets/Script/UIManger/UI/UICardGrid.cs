using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICardGrid : MonoBehaviour
{
    public Image quality;
    public Image cardImage;
    public Text cardLevel;
    public Slider cardExpSlider;
    public Text addExp;

    public void UpdateCardGrid(CardGainData data)
    {
        quality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        cardImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        cardLevel.text = "Lv." + data.Level;
        cardExpSlider.maxValue = data.MaxExp;
        cardExpSlider.value = data.CurrentExp;
        addExp.text = "Exp+ " + data.AddExp;
    }

    private void Update()
    {

    }

}
