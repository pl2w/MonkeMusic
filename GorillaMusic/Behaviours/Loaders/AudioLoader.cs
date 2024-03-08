using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;
using Debug = UnityEngine.Debug;

namespace GorillaMusic.Behaviours.Loaders
{
    //https://forum.unity.com/threads/load-mp3-from-user-file-at-runtime-windows.589138/
    public class AudioLoader : MonoBehaviour, IInitializable
    {
        public string songsDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Music");
        public List<Song> songs = new List<Song>();

        // (string) file extension,
        // (AudioType) audio type, (bool) is streamable
        public Dictionary<string, (AudioType, bool)> audioTypes = new Dictionary<string, (AudioType, bool)>()
        {
            // regular file types
            { ".mp3", (AudioType.MPEG, true) },
            { ".wav", (AudioType.WAV, true )},
            { ".ogg", (AudioType.OGGVORBIS, true) },
            // tracker file types
            { ".it", (AudioType.IT, false) },
            { ".s3m", (AudioType.S3M, false) },
            { ".xm", (AudioType.XM, false) },
            { ".mod", (AudioType.MOD, false) }
        };

        public bool songsLoaded;
        int _songLoadIndex = 0;

        [Inject]
        public MusicPlayer _player;

        bool init;

        public void Initialize()
        {
            if (init)
                return;

            init = true;
            GetSongsFromFolder();
        }

        public void GetSongsFromFolder()
        {
            if(!Directory.Exists(songsDirectory))
            {
                Directory.CreateDirectory(songsDirectory);
                return;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(songsDirectory);
            FileInfo[] songFiles = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (FileInfo item in songFiles)
                StartCoroutine(GetAudioClip(item));

            songsLoaded = true;
        }

        public IEnumerator GetAudioClip(FileInfo file)
        {
            string extension = file.Extension.ToLower();
            if (!audioTypes.Keys.Contains(extension))
                yield break;

            (AudioType, bool) type = audioTypes[extension];
            StartCoroutine(LoadSong(file, type));
        }

        //https://forum.unity.com/threads/async-streaming-audio-with-unitywebrequest.487062/
        public IEnumerator LoadSong(FileInfo file, (AudioType, bool) type)
        {
            var webRequest = UnityWebRequestMultimedia.GetAudioClip(file.FullName, type.Item1);

            // allow streaming?
            if (type.Item2)
            {
                ((DownloadHandlerAudioClip)webRequest.downloadHandler).streamAudio = true;
                webRequest.SendWebRequest();

                while (webRequest.result != UnityWebRequest.Result.ConnectionError && webRequest.downloadedBytes < 1024)
                    yield return null;
            }
            else
            {
                webRequest.SendWebRequest();
                while (!webRequest.downloadHandler.isDone)
                    yield return new WaitForEndOfFrame();
            }

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error);
                yield break;
            }

            AudioClip clip = ((DownloadHandlerAudioClip)webRequest.downloadHandler).audioClip;
            clip.name = Path.GetFileNameWithoutExtension(file.Name);
            DirectoryInfo group = Directory.GetParent(file.FullName);
            string groupName = group.Name;

            if (group.FullName == songsDirectory)
                groupName = "None";

            songs.Add(new Song(clip, clip.name, groupName));
            webRequest.Dispose();

            if (_songLoadIndex == 0)
                _player.GotoSong(0);

            _songLoadIndex++;
        }
    }
}
