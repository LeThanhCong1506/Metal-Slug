
namespace DenkKits.GameServices.Audio.Scripts
{
    public enum AudioName
    {
        // Main Music

        NoSound = -1,

        #region BGM 1 - 10

        BGM_Menu = 0,
        BGM_GAMEPLAY = 1,

        #endregion

        #region UI SOUND 11 - 100

        UI_Click = 11,
        UI_LevelUp = 12,
        UI_Yay = 13,
        UI_Yaheee = 14,
        UI_Wind = 15,
      

        #endregion

        #region GAME PLAY > 100
        
        Gameplay_Scored = 106,
        Gameplay_CoinEndGame = 107,
        Gameplay_GotHit = 108,
        Gameplay_LootElemet = 109,
        Gameplay_DoneRegen = 110,
        Gameplay_ChangeElemet = 111,
        Gameplay_EnemyHit = 112,
        Gameplay_EndGame = 113,
      
        #endregion
    }

    public enum AudioType
    {
        SFX = 0,
        BGM = 1,
        AMB = 2,
    }
}