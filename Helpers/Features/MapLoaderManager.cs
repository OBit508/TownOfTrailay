using LevelImposter;
using LevelImposter.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TownOfTrailay.Helpers.Features
{
    internal static class MapLoaderManager
    {
        public static string[] Maps = new string[] { "Polus" };
        public static void LoadMaps()
        {
            if (ModsManager.Instance == null)
            {
                return;
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string m in Maps)
            {
                try
                {
                    string folder = "TownOfTrailay.Assets.Maps." + m;
                    Sprite LImapLogoSpr = Utilities.Utils.LoadTextureFromResources(folder + ".LIMapLogo.png").ResizeTexture(210, 47).ToSprite();
                    LIMap map = GetMap(assembly.GetManifestResourceStream(folder + ".Map.lim2"), "idk");
                    LIMapDescription mapDescription = JsonUtility.FromJson<LIMapDescription>(new StreamReader(assembly.GetManifestResourceStream(folder + ".Description.json")).ReadToEnd());
                    LIMapLoader.cachedMaps.Add(mapDescription.MapName, map);
                    LIShipStatus mapInstance = LIMapLoader.CreateMap(mapDescription.MapName);
                    MapManager.allMaps.Add(map.name, new MapData(mapInstance, LImapLogoSpr));
                    MapManager.allMaps[map.name].Credits = mapDescription.Credits + map.authorName;
                }
                catch { }
            }
        }
        public static LIMap? GetMap(Stream map, string mapID)
        {
            LIMap lIMap = LIDeserializer.DeserializeMap(map);
            if (lIMap != null)
            {
                lIMap.id = mapID;
            }
            return lIMap;
        }
        [Serializable]
        public class LIMapDescription
        {
            public string MapName;
            public string Credits;
        }
    }
}
