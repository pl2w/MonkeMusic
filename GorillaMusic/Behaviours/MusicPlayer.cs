using GorillaMusic.Behaviours.Loaders;
using UnityEngine;
using Zenject;

namespace GorillaMusic.Behaviours
{
    public class MusicPlayer : MonoBehaviour, IInitializable
    {
        public Song currentPlayingSong;
        public AudioSource audioSource;
        public AssetLoader assetLoader;
        public AudioLoader audioLoader;
        bool _initialized;

        int currentSongIndex = 0;
        HoldableManager holdableManager;

        [Inject]
        public void InitializePlayer(AssetLoader assetLoader, AudioLoader audioLoader, HoldableManager holdableManager)
        {
            if (_initialized)
                return;

            _initialized = true;

            this.assetLoader = assetLoader;
            this.audioLoader = audioLoader;
            this.holdableManager = holdableManager;

            audioSource = GorillaLocomotion.Player.Instance.headCollider.gameObject.AddComponent<AudioSource>();
        }


        public void ChangeSong(bool next)
        {
            if (next && currentSongIndex + 1 < audioLoader.songs.Count)
                currentSongIndex++;
            if (!next && currentSongIndex - 1 >= 0)
                currentSongIndex--;

            holdableManager.songName.text = audioLoader.songs[currentSongIndex].name;
        }

        // weird work around
        public void GotoSong(int index)
        {
            currentSongIndex = index;
            holdableManager.songName.text = audioLoader.songs[currentSongIndex].name;
        }

        public void ChangeVolume(bool increase)
        {
            audioSource.volume += increase ? 0.1f : -0.1f;
            holdableManager.volumeText.text = $"VOL: {Mathf.CeilToInt(audioSource.volume * 10)}";
        }

        public void Initialize() { }

        public void ToggleSong(Song song)
        {
            if(currentPlayingSong == song && audioSource.isPlaying)
            {
                audioSource.Stop();
                holdableManager.playText.text = audioSource.isPlaying ? "STOP" : "PLAY";
                holdableManager.disk.transform.localEulerAngles = Vector3.zero;
                return;
            }

            currentPlayingSong = song;
            audioSource.clip = song.clip;
            audioSource.Play();

            holdableManager.playText.text = audioSource.isPlaying ? "STOP" : "PLAY";
        }

        public void ToggleCurrentSong()
        {
            ToggleSong(audioLoader.songs[currentSongIndex]);
        }

        //public void OnGUI()
        //{
        //    for (int i = 0; i < audioLoader.songs.Count; i++)
        //    {
        //        if (GUI.Button(new Rect(5, 25 * i, 250, 25), audioLoader.songs[i].name))
        //            ToggleSong(audioLoader.songs[i]);
        //    }
        //
        //    audioSource.volume = GUI.HorizontalSlider(new Rect(5, 25 * audioLoader.songs.Count + 10, 250, 15), audioSource.volume, 0.0f, 1f);
        //    audioSource.loop = GUI.Toggle(new Rect(5, 25 * audioLoader.songs.Count + 25, 75, 30), audioSource.loop, "Loop");
        //}
    }
}
