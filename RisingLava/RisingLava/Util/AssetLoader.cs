using System.IO;
using System.Reflection;
using UnityEngine;

namespace RisingLava.Util
{
    internal static class AssetLoader
    {
        private static AssetBundle _assetBundle;
        private static AssetBundle assetBundle
        {
            get
            {
                if (_assetBundle == null)
                {
                    using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RisingLava.Resources.assets"))
                    {
                        byte[] bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                        _assetBundle = AssetBundle.LoadFromMemory(bytes);
                    }
                }
                return _assetBundle;
            }
        }

        internal static UnityEngine.Object GetAsset(string Name)
        {
            return assetBundle.LoadAsset(Name);
        }
    }
}
