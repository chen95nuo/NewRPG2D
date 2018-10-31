using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Utility.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Battle.BattleUI
{
    public class BattleEndAwardItem : MonoBehaviour
    {
        public Text nameTxt;
        public Image icon;
        public GameObject counObj;

        public void SetBoxInfo(Sprite iconSprite, int count)
        {
            icon.sprite = iconSprite;
            if (count <= 1)
            {
                counObj.gameObject.CustomSetActive(false);
            }
            else
            {
                counObj.gameObject.CustomSetActive(true);
                nameTxt.text = count.ToString();
            }
        }
    }
}
