using System;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "GameEvent.asset",
        menuName = SOArchitecture_Utility.GAME_EVENT + "Game Event",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_EVENTS - 1)]
    public sealed class GameEvent : GameEventBase
    {
        internal void AddListener(GameEvent showSlotMachine)
        {
            throw new NotImplementedException();
        }
    }
}