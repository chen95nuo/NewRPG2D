using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRoundCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [System.NonSerialized]
    public int number;
    public Canvas canvas;
    public UIBagGrid grid;
    public Transform temporary;

    private RectTransform rt;
    private Image gridImage;
    private float maxTime = 0.8f;
    public bool isCard = false;
    private bool isDown = false;
    private bool isDrag = false;
    private float currentTime;
    private CardData currentData; //用于交换卡牌信息
    public CardData cardData; //当前位置的卡牌信息

    private void Awake()
    {
        canvas = TTUIRoot.Instance.GetComponent<Canvas>();
        rt = grid.GetComponent<RectTransform>();
        gridImage = grid.GetComponent<Image>();
    }

    public void Update()
    {
        if (isDown && isCard)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= maxTime)
            {
                Debug.Log("打开角色信息");
                //如果超过时间显示角色信息
                TinyTeam.UI.TTUIPage.ShowPage<UIRolePage>();
                UIEventManager.instance.SendEvent<CardData>(UIEventDefineEnum.UpdateCardMessageEvent, cardData);
                isDown = false;
                currentTime = 0;
            }
        }
    }

    public void UpdateRoundCard()
    {
        grid.gameObject.SetActive(false);
    }

    public void UpdateRoundCard(CardData data)
    {
        cardData = data;
        if (isCard)//如果需要替换 则将原卡初始化
        {
            currentData.TeamPos = 0;
            currentData.TeamType = TeamType.Nothing;
        }
        isCard = true;
        grid.gameObject.SetActive(true);
        grid.UpdateItem(data);
        currentData = data;
    }

    public void UpdateRoundCard(UIRoundCard data)
    {
        if (data != null)
        {
            //有卡交换 没卡覆盖
            if (data.isCard)
            {
                Debug.Log("有卡");
                CardData index = data.currentData;
                Debug.Log(index.Name);
                currentData.TeamPos = data.number;
                data.grid.UpdateItem(currentData);
                data.currentData = currentData;
                Debug.Log(index.Name);
                index.TeamPos = number;
                grid.UpdateItem(index);
                currentData = index;
            }
            else
            {
                Debug.Log("没卡");
                currentData.TeamPos = data.number;
                data.UpdateRoundCard(currentData);
                isCard = false;
                UpdateRoundCard();
            }

        }



    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentTime = 0;
        if (isCard == false)
        {
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateRoundEvent, number);

            return;
        }
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDown && currentTime < maxTime)
        {
            isDown = false;
            //如果时间不足就放开的话 显示背包信息
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateRoundEvent, number);
            currentTime = 0;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        grid.transform.SetParent(temporary);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDown = false;
        isDrag = true;

        Vector2 _pos = Vector2.one;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                    Input.mousePosition, canvas.worldCamera, out _pos);
        rt.anchoredPosition = _pos;
        gridImage.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //交换卡牌
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        UIRoundCard card = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIRoundCard>();
        UpdateRoundCard(card);

        grid.transform.SetParent(transform);
        rt.anchoredPosition = Vector2.zero;
    }

}
