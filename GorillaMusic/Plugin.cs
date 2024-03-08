using BepInEx;
using Bepinject;
using GorillaMusic;
using GorillaMusic.Behaviours.Loaders;

namespace CustomMusic
{
    [BepInPlugin("pl2w.gorillamusic", "GorillaMusic", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public Plugin()
        {
            MusicConfig.Init(Config);
            Zenjector.Install<MainInstaller>().OnProject().WithConfig(Config);
        }
    }
}
