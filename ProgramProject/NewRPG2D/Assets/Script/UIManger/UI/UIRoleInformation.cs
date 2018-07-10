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
    public UIRoleEquip roleWeapon;
    public UIRoleEquip roleArmor;
    public UIRoleEquip roleNecklace;
    public UIRoleEquip roleRing;

    public UIRoleAttribute roleHealth;
    public UIRoleAttribute roleAttack;
    public UIRoleAttribute roleAgile;
    public UIRoleAttribute roleDefense;

    public UIRoleSkill role1Skill;
    public UIRoleSkill role2Skill;
    public Text roleSkillLevel;
    public Text roleSkillExp;
    public Slider roleSkillExpSlider;


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


