using NWDFoundation.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace NWDUnityShared.Tools
{
    public partial class NWDConverter
    {
        public static string DateTimeYYYYMMdd(DateTime sDatetime)
        {
            return sDatetime.ToString("yyyy-MM-dd", NWDConstants.FormatCountry);
        }
        
        public static string DateTimeYYYYMMddHHmmss(DateTime sDatetime)
        {
            return sDatetime.ToString("yyyy-MM-dd HH:mm:ss", NWDConstants.FormatCountry);
        }
        
        public static string DateTimeYYYY(DateTime sDatetime)
        {
            return sDatetime.ToString("yyyy", NWDConstants.FormatCountry);
        }
        
        public static string BoolZero()
        {
            return BoolToString(false);
        }
        
        public static string BoolToString(bool sBool)
        {
            return sBool.ToString(NWDConstants.FormatCountry);
        }
        
        public static bool BoolFromString(string sString)
        {
            bool rReturn = false;
            bool.TryParse(sString, out rReturn);
            return rReturn;
        }
        
        public static string BoolToIntZero()
        {
            return BoolToIntString(false);
        }
        
        public static string BoolToIntString(bool sBool)
        {
            if (sBool == true)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        
        public static bool BoolFromIntString(string sString)
        {
            if (sString == "1")
            {
                return true;
            }
            else
            {
                return false;
            };
        }
        
        public static string IntZero()
        {
            return IntToString(0);
        }
        
        public static string IntToString(int sInt)
        {
            return sInt.ToString(NWDConstants.FormatCountry);
        }
        
        public static int IntFromString(string sString)
        {
            int rReturn = 0;
            int.TryParse(sString, NumberStyles.Integer, NWDConstants.FormatCountry, out rReturn);
            return rReturn;
        }
        
        public static string LongZero()
        {
            return LongToString(0);
        }
        
        public static string LongToString(long sLong)
        {
            return sLong.ToString(NWDConstants.FormatCountry);
        }
        
        public static long LongFromString(string sString)
        {
            long rReturn = 0;
            long.TryParse(sString, NumberStyles.Integer, NWDConstants.FormatCountry, out rReturn);
            return rReturn;
        }

        public static string ULongZero()
        {
            return ULongToString(0);
        }

        public static string ULongToString(ulong sLong)
        {
            string rReturn = sLong.ToString();
            return rReturn;
        }

        public static ulong ULongFromString(string sString)
        {
            ulong rReturn = 0;
            ulong.TryParse(sString, out rReturn);
            return rReturn;
        }

        public static string FloatZero()
        {
            return FloatToString(0.0F);
        }
        
        public static string FloatToString(float sFloat)
        {
            return sFloat.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
        }
        
        public static float FloatFromString(string sString)
        {
            float rReturn = 0.0F;
            float.TryParse(sString, NumberStyles.Float, NWDConstants.FormatCountry, out rReturn);
            return rReturn;
        }
        
        public static string DoubleZero()
        {
            return DoubleToString(0.0F);
        }
        
        public static string DoubleToString(double sDouble)
        {
            return sDouble.ToString(NWDConstants.DoubleFormat, NWDConstants.FormatCountry);
        }
        
        public static double DoubleFromString(string sString)
        {
            double rReturn = 0.0F;
            double.TryParse(sString, NumberStyles.Float, NWDConstants.FormatCountry, out rReturn);
            return rReturn;
        }
        
        public static string ColorZero()
        {
            return ColorToString(Color.black);
        }
        
        public static string ColorToString(Color sColor)
        {
            return ColorUtility.ToHtmlStringRGBA(sColor);
        }
        
        public static Color ColorFromString(string sString)
        {
            Color tColor = new Color();
            ColorUtility.TryParseHtmlString(NWDConstants.K_HASHTAG + sString, out tColor);
            return tColor;
        }
        
        public static string AnimationCurveZero()
        {
            return AnimationCurveToString(AnimationCurve.Linear(0.0F, 0.0F, 1.0F, 1.0F));
        }
        
        public static string AnimationCurveToString(AnimationCurve sAnimationCurve)
        {
            List<string> tList = new List<string>();
            foreach (Keyframe tF in sAnimationCurve.keys)
            {
                string tV = FloatToString(tF.time) + NWDConstants.kFieldSeparatorB +
                            FloatToString(tF.value) + NWDConstants.kFieldSeparatorB +
                            FloatToString(tF.inTangent) + NWDConstants.kFieldSeparatorB +
                            FloatToString(tF.outTangent);
                tList.Add(tV);
            }
            return string.Join(NWDConstants.kFieldSeparatorA, tList.ToArray());
        }
        
        public static AnimationCurve AnimationCurveFromString(string sString)
        {
            List<Keyframe> tList = new List<Keyframe>();
            AnimationCurve rCurve = new AnimationCurve();
            string[] tKeyFrames = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tV in tKeyFrames)
            {
                string[] tFloats = tV.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                float tX = 0.0F;
                float tY = 0.0F;
                float tZ = 0.0F;
                float tW = 0.0F;
                if (tFloats.Count() == 4)
                {
                    tX = FloatFromString(tFloats[0]);
                    tY = FloatFromString(tFloats[1]);
                    tZ = FloatFromString(tFloats[2]);
                    tW = FloatFromString(tFloats[3]);
                    Keyframe tKeyframe = new Keyframe(tX, tY, tZ, tW);
                    tList.Add(tKeyframe);
                }
            }
            rCurve.keys = tList.ToArray();
            return rCurve;
        }
        
        public static string FloatRangeZero()
        {
            return FloatRangeToString(0.0F, 1.0F);
        }
        
        public static string FloatRangeToString(float sStart, float sEnd)
        {
            if (sStart <= sEnd)
            {
                return FloatToString(sStart) + NWDConstants.kFieldSeparatorA + FloatToString(sEnd);
            }
            else
            {
                return FloatToString(sEnd) + NWDConstants.kFieldSeparatorA + FloatToString(sStart);
            }
        }
        
        public static float[] FloatRangeFromString(string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tStart = 0.0F;
            float tEnd = 0.0F;
            if (tFloats.Count() == 2)
            {
                tStart = FloatFromString(tFloats[0]);
                tEnd = FloatFromString(tFloats[1]);
            }
            float[] rReturn = new float[] { tStart, tEnd };
            return rReturn;
        }
        
        public static string JsonToString(string sJSon)
        {
            return NWDToolbox.TextProtect(sJSon);
        }
        
        public static string JsonFromString(string sString)
        {
            return NWDToolbox.TextUnprotect(sString);
        }
        
        public static string RectZero()
        {
            return RectToString(new Rect(0.0F, 0.0F, 0.0F, 0.0F));
        }
        
        public static string RectToString(Rect sRect)
        {
            return FloatToString(sRect.x) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sRect.y) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sRect.height) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sRect.width);
        }
        
        public static Rect RectFromString(string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            float tHeight = 0.0F;
            float tWidth = 0.0F;
            if (tFloats.Count() == 4)
            {
                float.TryParse(tFloats[0], NumberStyles.Float, NWDConstants.FormatCountry, out tX);
                float.TryParse(tFloats[1], NumberStyles.Float, NWDConstants.FormatCountry, out tY);
                float.TryParse(tFloats[2], NumberStyles.Float, NWDConstants.FormatCountry, out tHeight);
                float.TryParse(tFloats[3], NumberStyles.Float, NWDConstants.FormatCountry, out tWidth);
            }
            Rect rReturn = new Rect(tX, tY, tHeight, tWidth);
            return rReturn;
        }
        
        public static string RectIntZero()
        {
            return RectIntToString(new RectInt(0, 0, 0, 0));
        }
        
        public static string RectIntToString(RectInt sRect)
        {
            return IntToString(sRect.x) + NWDConstants.kFieldSeparatorA +
                    IntToString(sRect.y) + NWDConstants.kFieldSeparatorA +
                    IntToString(sRect.height) + NWDConstants.kFieldSeparatorA +
                    IntToString(sRect.width);
        }
        
        public static RectInt RectIntFromString(string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            int tX = 0;
            int tY = 0;
            int tHeight = 0;
            int tWidth = 0;
            if (tFloats.Count() == 4)
            {
                int.TryParse(tFloats[0], NumberStyles.Integer, NWDConstants.FormatCountry, out tX);
                int.TryParse(tFloats[1], NumberStyles.Integer, NWDConstants.FormatCountry, out tY);
                int.TryParse(tFloats[2], NumberStyles.Integer, NWDConstants.FormatCountry, out tHeight);
                int.TryParse(tFloats[3], NumberStyles.Integer, NWDConstants.FormatCountry, out tWidth);
            }
            RectInt rReturn = new RectInt(tX, tY, tHeight, tWidth);
            return rReturn;
        }
        
        public static string Vector2IntZero()
        {
            return Vector2IntToString(new Vector2Int(0, 0));
        }
        
        public static string Vector2IntToString(Vector2Int sVector)
        {
            return IntToString(sVector.x) + NWDConstants.kFieldSeparatorA +
                    IntToString(sVector.y);
        }
        
        public static Vector2Int Vector2IntFromString(string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            int tX = 0;
            int tY = 0;
            if (tFloats.Count() == 2)
            {
                int.TryParse(tFloats[0], NumberStyles.Float, NWDConstants.FormatCountry, out tX);
                int.TryParse(tFloats[1], NumberStyles.Float, NWDConstants.FormatCountry, out tY);
            }
            Vector2Int rReturn = new Vector2Int(tX, tY);
            return rReturn;
        }
        
        public static string Vector3IntZero()
        {
            return Vector3IntToString(new Vector3Int(0, 0, 0));
        }
        
        public static string Vector3IntToString(Vector3Int sVector)
        {
            return IntToString(sVector.x) + NWDConstants.kFieldSeparatorA +
                    IntToString(sVector.y) + NWDConstants.kFieldSeparatorA +
                    IntToString(sVector.z);
        }
        
        public static Vector3Int Vector3IntFromString(string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            int tX = 0;
            int tY = 0;
            int tZ = 0;
            if (tFloats.Count() == 3)
            {
                int.TryParse(tFloats[0], NumberStyles.Float, NWDConstants.FormatCountry, out tX);
                int.TryParse(tFloats[1], NumberStyles.Float, NWDConstants.FormatCountry, out tY);
                int.TryParse(tFloats[2], NumberStyles.Float, NWDConstants.FormatCountry, out tZ);
            }
            Vector3Int rReturn = new Vector3Int(tX, tY, tZ);
            return rReturn;
        }
        
        public static string Vector2Zero()
        {
            return Vector2ToString(new Vector2(0.0F, 0.0F));
        }
        
        public static string Vector2ToString(Vector2 sVector)
        {
            return FloatToString(sVector.x) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sVector.y);
        }
        
        public static Vector2 Vector2FromString(string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            if (tFloats.Count() == 2)
            {
                float.TryParse(tFloats[0], NumberStyles.Float, NWDConstants.FormatCountry, out tX);
                float.TryParse(tFloats[1], NumberStyles.Float, NWDConstants.FormatCountry, out tY);
            }
            Vector2 rReturn = new Vector2(tX, tY);
            return rReturn;
        }
        
        public static string Vector3Zero()
        {
            return Vector3ToString(new Vector3(0.0F, 0.0F, 0.0F));
        }
        
        public static string Vector3One()
        {
            return Vector3ToString(new Vector3(1.0F, 1.0F, 1.0F));
        }
        
        public static string Vector3ToString(Vector3 sVector)
        {
            return FloatToString(sVector.x) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sVector.y) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sVector.z);
        }
        
        public static Vector3 Vector3FromString(string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            float tZ = 0.0F;
            if (tFloats.Count() == 3)
            {
                float.TryParse(tFloats[0], NumberStyles.Float, NWDConstants.FormatCountry, out tX);
                float.TryParse(tFloats[1], NumberStyles.Float, NWDConstants.FormatCountry, out tY);
                float.TryParse(tFloats[2], NumberStyles.Float, NWDConstants.FormatCountry, out tZ);
            }
            Vector3 rReturn = new Vector3(tX, tY, tZ);
            return rReturn;
        }
        
        public static string Vector4Zero()
        {
            return Vector4ToString(new Vector4(0.0F, 0.0F, 0.0F, 0.0F));
        }
        
        public static string Vector4ToString(Vector4 sVector)
        {
            return FloatToString(sVector.x) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sVector.y) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sVector.z) + NWDConstants.kFieldSeparatorA +
                    FloatToString(sVector.w);
        }
        
        public static Vector4 Vector4FromString(string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            float tZ = 0.0F;
            float tW = 0.0F;
            if (tFloats.Count() == 4)
            {
                float.TryParse(tFloats[0], NumberStyles.Float, NWDConstants.FormatCountry, out tX);
                float.TryParse(tFloats[1], NumberStyles.Float, NWDConstants.FormatCountry, out tY);
                float.TryParse(tFloats[2], NumberStyles.Float, NWDConstants.FormatCountry, out tZ);
                float.TryParse(tFloats[3], NumberStyles.Float, NWDConstants.FormatCountry, out tW);
            }
            Vector4 rReturn = new Vector4(tX, tY, tZ, tW);
            return rReturn;
        }
        
        public static string TransformToString(Transform sTransform)
        {
            return FloatToString(sTransform.position.x) + NWDConstants.kFieldSeparatorF +
                    FloatToString(sTransform.position.y) + NWDConstants.kFieldSeparatorF +
                    FloatToString(sTransform.position.z) + NWDConstants.kFieldSeparatorF +

                    FloatToString(sTransform.rotation.w) + NWDConstants.kFieldSeparatorF +
                    FloatToString(sTransform.rotation.x) + NWDConstants.kFieldSeparatorF +
                    FloatToString(sTransform.rotation.y) + NWDConstants.kFieldSeparatorF +
                    FloatToString(sTransform.rotation.z) + NWDConstants.kFieldSeparatorF +

                    FloatToString(sTransform.localScale.x) + NWDConstants.kFieldSeparatorF +
                    FloatToString(sTransform.localScale.y) + NWDConstants.kFieldSeparatorF +
                    FloatToString(sTransform.localScale.z);
        }
        
        public static void TransformFromString(Transform sTransform, string sString)
        {
            string[] tFloats = sString.Split(new string[] { NWDConstants.kFieldSeparatorF }, StringSplitOptions.RemoveEmptyEntries);
            if (tFloats.Count() == 10)
            {
                float tX = 0.0F;
                float tY = 0.0F;
                float tZ = 0.0F;
                float.TryParse(tFloats[0], NumberStyles.Float, NWDConstants.FormatCountry, out tX);
                float.TryParse(tFloats[1], NumberStyles.Float, NWDConstants.FormatCountry, out tY);
                float.TryParse(tFloats[2], NumberStyles.Float, NWDConstants.FormatCountry, out tZ);
                sTransform.position = new Vector3(tX, tY, tZ);
                float trX = 0.0F;
                float trY = 0.0F;
                float trZ = 0.0F;
                float trW = 0.0F;
                float.TryParse(tFloats[3], NumberStyles.Float, NWDConstants.FormatCountry, out trW);
                float.TryParse(tFloats[4], NumberStyles.Float, NWDConstants.FormatCountry, out trX);
                float.TryParse(tFloats[5], NumberStyles.Float, NWDConstants.FormatCountry, out trY);
                float.TryParse(tFloats[6], NumberStyles.Float, NWDConstants.FormatCountry, out trZ);
                sTransform.rotation = new Quaternion(trX, trY, trZ, trW);
                float tsX = 0.0F;
                float tsY = 0.0F;
                float tsZ = 0.0F;
                float.TryParse(tFloats[7], NumberStyles.Float, NWDConstants.FormatCountry, out tsX);
                float.TryParse(tFloats[8], NumberStyles.Float, NWDConstants.FormatCountry, out tsY);
                float.TryParse(tFloats[9], NumberStyles.Float, NWDConstants.FormatCountry, out tsZ);
                sTransform.localScale = new Vector3(tsX, tsY, tsZ);
            }
        }
        
    }
    
}

