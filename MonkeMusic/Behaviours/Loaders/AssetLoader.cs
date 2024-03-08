using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MonkeMusic.Behaviours.Loaders
{
    public class AssetLoader
    {
        public AssetBundle bundle;

        public T LoadAsset<T>(string name) where T : Object
        {
            if (bundle == null)
                LoadBundle();

            return bundle.LoadAsset<T>(name);
        }

        private void LoadBundle()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GorillaMusic.Resources.music");
            bundle = AssetBundle.LoadFromStream(stream);
        }
    }
}
