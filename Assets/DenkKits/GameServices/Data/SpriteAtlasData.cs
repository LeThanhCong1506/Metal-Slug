using System;
using UnityEngine.U2D;

namespace DenkKits.GameServices.Data
{

    [Serializable]
    public class SpriteAtlasData
    {
        public AtlasName name;
        public SpriteAtlas atlas;
    }
    /// <summary>
    /// Name of Atlas using to get sprite
    /// </summary>
    public enum AtlasName
    {
        Common = 0,
        Cards,
        Cars,
        PowerUps
    }
}