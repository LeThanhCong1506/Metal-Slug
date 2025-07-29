using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DenkKits.GameServices.Data
{
    [Serializable]
    public class AuxiliaryData
    {
        public AuxiliaryType type;
        [PreviewField] public Sprite Sprite;
    }

    public enum AuxiliaryType
    {
        Booster1,
        Booster2,
        Booster3,
        Booster4,
        Money,
        Money2,
        Item1,
        Item2,
        Item3,
        Item4,
    }
}