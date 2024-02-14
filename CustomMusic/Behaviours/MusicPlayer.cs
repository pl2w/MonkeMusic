using CustomMusic;
using CustomMusic.Behaviours;
using CustomMusic.Behaviours.Buttons;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace CustomSpeakerMusic.Behaviours
{
    public class MusicPlayer : MonoBehaviour, IInitializable
    {
        public Song currentPlayingSong;
        public AudioSource audioSource;
        public AssetLoader assetLoader;
        public AudioLoader audioLoader;
        bool _initialized;

        int currentSongIndex = 0;

        TMP_Text loopText;
        TMP_Text playText;
        TMP_Text volumeText;

        [Inject]
        public void InitializePlayer(AssetLoader assetLoader, AudioLoader audioLoader)
        {
            if (_initialized)
                return;

            _initialized = true;

            this.assetLoader = assetLoader;
            this.audioLoader = audioLoader;

            CreateHoldable();
            audioSource = GorillaLocomotion.Player.Instance.headCollider.gameObject.AddComponent<AudioSource>();

            volumeText.text = $"VOL: {Mathf.CeilToInt(audioSource.volume * 10)}";
            playText.text = audioSource.isPlaying ? "STOP" : "PLAY";
            loopText.text = audioSource.loop ? "UNLOOP" : "LOOP";
        }

        void CreateHoldable()
        {
            GameObject holdable = Instantiate(assetLoader.LoadAsset<GameObject>("HoldableMusic"));
            Transform buttons = holdable.transform.Find("Buttons");

            volumeText = holdable.transform.Find("Volume").gameObject.GetComponent<TMP_Text>();

            buttons.transform.Find("AddVolume").gameObject.AddComponent<PressableButton>().OnPress += delegate { ChangeVolume(true); };
            buttons.transform.Find("DecreaseVolume").gameObject.AddComponent<PressableButton>().OnPress += delegate { ChangeVolume(false); };

            buttons.transform.Find("ChangeSongOrGroupRight").gameObject.AddComponent<PressableButton>().OnPress += delegate { ChangeSong(true); };
            buttons.transform.Find("ChangeSongOrGroupLeft").gameObject.AddComponent<PressableButton>().OnPress += delegate { ChangeSong(false); };

            playText = buttons.transform.Find("PlayStopSong").gameObject.GetComponent<TMP_Text>();
            playText.gameObject.AddComponent<PressableButton>().OnPress += delegate { ToggleSong(audioLoader.songs[currentSongIndex]); };

            loopText = buttons.transform.Find("LoopUnloopSong").gameObject.GetComponent<TMP_Text>();
            loopText.gameObject.AddComponent<PressableButton>().OnPress += delegate 
            { 
                audioSource.loop = !audioSource.loop;
                loopText.text = audioSource.loop ? "UNLOOP" : "LOOP";
            };
        }

        void ChangeSong(bool next)
        {
            if (next && currentSongIndex + 1 < audioLoader.songs.Count)
                currentSongIndex++;
            if (!next && currentSongIndex - 1 >= 0)
                currentSongIndex--;
        }

        void ChangeVolume(bool increase)
        {
            audioSource.volume += increase ? 0.1f : -0.1f;
            volumeText.text = $"VOL: {Mathf.CeilToInt(audioSource.volume * 10)}";
        }

        public void Initialize() { }

        public void ToggleSong(Song song)
        {
            if(currentPlayingSong == song && audioSource.isPlaying)
            {
                audioSource.Stop();
                playText.text = audioSource.isPlaying ? "STOP" : "PLAY";
                return;
            }

            currentPlayingSong = song;
            audioSource.clip = song.clip;
            audioSource.Play();

            playText.text = audioSource.isPlaying ? "STOP" : "PLAY";
        }

        public void OnGUI()
        {
            for (int i = 0; i < audioLoader.songs.Count; i++)
            {
                if (GUI.Button(new Rect(5, 25 * i, 250, 25), audioLoader.songs[i].name))
                    ToggleSong(audioLoader.songs[i]);
            }

            audioSource.volume = GUI.HorizontalSlider(new Rect(5, 25 * audioLoader.songs.Count + 10, 250, 15), audioSource.volume, 0.0f, 1f);
            audioSource.loop = GUI.Toggle(new Rect(5, 25 * audioLoader.songs.Count + 25, 75, 30), audioSource.loop, "Loop");
        }
    }
}
