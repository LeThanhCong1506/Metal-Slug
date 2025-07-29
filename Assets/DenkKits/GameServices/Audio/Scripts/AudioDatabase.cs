using System.Collections.Generic;
using UnityEngine;

namespace DenkKits.GameServices.Audio.Scripts
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "AudioSO/Audio Database")]
    public class AudioDatabase : ScriptableObject
    {
        public List<AudioData> audioDataList = new List<AudioData>();
    }
}