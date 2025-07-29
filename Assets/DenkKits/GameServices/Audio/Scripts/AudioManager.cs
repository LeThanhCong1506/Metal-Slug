/*
TODO: Pooling audio source
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Imba.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DenkKits.GameServices.Audio.Scripts
{
    public class AudioManager : ManualSingletonMono<AudioManager>
    {
        public event Action OnChangeSfx;
        public AudioDatabase database;
        private Queue<AudioSource> _audioSourcePool = new Queue<AudioSource>();
        private const int InitialPoolSize = 10;

        #region VARIABLES

        private const string MusicVolumeKey = "musicKey";
        private const string AudioVolumeKey = "audioKey";
        private const float TimeToCheckIdleAudioSource = 5f;
        private float _timeToReset;
        private float _timeToCheckIdleAudioSource;
        private bool _timerIsSet;
        private AudioName _tmpName;
        private float _tmpVol;
        private bool _isLowered;
        private bool _fadeOut;
        private bool _fadeIn;
        private string _fadeInUsedString;
        private string _fadeOutUsedString;
        private bool _isMuteMusic;
        private bool _isMuteSfx;

        #endregion

        #region AUDIO CONFIG

        public float musicVolume = 0.5f;
        public float audioVolume = 0.5f;

        public void SetMusicVolume(float newValue)
        {
            musicVolume = newValue;
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
            foreach (var s in database.audioDataList)
            {
                if (s.audioClip == null)
                    continue;
                if (s.type == AudioType.BGM)
                {
                    if (s.source == null)
                    {
                        continue;
                    }

                    s.source.volume = newValue;
                }
            }
        }

        public void SetAudioVolume(float newValue)
        {
            audioVolume = newValue;
            PlayerPrefs.SetFloat(AudioVolumeKey, audioVolume);
        }

        public void SaveAudioSetting()
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, musicVolume);
            PlayerPrefs.SetFloat(AudioVolumeKey, audioVolume);
        }

        #endregion

        #region UNITY METHOD

        // Use this for initialization
        public override void Awake()
        {
            base.Awake();

            if (PlayerPrefs.HasKey("MuteMusic"))
            {
                _isMuteMusic = PlayerPrefs.GetInt("MuteMusic") == 1;
            }

            if (PlayerPrefs.HasKey("MuteSFX"))
            {
                _isMuteSfx = PlayerPrefs.GetInt("MuteSFX") == 1;
            }

            musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
            audioVolume = PlayerPrefs.GetFloat(AudioVolumeKey, 0.5f);

            foreach (var s in database.audioDataList)
            {
                if (s.playOnAwake)
                {
                    s.source = CreateAudioSource(s);
                    if (IsMuteAudio(s.type))
                    {
                        s.source.mute = true;
                    }

                    if (s.type == AudioType.BGM)
                    {
                        s.volume = musicVolume * s.volume;
                    }

                    s.source.Play();
                }
            }

        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Instance.FadeIn(GetMusicBGName(scene.name), 1f);
        }

        void OnSceneUnloaded(Scene scene)
        {
            Instance.StopMusic(GetMusicBGName(scene.name));
        }

        private AudioName GetMusicBGName(string currentScene)
        {
            switch (currentScene)
            {
                case "MainMenu":
                {
                    return AudioName.BGM_Menu;
                }
                case "Game":
                {
                    return AudioName.BGM_GAMEPLAY;
                }
            }

            return AudioName.BGM_Menu;
        }

        #endregion

        #region CLASS METHODS

        private AudioSource CreateAudioSource(AudioData a)
        {
            AudioSource s = Instance.gameObject.AddComponent<AudioSource>();
            s.clip = a.audioClip;
            s.volume = a.volume;
            s.playOnAwake = a.playOnAwake;
            s.priority = a.priority;
            s.loop = a.isLooping;
            return s;
        }

        public bool IsMuteAudio(AudioType type)
        {
            if (type == AudioType.BGM && _isMuteMusic) return true;
            if (type == AudioType.SFX && _isMuteSfx) return true;

            return false;
        }

        private AudioData GetAudioData(AudioName audioName)
        {
            AudioData s = database.audioDataList.Find(a => a.audioName == audioName);

            return s;
        }

        public void PlaySfx(AudioName audioName, bool isLoop = false)
        {
            if (_isMuteSfx || audioName == AudioName.NoSound) return;

            AudioData s = GetAudioData(audioName);
            if (s == null)
            {
                //Debug.LogError("Sound name" + audioName + "not found!");
            }
            else
            {
                if (s.source == null)
                {
                    s.source = CreateAudioSource(s);
                }

                s.source.volume = s.volume * audioVolume;
                s.isLooping = isLoop;
                s.source.PlayOneShot(s.audioClip, s.volume);
            }
        }

        public void StopSfx(AudioName audioName)
        {
            if (_isMuteSfx || audioName == AudioName.NoSound) return;

            AudioData s = GetAudioData(audioName);
            if (s == null)
            {
                //Debug.LogError("Sound name" + audioName + "not found!");
            }
            else
            {
                if (s.source == null)
                {
                    s.source = CreateAudioSource(s);
                }

                s.source.Stop();
            }
        }

        public void PlaySoundFromSource(AudioName audioName, AudioSource audioSource, bool isChangeSound = false)
        {
            if (_isMuteSfx || audioName == AudioName.NoSound) return;

            if (audioSource == null) return;

            if (audioSource.clip == null || isChangeSound)
            {
                AudioData s = GetAudioData(audioName);
                if (s == null)
                {
                    //Debug.LogError("Sound name" + audioName + "not found!");
                    return;
                }

                if (audioSource.clip == null)
                {
                    audioSource.clip = s.audioClip;
                    audioSource.volume = s.volume;
                    audioSource.priority = s.priority;
                    audioSource.playOnAwake = s.playOnAwake;
                    audioSource.spatialBlend = s.spatialBlend;
                    audioSource.rolloffMode = AudioRolloffMode.Linear;
                    audioSource.minDistance = 1;
                    audioSource.maxDistance = 50;
                }
            }

            audioSource.PlayOneShot(audioSource.clip, audioSource.volume);
        }

        public void PlayMusic(AudioName audioName)
        {
            if (_isMuteMusic || audioName == AudioName.NoSound) return;

            AudioData s = GetAudioData(audioName);
            if (s == null)
            {
                Debug.LogError("Sound name" + audioName + "not found!");
                Debug.LogError("Sound name" + audioName + "not found!");
                return;
            }

            s.source.volume = s.volume * musicVolume;
            if (s.source == null)
            {
                s.source = CreateAudioSource(s);
            }

            s.source.volume = musicVolume;
            //s.Source.Se
            //Debug.Log($"{audioName} {s.Source} {s.Source.volume} {s.Volume * _musicVolume}");

            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }

        public void MuteMusic()
        {
            if (PlayerPrefs.HasKey("MuteMusic"))
            {
                _isMuteMusic = PlayerPrefs.GetInt("MuteMusic") == 1;
                foreach (var s in database.audioDataList)
                {
                    if (s.audioClip == null)
                        continue;
                    if (s.type == AudioType.BGM)
                    {
                        if (s.source == null)
                        {
                            continue;
                        }

                        //s.Source.volume = (_isMuteMusic) ? 0f : 1f;
                        s.source.mute = _isMuteMusic;
                    }
                }
            }
        }

        public void MuteSfx()
        {
            if (PlayerPrefs.HasKey("MuteSFX"))
            {
                OnChangeSfx?.Invoke();
                _isMuteSfx = PlayerPrefs.GetInt("MuteSFX") == 1;
                foreach (var s in database.audioDataList)
                {
                    if (s.audioClip == null)
                        continue;
                    if (s.type == AudioType.SFX)
                    {
                        if (s.source == null)
                        {
                            continue;
                        }

                        s.source.mute = _isMuteSfx;
                    }
                }
            }
        }

        public void StopAllMusic()
        {
            foreach (var s in database.audioDataList.Where(o => o.type == AudioType.BGM))
            {
                if (s.source != null)
                {
                    s.source.Stop();
                }
            }
        }

        public void StopMusic(AudioName audioName)
        {
            if (audioName == AudioName.NoSound)
            {
                return;
            }

            AudioData s = GetAudioData(audioName);
            if (s == null)
            {
                //Debug.LogError("Sound name" + audioName + "not found!");
            }
            else
            {
                if (s.type == AudioType.BGM && s.source != null)
                {
                    s.source.Stop();
                }
            }
        }

        public void PauseMusic(AudioName audioName)
        {
            AudioData s = GetAudioData(audioName);
            if (s == null)
            {
                //Debug.LogError("Sound name" + audioName + "not found!");
            }
            else
            {
                if (s.type == AudioType.BGM && s.source != null)
                {
                    s.source.Pause();
                }
            }
        }

        public void UnPauseMusic(AudioName audioName)
        {
            AudioData s = GetAudioData(audioName);
            if (s == null)
            {
                //Debug.LogError("Sound name" + audioName + "not found!");
            }
            else
            {
                if (s.type == AudioType.BGM && s.source != null)
                {
                    s.source.UnPause();
                }
            }
        }

        public void LowerVolume(AudioName audioName, float duration)
        {
            if (Instance._isLowered == false)
            {
                AudioData s = GetAudioData(audioName);
                if (s == null)
                {
                    //Debug.LogError("Sound name" + audioName + "not found!");
                    return;
                }
                else
                {
                    Instance._tmpName = audioName;
                    Instance._tmpVol = s.volume;
                    Instance._timeToReset = Time.time + duration;
                    Instance._timerIsSet = true;
                    s.source.volume = s.source.volume / 3;
                }

                Instance._isLowered = true;
            }
        }

        public void FadeOut(AudioName audioName, float duration)
        {
            Instance.StartCoroutine(Instance.AudioFadeOut(audioName, duration));
        }

        public void FadeIn(AudioName audioName, float duration)
        {
            Instance.StartCoroutine(Instance.AudioFadeIn(audioName, duration));
        }

        //not for use
        private IEnumerator AudioFadeOut(AudioName audioName, float duration)
        {
            AudioData s = GetAudioData(audioName);
            if (s == null)
            {
                //Debug.LogError("Sound name" + name + "not found!");
                yield return null;
            }
            else
            {
                if (_fadeOut == false)
                {
                    _fadeOut = true;

                    if (s.source == null)
                    {
                        yield return null;
                    }

                    float startVol = s.source.volume;
                    _fadeOutUsedString = name;
                    while (s.source != null && s.source.volume > 0)
                    {
                        s.source.volume -= startVol * Time.deltaTime / duration;
                        yield return null;
                    }

                    s.source.Stop();
                    yield return new WaitForSeconds(duration);
                    _fadeOut = false;
                }
                else
                {
                    ////Debug.Log("Could not handle two fade outs at once : " + name + " , " + _fadeOutUsedString +
                    //        "! Stopped the music " + name);
                    //StopMusic(audioName);//dont stop, cause stop same music
                }
            }
        }

        private IEnumerator AudioFadeIn(AudioName audioName, float duration)
        {
            if (audioName == AudioName.NoSound)
                yield break;

            AudioData s = GetAudioData(audioName);
            if (s == null)
                yield break;

            if (s.source == null)
                s.source = CreateAudioSource(s);

            if (s.source.isPlaying)
                yield break;

            if (_fadeIn)
            {
                // Có fade-in đang chạy, dùng fallback:
                StopMusic(audioName);
                PlayMusic(audioName);
                yield break;
            }

            _fadeIn = true;
            _fadeInUsedString = name;

            float targetVolume = s.volume * musicVolume;

            if (_isMuteMusic || targetVolume <= 0f)
            {
                s.source.volume = 0f;
                s.source.mute = true;
                s.source.Play();
                _fadeIn = false;
                yield break;
            }

            s.source.volume = 0f;
            s.source.mute = false;
            s.source.Play();

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                s.source.volume = Mathf.Lerp(0f, targetVolume, t);
                yield return null;
            }

            s.source.volume = targetVolume;
            _fadeIn = false;
        }


        void ResetVol()
        {
            AudioData s = GetAudioData(_tmpName);
            s.source.volume = _tmpVol;
            _isLowered = false;
        }

        private void Update()
        {
            if (Time.time >= _timeToReset && _timerIsSet)
            {
                ResetVol();
                _timerIsSet = false;
            }

            _timeToCheckIdleAudioSource += Time.deltaTime;
            if (_timeToCheckIdleAudioSource > TimeToCheckIdleAudioSource)
            {
                var audios = GetComponents<AudioSource>();
                foreach (var a in audios)
                {
                    if (!a.isPlaying)
                        Destroy(a);
                }

                _timeToCheckIdleAudioSource = 0;
            }
        }

        public void SetSound(bool isOn)
        {
            _isMuteSfx = isOn;
            PlayerPrefs.SetInt("MuteSFX", isOn ? 1 : 0);
        }

        public void SetMusic(bool isOn)
        {
            _isMuteMusic = isOn;
            PlayerPrefs.SetInt("MuteMusic", isOn ? 1 : 0);
        }

        #endregion

        #region ADDED GPT
        public AudioSource PlayLoopingSfx(AudioName audioName)
        {
            if (_isMuteSfx || audioName == AudioName.NoSound) return null;

            AudioData s = GetAudioData(audioName);
            if (s == null || s.audioClip == null) return null;

            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = s.audioClip;
            source.volume = s.volume * audioVolume;
            source.loop = true;
            source.playOnAwake = false;
            source.Play();

            return source;
        }

        public void StopLoopingSfx(AudioSource source)
        {
            if (source == null) return;

            source.Stop();
            Destroy(source); // Huỷ AudioSource khi không dùng nữa
        }
        #endregion
    }
}