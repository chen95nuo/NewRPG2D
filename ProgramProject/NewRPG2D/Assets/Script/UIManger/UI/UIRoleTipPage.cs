using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRoleTipPage : TTUIPage
{

    private UIBagGrid CardGrid;

    public UIRoleTipPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIRoleTipPage";
    }

    public override void Awake(GameObject go)
    {
        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateRoleTipEvent, UpdateRole);

        this.gameObject.transform.Find("Enter").GetComponent<Button>().onClick.AddListener(ClosePage<UIRoleTipPage>);
        CardGrid = this.gameObject.transform.Find("UIItem").GetComponent<UIBagGrid>();
    }

    public void UpdateRole(CardData data)
    {
        Debug.Log(data.Id);
        float atk = Random.Range(data.AttackMinGrow, data.AttackMaxGrow + 1);
        int _atk = RoleGrade(atk, data.AttackMinGrow, data.AttackMaxGrow);

        float heal = Random.Range(data.HealthMinGrow, data.HealthMaxGrow + 1);
        int _heal = RoleGrade(heal, data.HealthMinGrow, data.HealthMaxGrow);

        float def = Random.Range(data.DefenseMinGrow, data.DefenseMaxGrow + 1);
        int _def = RoleGrade(def, data.DefenseMinGrow, data.DefenseMaxGrow);

        float agile = Random.Range(data.AgileMinGrow, data.AgileMaxGrow + 1);
        int _agile = RoleGrade(agile, data.AgileMinGrow, data.AgileMaxGrow);
        int index = Quality(_atk + _heal + _def + _agile);
        CardData newData = new CardData(data, atk, heal, def, agile, index);
        BagRoleData.Instance.AddItem(newData);
        CardGrid.UpdateItem(newData);

    }


    private int Quality(int index)
    {
        Debug.Log(index);
        if (index < 5)
        {
            return 1;
        }
        else if (index >= 5 && index < 9)
        {
            return 2;
        }
        else if (index >= 8 && index < 13)
        {
            return 3;
        }
        else if (index >= 13 && index < 17)
        {
            return 4;
        }
        else
        {
            Debug.Log(index);
            return 0;
        }
    }

    private int RoleGrade(float grade, float min, float max)
    {
        float Amin = 0;
        float Amax = 0;
        float Bmin = 0;
        float Bmax = 0;
        float Cmin = 0;
        float Cmax = 0;
        float Smin = 0;
        float Smax = 0;
        Cmin = min;
        Cmax = min + (max - min) / 10.0f * 3.0f;
        Bmin = Cmax + 0.1f;
        Bmax = Cmax + (max - min) / 10.0f * 3.0f;
        Amin = Bmax + 0.1f;
        Amax = Bmax + (max - min) / 10.0f * 3.0f;
        Smin = Amax + 0.1f;
        Smax = Amax + (max - min) / 10.0f * 1.0f;

        if (grade >= Cmin && grade <= Cmax)
        {
            return 1;
        }
        else if (grade >= Bmin && grade <= Bmax)
        {
            return 2;
        }
        else if (grade >= Amin && grade <= Amax)
        {
            return 3;
        }
        else if (grade >= Smin && grade <= Smax)
        {
            return 4;
        }
        else
        {
            return 0;
        }


    }
}
