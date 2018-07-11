using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRoleInformation : MonoBehaviour
{
    public Text roleName;
    public Text roleLevel;
    public Text roleExp;
    public Slider roleExpSlider;
    public Text roleHeart;
    public Image role;
    public Image roleQuality;
    public Image roleAttribute;
    public Image roleStars;

    public UIRoleEquip[] roleEquip;

    public UIRoleAttribute roleHealth;
    public UIRoleAttribute roleAttack;
    public UIRoleAttribute roleAgile;
    public UIRoleAttribute roleDefense;

    public UIRoleSkill role1Skill;
    public UIRoleSkill role2Skill;
    public Text roleSkillLevel;
    public Text roleSkillExp;
    public Slider roleSkillExpSlider;

    public CardData roleData;

    public void updateMessage(CardData data)
    {
        roleData = data;
        roleName.text = data.Name;
        roleLevel.text = data.Level.ToString();
        roleExp.text = data.Exp.ToString();
        roleHeart.text = data.GoodFeeling.ToString();
        role.sprite = Resources.Load<Sprite>("UITexture/Icon/role/" + data.Name);
        roleQuality.sprite = Resources.Load<Sprite>("UITexture/Icon/roleQuality/" + data.Quality);
        roleAttribute.sprite = Resources.Load<Sprite>("UITexture/Icon/attribute/" + data.Attribute);
        roleStars.sprite = Resources.Load<Sprite>("UITexture/Icon/stars/" + data.Stars);
        for (int i = 0; i < roleEquip.Length; i++)
        {
            if (data.Equipdata != null)
            {
                if (data.Equipdata.Length > i)
                {
                    roleEquip[i].roleEquipImage.gameObject.SetActive(true);
                    roleEquip[i].roleEquipQuality.gameObject.SetActive(true);
                    roleEquip[i].roleEquipImage.sprite = Resources.Load<Sprite>("UITexture/Icon/equip/" + data.Equipdata[i].Id);
                    roleEquip[i].roleEquipQuality.sprite = Resources.Load<Sprite>("UITexture/Icon/quality/" + data.Equipdata[i].Quality);
                }
                else
                {
                    roleEquip[i].roleEquipImage.gameObject.SetActive(false);
                    roleEquip[i].roleEquipQuality.gameObject.SetActive(false);
                }
            }
            else
            {
                roleEquip[i].roleEquipImage.gameObject.SetActive(false);
                roleEquip[i].roleEquipQuality.gameObject.SetActive(false);
            }
        }
        roleHealth.roleValue.text = data.Health.ToString();
        roleHealth.roleScore.text = data.HealthGrow.ToString();
        roleAttack.roleValue.text = data.Attack.ToString();
        roleAttack.roleScore.text = data.AttackGrow.ToString();
        roleAgile.roleValue.text = data.Agile.ToString();
        roleAgile.roleScore.text = data.AgileGrow.ToString();
        roleDefense.roleValue.text = data.Defense.ToString();
        roleDefense.roleScore.text = data.DefenseGrow.ToString();




    }


    [System.Serializable]
    public class UIRoleEquip
    {
        public Image roleEquipImage;
        public Image roleEquipQuality;
    }
    [System.Serializable]
    public class UIRoleAttribute
    {
        public Text roleValue;
        public Text roleScore;
        public Image roleQuality;
    }
    [System.Serializable]
    public class UIRoleSkill
    {
        public Image roleSkillBG;
        public Image roleSkillImage;
        public Text roleSkillLevel;
    }
}


