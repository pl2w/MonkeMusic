using MonkeMusic.Behaviours.Buttons;
using MonkeMusic.Behaviours.Loaders;
using TMPro;
using UnityEngine;
using Zenject;

namespace MonkeMusic.Behaviours
{
    public class HoldableManager : MonoBehaviour, IInitializable
    {
        public GameObject holdable, disk;
        public TMP_Text volumeText, 
            songName, 
            playText, 
            loopText;

        public MusicPlayer musicPlayer;
        bool init;

        public void Initialize() { }

        [Inject]
        public void CreateHoldable(AssetLoader assetLoader, MusicPlayer musicPlayer)
        {
            if (init)
                return;

            init = true;
            this.musicPlayer = musicPlayer;

            holdable = Instantiate(assetLoader.LoadAsset<GameObject>("HoldableMusic"));
            holdable.transform.parent = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L/").transform;
            holdable.transform.localPosition = new Vector3(-0.09f, 0.08f, 0.02f);
            holdable.transform.localEulerAngles = new Vector3(0f, 270f, 317.5f);
            holdable.transform.localScale = new Vector3(0.035f, 0.035f, 0.035f);

            disk = holdable.transform.Find("CurrentSelectedVinyl").gameObject;

            Transform buttons = holdable.transform.Find("Buttons");

            volumeText = holdable.transform.Find("Volume").gameObject.GetComponent<TMP_Text>();
            songName = holdable.transform.Find("SongNameAuthor").gameObject.GetComponent<TMP_Text>();

            buttons.transform.Find("AddVolume").gameObject.AddComponent<PressableButton>().OnPress += delegate { musicPlayer.ChangeVolume(true); };
            buttons.transform.Find("DecreaseVolume").gameObject.AddComponent<PressableButton>().OnPress += delegate { musicPlayer.ChangeVolume(false); };

            buttons.transform.Find("ChangeSongOrGroupRight").gameObject.AddComponent<PressableButton>().OnPress += delegate { musicPlayer.ChangeSong(true); };
            buttons.transform.Find("ChangeSongOrGroupLeft").gameObject.AddComponent<PressableButton>().OnPress += delegate { musicPlayer.ChangeSong(false); };

            playText = buttons.transform.Find("PlayStopSong").gameObject.GetComponent<TMP_Text>();
            playText.gameObject.AddComponent<PressableButton>().OnPress += delegate { musicPlayer.ToggleCurrentSong(); };

            loopText = buttons.transform.Find("LoopUnloopSong").gameObject.GetComponent<TMP_Text>();
            loopText.gameObject.AddComponent<PressableButton>().OnPress += delegate
            {
                musicPlayer.audioSource.loop = !musicPlayer.audioSource.loop;
                loopText.text = musicPlayer.audioSource.loop ? "UNLOOP" : "LOOP";
            };

            volumeText.text = $"VOL: {Mathf.CeilToInt(musicPlayer.audioSource.volume * 10)}";
            playText.text = musicPlayer.audioSource.isPlaying ? "STOP" : "PLAY";
            loopText.text = musicPlayer.audioSource.loop ? "UNLOOP" : "LOOP";
        }

        void Update()
        {
            holdable?.SetActive(IsInputHeldDown());

            if (musicPlayer.audioSource.isPlaying)
            {
                disk.transform.localEulerAngles += new Vector3(0, 0, 1);
            }
        }

        bool IsInputHeldDown()
        {
            switch (MusicConfig.bindedInput)
            {
                case EInput.LeftGrip:
                    return ControllerInputPoller.instance.leftControllerGripFloat >= MusicConfig.heldSensitivity;
                case EInput.RightGrip:
                    return ControllerInputPoller.instance.rightControllerGripFloat >= MusicConfig.heldSensitivity;
                case EInput.LeftTrigger:
                    return ControllerInputPoller.instance.leftControllerIndexFloat >= MusicConfig.heldSensitivity;
                case EInput.RightTrigger:
                    return ControllerInputPoller.instance.rightControllerIndexFloat >= MusicConfig.heldSensitivity;

                case EInput.LeftRightGrip:
                    return ControllerInputPoller.instance.leftControllerGripFloat >= MusicConfig.heldSensitivity ||
                           ControllerInputPoller.instance.rightControllerGripFloat >= MusicConfig.heldSensitivity;
                case EInput.LeftRightTrigger:
                    return ControllerInputPoller.instance.leftControllerIndexFloat >= MusicConfig.heldSensitivity ||
                           ControllerInputPoller.instance.rightControllerIndexFloat >= MusicConfig.heldSensitivity;
            }

            return false;
        }
    }
}