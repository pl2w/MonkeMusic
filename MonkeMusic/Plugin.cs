using BepInEx;
using Bepinject;
using MonkeMusic.Behaviours.Loaders;

namespace MonkeMusic
{
    [BepInPlugin("pl2w.monkemusic", "MonkeMusic", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public Plugin()
        {
            MusicConfig.Init(Config);
            Zenjector.Install<MainInstaller>().OnProject().WithConfig(Config);
        }
    }
}
