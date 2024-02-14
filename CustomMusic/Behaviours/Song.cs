using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomMusic.Behaviours
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
