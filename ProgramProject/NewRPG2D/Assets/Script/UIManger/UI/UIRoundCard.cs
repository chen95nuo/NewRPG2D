using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRoundCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler
{
    [System.NonSerialized]
    public int number;
    public UIBagGrid grid;
    public float maxTime = 2.0f;
    public bool isCard = false;

    private bool isDown = false;
    private float currentTime;


    public void Update()
    {
        if (isDown && isCard)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= maxTime)
            {
                Debug.Log("打开角色信息");
                //如果超过时间显示角色信息
                //TinyTeam.UI.TTUIPage.ShowPage<UIRolePage>();
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
        grid.gameObject.SetActive(true);
        grid.UpdateItem(data);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        currentTime = 0;
        if (isCard == false)
        {
            TinyTeam.UI.TTUIPage.ShowPage<UIUseRoleHousePage>();

            return;
        }
        isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        if (currentTime < maxTime)
        {
            //如果时间不足就放开的话 显示背包信息
            //TinyTeam.UI.TTUIPage.ShowPage<UIUseRoleHousePage>();
            //UIEventManager.instance.SendEvent<GridType>(UIEventDefineEnum.UpdateRolesEvent, GridType.Team);
            currentTime = 0;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("拖动");
    }
}
