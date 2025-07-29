using UnityEngine;

namespace DenkKits.GameServices.Audio.Scripts
{
    
    [System.Serializable]
    public class AudioData
    {
        public AudioName audioName;

        public AudioType type;

        public AudioClip audioClip;

        [Range(0f, 1f)] public float volume = 1f;

        [Range(0, 256)] public int priority = 128;

        [Range(0, 1)] public float spatialBlend;

        [HideInInspector] public AudioSource source;

        public bool isLooping;

        public bool playOnAwake;

    }
}