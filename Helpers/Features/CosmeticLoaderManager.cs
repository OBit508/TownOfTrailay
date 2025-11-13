using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using static TownOfTrailay.Helpers.Features.CosmeticLoaderManager;

namespace TownOfTrailay.Helpers.Features
{
    internal static class CosmeticLoaderManager
    {
        public static System.Collections.IEnumerator LoadCosmetics(TextMeshPro text, string baseStr)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<string> resourceNames = assembly.GetManifestResourceNames().Where(name => name.StartsWith("TownOfTrailay.Assets.Hats.strings.") && name.EndsWith(".json")).ToList();
            int originalCount = resourceNames.Count;
            while (resourceNames.Count > 0)
            {
                try
                {
                    HatData hatData = JsonUtility.FromJson<HatData>(new StreamReader(assembly.GetManifestResourceStream(resourceNames[0])).ReadToEnd());
                    MemoryStream stream = new MemoryStream();
                    assembly.GetManifestResourceStream(hatData.ImagePath.Replace("resources/images/", "TownOfTrailay.Assets.Hats.images.")).CopyTo(stream);
                    AddHat(hatData, ImageUtils.LoadFromRawImage(stream.ToArray(), 300f / hatData.Size, SpriteMeshType.FullRect));
                }
                catch { }
                text.text = baseStr + ((originalCount - resourceNames.Count) * 100 / originalCount).ToString() + "%";
                resourceNames.RemoveAt(0);
                yield return null;
            }
        }
        [Serializable]
        public class HatData
        {
            public bool InFront;
            public float Size;
            public bool Bounce;
            public bool PlayerColored;
            public string ProductId;
            public string ImagePath;
        }
        public static void AddHat(HatData data, Sprite image)
        {
            HatBehaviour hat = ScriptableObject.CreateInstance<HatBehaviour>();
            hat.InFront = data.InFront;
            hat.MainImage = image;
            hat.FloorImage = image;
            hat.Bounce = data.Bounce;
            hat.PlayerColored = data.PlayerColored;
            hat.ProductId = data.ProductId;
            hat.StoreName = "";
            HatManager.Instance.AllHats.Add(hat);
        }
    }
}
