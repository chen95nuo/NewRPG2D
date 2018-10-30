using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleEndPlayerInfoItem : MonoBehaviour
    {
        public Text nameTxt;
        public Image icon;
        public Slider HpSlider;

        public void SetItemInfo(string name, Sprite iconSprite, float HpPercent)
        {
            nameTxt.text = name;
           if(iconSprite) icon.sprite = iconSprite;
            HpSlider.value = HpPercent;
        }

    }
}
