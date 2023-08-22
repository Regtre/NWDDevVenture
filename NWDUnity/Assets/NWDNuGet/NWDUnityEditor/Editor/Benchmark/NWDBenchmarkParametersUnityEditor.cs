using NWDFoundation.Benchmark;
using NWDUnityEditor.Constants;
using NWDUnityEditor.Tools;
using System;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Benchmark
{
    [Serializable]
    public class NWDBenchmarkParametersUnityEditor : NWDBenchmarkParameters
    {

        /// <summary>
        /// benchmark color pro skin for fast result
        /// </summary>
        const string kColorGreen_Pro = "2EDD66FF";
        /// <summary>
        /// benchmark color pro skin for normal result
        /// </summary>
        const string kColorOrange_Pro = "FF9842FF";
        /// <summary>
        /// benchmark color pro skin for slow result
        /// </summary>
        const string kColorRed_Pro = "FF7070FF";
        /// <summary>
        /// benchmark color pro skin for quick analyze
        /// </summary>
        const string kColorBlue_Pro = "7092FFFF";
        /// <summary>
        /// benchmark color standard skin for fast result
        /// </summary>
        const string kColorGreen = "007626FF";
        /// <summary>
        /// benchmark color standard skin for normal result
        /// </summary>
        const string kColorOrange = "B45200FF";
        /// <summary>
        /// benchmark color standard skin for slow result
        /// </summary>
        const string kColorRed = "890000FF";
        /// <summary>
        /// benchmark color standard skin for quick analyze
        /// </summary>
        const string kColorBlue = "002089FF";

        public NWDBenchmarkParametersUnityEditor()
        {
            PrefReload();
        }

        public void ResetColor()
        {
            if (EditorGUIUtility.isProSkin)
            {
                Green = kColorGreen_Pro;
                Orange = kColorOrange_Pro;
                Red = kColorRed_Pro;
                Blue = kColorBlue_Pro;
            }
            else
            {
                Green = kColorGreen;
                Orange = kColorOrange;
                Red = kColorRed;
                Blue = kColorBlue;
            }
        }

        public override void Save()
        {
            NWDUserSettings.Save(this);
        }

        public override void PrefReload()
        {
            NWDBenchmarkParametersUnityEditor tLoaded = NWDUserSettings.Load<NWDBenchmarkParametersUnityEditor>();

            if (tLoaded != null)
            {
                IsEnable = tLoaded.IsEnable;
                BenchmarkLimit = tLoaded.BenchmarkLimit;
                BenchmarkShowStart = tLoaded.BenchmarkShowStart;
                FrameRate = tLoaded.FrameRate;
                Green = tLoaded.Green;
                Orange = tLoaded.Orange;
                Red = tLoaded.Red;
                Blue = tLoaded.Blue;
            }
            else
            {
                IsEnable = false;
                BenchmarkLimit = 0.0F;
                BenchmarkShowStart = false;
                FrameRate = 60.0F;

                if (EditorGUIUtility.isProSkin)
                {
                    Green = kColorGreen_Pro;
                    Orange = kColorOrange_Pro;
                    Red = kColorRed_Pro;
                    Blue = kColorBlue_Pro;
                }
                else
                {
                    Green = kColorGreen;
                    Orange = kColorOrange;
                    Red = kColorRed;
                    Blue = kColorBlue;
                }
            }

            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    {
                        kWarningDefault = 0.0033f;
                        kMaxDefault = 0.010f;
                    }
                    break;
                case RuntimePlatform.WindowsEditor:
                    {
                        kWarningDefault = 0.0033f;
                        kMaxDefault = 0.010f;
                    }
                    break;
                case RuntimePlatform.LinuxEditor:
                    {
                        kWarningDefault = 0.0033f;
                        kMaxDefault = 0.010f;
                    }
                    break;
            }
        }
    }
}