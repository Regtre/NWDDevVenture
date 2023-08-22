using NWDFoundation.Models;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Video;

namespace NWDUnityShared.Tools
{
    public class NWDAssetUtility
    {
        static public readonly Dictionary<Type, Type> FacadeAssetToUnityAsset = new Dictionary<Type, Type>()
        {
            { typeof(INWDTextAsset), typeof(TextAsset) },
            { typeof(INWDSpriteAsset), typeof(Sprite) },
            { typeof(INWDTextureAsset), typeof(Texture) },
            { typeof(INWDAudioAsset), typeof(AudioClip) },
            { typeof(INWDVideoAsset), typeof(VideoClip) },
            { typeof(INWDPrefabAsset), typeof(GameObject) },
#if UNITY_EDITOR
            { typeof(INWDSceneAsset), typeof(SceneAsset) },
#endif
        };

        static public string Serialize (NWDAssetUtility sAssetUtility)
        {
            if (sAssetUtility == null)
            {
                return "";
            }

            return sAssetUtility.GUID + ":" + sAssetUtility.Index + ":" + sAssetUtility.Key + ":" + sAssetUtility.Mode;
        }

        static public NWDAssetUtility Deserialize (string sAssetUtility)
        {
            if (string.IsNullOrEmpty (sAssetUtility))
            {
                return null;
            }

            string[] tData = sAssetUtility.Split(':');
            return new NWDAssetUtility
            {
                GUID = tData[0],
                Index = long.Parse(tData[1]),
                Key = tData[2],
                Mode = int.Parse (tData[3]),
            };
        }

        public string GUID;
        public long Index;
        public string Key;
        public int Mode;

        public NWDAssetUtility() { }
    }
}
