using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Battle;
using UnityEngine;
using UnityEngine.U2D;

namespace Assets.Script.Utility.Tools
{

    public enum SpriteAtlasTypeEnum
    {
        Icon,
    }

    public class SpriteHelper : TSingleton<SpriteHelper>
    {
        public static Sprite CreateSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        public static Sprite CreateSprite(string texturePath)
        {
            if (string.IsNullOrEmpty(texturePath)) return null;
            Texture2D texture = ResourcesLoadMgr.instance.LoadResource<Texture2D>(texturePath);
            if (texture == null) return null;
            return CreateSprite(texture);
        }

        private Dictionary<SpriteAtlasTypeEnum, SpriteAtlas> iconImageDic;

        private Dictionary<SpriteAtlasTypeEnum, string> spritePathDic = new Dictionary<SpriteAtlasTypeEnum, string>
        {
            {SpriteAtlasTypeEnum.Icon, "UISpriteAtlas/MonsterIcon"}
        };

        public override void Init()
        {
            base.Init();
            iconImageDic = new Dictionary<SpriteAtlasTypeEnum, SpriteAtlas>();
            SpriteAtlasTypeEnum defaultSpriteAtlasType = SpriteAtlasTypeEnum.Icon;
            SetIconImageDic(defaultSpriteAtlasType);
        }

        public Sprite GetIcon(SpriteAtlasTypeEnum spriteAtlasType, string name)
        {
            SpriteAtlas tempSpriteAtlas = null;
            if (iconImageDic.TryGetValue(spriteAtlasType, out tempSpriteAtlas) == false)
            {
                tempSpriteAtlas = SetIconImageDic(spriteAtlasType);
            }
            var sprite = tempSpriteAtlas.GetSprite(name);

            if (sprite != null)
            {
                return sprite;
            }
            else
            {
                return null;
            }
        }

        private SpriteAtlas SetIconImageDic(SpriteAtlasTypeEnum spriteAtlasType)
        {
            SpriteAtlas tempSpriteAtlas = null;
            string defaultPath = spritePathDic[spriteAtlasType];
            tempSpriteAtlas = Resources.Load<SpriteAtlas>(defaultPath);
            iconImageDic[spriteAtlasType] = tempSpriteAtlas;
            Sprite[] sprites = new Sprite[tempSpriteAtlas.spriteCount];
            tempSpriteAtlas.GetSprites(sprites);
            return tempSpriteAtlas;
        }

    }
}
