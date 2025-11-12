using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TownOfTrailay.Helpers.Features
{
    internal static class CosmeticLoaderManager
    {
        public static void LoadCosmetics()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<string> resourceNames = assembly.GetManifestResourceNames().Where(name => name.StartsWith("TownOfTrailay.Assets.Hats.strings.") && name.EndsWith(".json")).ToList();
            List<HatData> hatList = new List<HatData>();
            foreach (string jsonFile in resourceNames)
            {
                try
                {
                    hatList.Add(JsonUtility.FromJson<HatData>(new StreamReader(assembly.GetManifestResourceStream(jsonFile)).ReadToEnd()));
                }
                catch { }
            }
            foreach (HatData hatData in hatList)
            {
                try
                {
                    MemoryStream stream = new MemoryStream();
                    assembly.GetManifestResourceStream(hatData.ImagePath.Replace("resources/images/", "TownOfTrailay.Assets.Hats.images.")).CopyTo(stream);
                    AddHat(hatData, ImageUtils.LoadFromRawImage(stream.ToArray(), 300f / hatData.Size, SpriteMeshType.FullRect));
                }
                catch { }
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
