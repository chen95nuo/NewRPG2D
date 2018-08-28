using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExpCardGrid : MonoBehaviour
{

    public Text levelTip;
    public Text needLevel;
    public Text currentLevel;
    public Button btn_AddCard;
    public SkeletonGraphic cardAnim;
    public GameObject levelType;
    public CardData cardData;

    public UIExplore explore;

    private void Awake()
    {
        btn_AddCard.onClick.AddListener(ChickAddCard);
    }
    public void UpdateCard()
    {
        cardData = null;
        gameObject.SetActive(true);
        levelType.SetActive(false);
        cardAnim.gameObject.SetActive(false);
        Color color = btn_AddCard.image.color;
        color.a = 1f;
        btn_AddCard.image.color = color;
    }
    public void UpdateCard(ExploreData data)
    {
        cardData = null;
        gameObject.SetActive(true);
        levelTip.gameObject.SetActive(true);
        needLevel.gameObject.SetActive(true);
        currentLevel.gameObject.SetActive(false);
        cardAnim.gameObject.SetActive(false);
        levelType.SetActive(true);
        needLevel.text = data.CaptainLevel.ToString();
        Color color = btn_AddCard.image.color;
        color.a = 1f;
        btn_AddCard.image.color = color;
    }

    public void UpdateCard(CardData data)
    {
        cardData = data;
        gameObject.SetActive(true);


        Destroy(cardAnim.gameObject);
        GameObject Anim = Resources.Load<GameObject>("UIPrefab/CardPrefab/" + data.AnimationName);
        GameObject GO = Instantiate(Anim, btn_AddCard.transform) as GameObject;
        cardAnim = GO.GetComponent<SkeletonGraphic>();

        levelType.SetActive(true);
        levelTip.gameObject.SetActive(false);
        needLevel.gameObject.SetActive(false);
        currentLevel.gameObject.SetActive(true);
        currentLevel.text = "lv." + data.Level;
        Color color = btn_AddCard.image.color;
        color.a = 0.01f;
        btn_AddCard.image.color = color;
    }

    private void ChickAddCard()
    {
        Debug.Log("ChickCard");
        explore.UpdateCard(this);
    }

    //public void
}
