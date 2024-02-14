using BepInEx;
using Bepinject;

namespace CustomMusic
{
    [BepInPlugin("pl2w.musicplayer", "MusicPlayer", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public Plugin() => Zenjector.Install<MainInstaller>().OnProject();
    }
}
