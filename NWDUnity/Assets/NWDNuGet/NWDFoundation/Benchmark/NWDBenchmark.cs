using NWDFoundation.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace NWDFoundation.Benchmark
{
    public abstract class NWDBenchmark
    {
        private static NWDBenchmarkParameters Parameter = new NWDBenchmarkParameters();

        private static Dictionary<string, long> cStartDico = new Dictionary<string, long>();
        private static Dictionary<string, long> cStepDico = new Dictionary<string, long>();
        private static Dictionary<string, List<double>> kMethodResult = new Dictionary<string, List<double>>();
        private static Stopwatch Watch = new Stopwatch();
        private static Dictionary<string, long> cQuickStartDico = new Dictionary<string, long>();

        private static int StartCount = 0;

        static NWDBenchmark()
        {
            Watch.Start();
        }
        public static void SetParameters(NWDBenchmarkParameters sParameter)
        {
            if (sParameter != null)
            {
                Parameter = sParameter;
            }
        }
        public static NWDBenchmarkParameters GetParameters()
        {
            if (Parameter == null)
            {
                Parameter = new NWDBenchmarkParameters();
            }
            return Parameter;
        }

        public static void PrefReload()
        {
            if (Parameter != null)
            {
                Parameter.PrefReload();
            }
        }

        public static void ResetAll()
        {
            Watch.Restart();
            cStartDico.Clear();
        }

        private static string GetKey()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);
            MethodBase tM = sf.GetMethod();
            string tDot = ".";
            if (tM.IsStatic == true) { tDot = ">"; }
            string tMethod = tM.DeclaringType.Name + tDot + tM.Name;
            return tMethod;
        }

        public static void Trace()
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    NWDLogger.Trace("TRACE " + GetKey());
                }
            }
        }

        public static void Start()
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    Start(GetKey());
                }
            }
        }

        private static string GetIndentation()
        {
            string rReturn = "";
            for (int i = 0; i < StartCount; i++)
            {
                if (i == 0)
                {
                    rReturn = rReturn + "\t";
                }
                else
                {
                    rReturn = rReturn + "|\t";
                }
            }
            return rReturn;
        }

        public static void Start(string sKey)
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    if (cStartDico.ContainsKey(sKey) == true)
                    {
                    }
                    else
                    {
                        StartCount++;
                        cStartDico.Add(sKey, Watch.ElapsedMilliseconds);
                        if (cStepDico.ContainsKey(sKey) == true)
                        {
                            cStepDico[sKey] = Watch.ElapsedMilliseconds;
                        }
                        else
                        {
                            cStepDico.Add(sKey, Watch.ElapsedMilliseconds);
                        }
                        if (Parameter.BenchmarkShowStart == true)
                        {
                            string tLog = "benchmark : " + GetIndentation() + "•<b>" + sKey + "</b>\t" + " start now!";
                            NWDLogger.Trace(tLog);
                        }
                    }
                }
            }
        }

        public static void Log(string sInfos = "")
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    NWDLogger.Trace("benchmark : " + GetIndentation() + "|\t• " + " Log : " + sInfos);
                }
            }
        }

        public static void LogWarning(string sInfos = "")
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    NWDLogger.Warning("benchmark : " + GetIndentation() + "|\t !!! " + " Log : " + sInfos);
                }
            }
        }

        public static void Finish(bool sWithDebug = true, string sMoreInfos = "")
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    Finish(GetKey(), sWithDebug, sMoreInfos);
                }
            }
        }

        public static void Step(bool sWithDebug = true, string sMoreInfos = "")
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    Step(GetKey(), sWithDebug, sMoreInfos);
                }
            }
        }

        public static void Finish(string sKey, bool sWithDebug = true, string sMoreInfos = "")
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    if (cStepDico.ContainsKey(sKey) == true)
                    {
                        cStepDico[sKey] = Watch.ElapsedMilliseconds;
                    }
                    else
                    {
                        cStepDico.Add(sKey, Watch.ElapsedMilliseconds);
                    }
                    double rDelta = 0;
                    double rFrameSpend = 0;
                    if (cStartDico.ContainsKey(sKey) == true)
                    {
                        rDelta = (Watch.ElapsedMilliseconds - cStartDico[sKey]) / 1000.0F;
                        rFrameSpend = Parameter.FrameRate * rDelta;
                        string tMaxColor = Parameter.Green;
                        if (rDelta >= Parameter.kWarningDefault)
                        {
                            tMaxColor = Parameter.Orange;
                        }
                        if (rDelta >= Parameter.kMaxDefault)
                        {
                            tMaxColor = Parameter.Red;
                        }
                        if (rDelta > Parameter.BenchmarkLimit)
                        {
                            if (Parameter.BenchmarkShowStart == true)
                            {
                                NWDLogger.Trace("benchmark : " + GetIndentation() + "| <b><color=" + tMaxColor + ">" + rDelta.ToString("F3") + "</color></b>" + "");
                            }
                            string tLog = "benchmark : " + GetIndentation() + "•<b>" + sKey + "</b>\t" + " execute in <color=" + tMaxColor + ">" +
                         rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/" + Parameter.FrameRate + "Fps. " + sMoreInfos;
                            NWDLogger.Trace(tLog);
                        }
                        StartCount--;
                        cStartDico.Remove(sKey);

                        if (kMethodResult.ContainsKey(sKey) == false)
                        {
                            kMethodResult.Add(sKey, new List<double>());
                        }
                        kMethodResult[sKey].Add(rDelta);
                    }
                    else
                    {
                        NWDLogger.Error("benchmark : error '" + GetIndentation() + sKey + "' has no start value. " + sMoreInfos);
                    }
                }
            }
        }

        public static void Step(string sKey, bool sWithDebug = true, string sMoreInfos = "")
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    double rDeltaAbsolute = 0;
                    double rDelta = 0;
                    double rFrameSpend = 0;
                    long LastStep = 0;
                    if (cStartDico.ContainsKey(sKey) == true)
                    {
                        rDeltaAbsolute = (Watch.ElapsedMilliseconds - cStartDico[sKey]) / 1000.0F;
                        if (cStartDico.ContainsKey(sKey) == true)
                        {
                            LastStep = cStepDico[sKey];
                        }
                        rDelta = (Watch.ElapsedMilliseconds - LastStep) / 1000.0F;
                        rFrameSpend = Parameter.FrameRate * rDelta;
                        string tMaxColor = Parameter.Green;
                        if (rDelta >= Parameter.kWarningDefault)
                        {
                            tMaxColor = Parameter.Orange;
                        }
                        if (rDelta >= Parameter.kMaxDefault)
                        {
                            tMaxColor = Parameter.Red;
                        }
                        if (rDelta > Parameter.BenchmarkLimit)
                        {
                            string tLog = "benchmark : " + GetIndentation() + "|    <b>" + sKey + "</b>\t" + " step <color=" + tMaxColor + ">" +
                            rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/" + Parameter.FrameRate + "Fps. (Delta Absolute = " + rDeltaAbsolute.ToString("F3") + ") " + sMoreInfos;
                            NWDLogger.Trace(tLog);
                        }
                    }
                    else
                    {
                        NWDLogger.Error("benchmark : error '" + GetIndentation() + sKey + "' has no start value. " + sMoreInfos);
                    }
                    if (cStepDico.ContainsKey(sKey) == true)
                    {
                        cStepDico[sKey] = Watch.ElapsedMilliseconds;
                    }
                    else
                    {
                        cStepDico.Add(sKey, Watch.ElapsedMilliseconds);
                    }
                }
            }
        }

        public static void QuickStart(string sKey = null)
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    string tKey = string.Empty;
                    if (sKey != null)
                    {
                        tKey = GetKey();
                        tKey = tKey + " <color=" + Parameter.Blue + ">" + sKey + "</color>";
                    }
                    else
                    {
                        tKey = GetKey();
                    }
                    if (cQuickStartDico.ContainsKey(tKey) == true)
                    {
                        //string tLog = "benchmark : " + GetIndentation() + "<b>" + tKey + "</b>\t" + " all ready started!";
                    }
                    else
                    {
                        cQuickStartDico.Add(tKey, Watch.ElapsedMilliseconds);
                    }
                }
            }
        }

        public static void QuickFinish(string sKey = null)
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    string tKey = string.Empty;
                    if (sKey != null)
                    {
                        tKey = GetKey();
                        tKey = tKey + " <color=" + Parameter.Blue + ">" + sKey + "</color>";
                    }
                    else
                    {
                        tKey = GetKey();
                    }
                    if (cQuickStartDico.ContainsKey(tKey) == true)
                    {
                        double rDelta = (Watch.ElapsedMilliseconds - cQuickStartDico[tKey]) / 1000.0F;
                        cQuickStartDico.Remove(tKey);
                        if (kMethodResult.ContainsKey(tKey) == false)
                        {
                            kMethodResult.Add(tKey, new List<double>());
                        }
                        kMethodResult[tKey].Add(rDelta);
                    }
                    else
                    {
                        NWDLogger.Error("benchmark : error '" + tKey + "' has no QuickStart value.");
                    }
                }
            }
        }

        public static void ResetAllResults()
        {
            kMethodResult = new Dictionary<string, List<double>>();
        }

        public static void AllResults()
        {
            if (Parameter != null)
            {
                if (Parameter.IsEnable == true)
                {
                    foreach (KeyValuePair<string, List<double>> tResult in kMethodResult)
                    {
                        List<double> tResultList = tResult.Value.OrderByDescending(d => d).ToList();
                        double rRawDelta = Enumerable.Average(tResultList);
                        double rRawSum = Enumerable.Sum(tResultList);
                        double rRawMax = Enumerable.Max(tResultList);
                        double rRawMin = Enumerable.Min(tResultList);
                        double rDelta = rRawDelta;
                        double rMax = rRawMax;
                        double rMin = rRawMin;
                        bool AA = false;
                        int tD = 0;
                        if (tResultList.Count > 20)
                        {
                            AA = true;
                            int tCount = tResultList.Count;
                            int tNN = (int)Math.Floor(tCount * 0.95);
                            tD = tCount - tNN;
                            for (int i = 0; i < tD; i++)
                            {
                                //tResultList.RemoveAt(tResultList.Count-1);
                                tResultList.RemoveAt(0);
                            }
                            rMax = Enumerable.Max(tResultList);
                            rMin = Enumerable.Min(tResultList);
                        }
                        rDelta = Enumerable.Average(tResultList);
                        string tMaxColor = Parameter.Green;
                        if (rDelta >= Parameter.kWarningDefault)
                        {
                            tMaxColor = Parameter.Orange;
                        }
                        if (rDelta >= Parameter.kMaxDefault)
                        {
                            tMaxColor = Parameter.Red;
                        }
                        string tSumColor = Parameter.Green;
                        if (rRawSum >= Parameter.kWarningDefault)
                        {
                            tSumColor = Parameter.Orange;
                        }
                        if (rRawSum >= Parameter.kMaxDefault)
                        {
                            tSumColor = Parameter.Red;
                        }
                        NWDLogger.Trace("benchmark Result " +
                            "'<b>" + tResult.Key + "</b>' has " + tResultList.Count + " value" + (tResult.Value.Count > 1 ? "s" : "") +
                            " and average is <color=" + tMaxColor + ">" + rDelta.ToString("F6") + "</color> seconds" +
                            (tResult.Value.Count > 1 ? " (min " + rMin.ToString("F6") + " max " + rMax.ToString("F6") + ")" : "") +
                            (AA == true ? " at 95%  with " + tResult.Value.Count + " raw datas average <color=" + tMaxColor + ">" + rRawDelta.ToString("F6") + "</color> seconds (min " + rRawMin.ToString("F6") + " max " + rRawMax.ToString("F6") + ") " : " ") +
                            (tResult.Value.Count > 1 ? " sum is <color=" + tSumColor + ">" + rRawSum.ToString("F6") + "</color> seconds" : "") +

                            ""
                            );
                    }
                }
            }
        }
    }
}