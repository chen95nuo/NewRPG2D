using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Battle;
using UnityEngine;

namespace Assets.Script.Utility.Tools
{
   public class SpriteHelper
    {
       public static Sprite CreateSprite(Texture2D texture)
       {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width, texture.height) * 0.5f);
        }
        public static Sprite CreateSprite(string texturePath)
        {
            Texture2D texture = ResourcesLoadSys.instance.LoadResource<Texture2D>(texturePath);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width, texture.height) * 0.5f);
        }
    }
}
