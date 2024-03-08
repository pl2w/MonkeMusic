using UnityEngine;

namespace GorillaMusic.Behaviours
{
    public class Song
    {
        public AudioClip clip;
        public string name;
        public string group;

        public Song(AudioClip clip, string name, string group)
        {
            this.clip = clip;
            this.name = name;
            this.group = group;
        }
    }
}
