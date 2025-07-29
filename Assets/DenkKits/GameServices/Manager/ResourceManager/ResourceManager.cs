
using System.Collections.Generic;
using DenkKits.GameServices.Data;
using DenkKits.GameServices.Data.Collections;
using Imba.Utils;
using UnityEngine;

namespace DenkKits.GameServices.Manager.ResourceManager
{
    /// <summary>
    /// Manage resource to load in game
    /// </summary>
    public class ResourceManager : ManualSingletonMono<ResourceManager>
    {
        public List<SpriteAtlasData> listAtlasData;//TODO: convert to use Scriptable Object
        public RewardBonusListCollection dailyRewardCollection;

        public Sprite GetSpriteByName(AtlasName atlasName, string spriteName)
        {
            SpriteAtlasData atlasData = listAtlasData.Find(r => r.name == atlasName);
            if (atlasData == null)
            {
                Debug.LogError("Cannot find atlas " + atlasName);
                return null;
            }

            Sprite s = atlasData.atlas.GetSprite(spriteName);
            if (s == null)
            {
                Debug.LogError("Cannot find sprite " + spriteName + " in atlas " + atlasName);
                return null;
            }

            return s;

        }
    }
}