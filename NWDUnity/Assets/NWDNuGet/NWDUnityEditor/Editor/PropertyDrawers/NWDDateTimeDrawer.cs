using NWDFoundation.Models;
using NWDFoundation.Tools;
using NWDUnityEditor.Attributes;
using NWDUnityEditor.Tools;
using NWDUnityShared.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.PropertyDrawers
{
    [NWDCustomPropertyDrawer(typeof(NWDDateTime), true)]
    public class NWDDateTimeDrawer : NWDPropertyDrawer
    {
        static private Dictionary<int, string[]> kDaysOfTheMounth = new Dictionary<int, string[]>();
        static public string[] GetDaysOfTheMounth (int sDaysAmount)
        {
            if (!kDaysOfTheMounth.ContainsKey(sDaysAmount))
            {
                kDaysOfTheMounth.Add(sDaysAmount, Enumerable.Range(1, sDaysAmount).Select(x => x.ToString("00")).ToArray());
            }

            return kDaysOfTheMounth[sDaysAmount];
        }

        static public readonly string[] kDayNames = new string[] {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };
        static public readonly string[] kMonths = new string[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };

        static private string[] kYears = null;
        static public string[] GetYears ()
        {
            if (kYears == null)
            {
                kYears = Enumerable.Range(1900, 200).Select(x => x.ToString()).ToArray();
            }
            return kYears;
        }

        static private string[] kHours = null;
        static public string[] GetHours()
        {
            if (kHours == null)
            {
                kHours = Enumerable.Range(0, 24).Select(x => x.ToString("00")).ToArray();
            }
            return kHours;
        }

        static private string[] kMinutes = null;
        static public string[] GetMinutes()
        {
            if (kMinutes == null)
            {
                kMinutes = Enumerable.Range(0, 60).Select(x => x.ToString("00")).ToArray();
            }
            return kMinutes;
        }

        static private string[] kSeconds = null;
        static public string[] GetSeconds()
        {
            if (kSeconds == null)
            {
                kSeconds = Enumerable.Range(0, 60).Select(x => x.ToString("00")).ToArray();
            }
            return kSeconds;
        }

        public NWDModelTypeInformation Info = null;
        public NWDSerializedProperty TimestampProperty;
        DateTime Current;

        public NWDDateTimeDrawer() : base()
        {
        }

        public NWDDateTimeDrawer(PropertyInfo sPropertyInfo) : base(sPropertyInfo)
        {
        }

        public override float GetPropertyHeight(NWDSerializedProperty sProperty, string sDisplayName)
        {
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect sPosition, NWDSerializedProperty sProperty, string slabel)
        {
            Init(sProperty);
            NWDDateTime sValue = (NWDDateTime)sProperty.GetValue();

            Current = NWDTimestamp.TimeStampToDateTime ((long)sValue.Timstamp);

            sPosition.height = EditorGUIUtility.singleLineHeight;
            sPosition = EditorGUI.PrefixLabel(sPosition, new GUIContent(slabel));

            OnDateGUI(sPosition, ref Current);

            sPosition.y += sPosition.height + EditorGUIUtility.standardVerticalSpacing;

            OnTimeGUI(sPosition, ref Current);

            sValue.Timstamp = NWDTimestamp.Timestamp(Current);

            sProperty.SetValue(sValue);
        }
        private void Init(NWDSerializedProperty sProperty)
        {
            if (Info == null)
            {
                Info = NWDModelType.ModelTypeInformationByType[sProperty.PropertyType];
                TimestampProperty = Info.Fields[0].GetProperty();
            }
        }

        protected void OnDateGUI (Rect sPosition, ref DateTime sDateTime) // Put that into NWDGUI
        {
            int tYear = sDateTime.Year;
            int tMounth = sDateTime.Month;
            int tDay = sDateTime.Day;

            Rect tControlPosition = sPosition;
            tControlPosition.width = 55;
            tYear = EditorGUI.Popup(tControlPosition, tYear - 1900, GetYears()) + 1900;

            tControlPosition.x += tControlPosition.width;
            tControlPosition.width = 10;
            EditorGUI.LabelField(tControlPosition, "/");

            tControlPosition.x += tControlPosition.width;
            tControlPosition.width = sPosition.width - (55 + 40 + 10 * 2);
            tMounth = EditorGUI.Popup(tControlPosition, tMounth - 1, kMonths) + 1;

            tControlPosition.x += tControlPosition.width;
            tControlPosition.width = 10;
            EditorGUI.LabelField(tControlPosition, "/");

            tControlPosition.x += tControlPosition.width;
            tControlPosition.width = 40;
            int tDaysInMounth = DateTime.DaysInMonth(tYear, tMounth);

            if (tDay > tDaysInMounth)
            {
                tDay = tDaysInMounth;
            }

            tDay = EditorGUI.Popup(tControlPosition, tDay - 1, GetDaysOfTheMounth (tDaysInMounth)) + 1;

            sDateTime = new DateTime(tYear, tMounth, tDay, sDateTime.Hour, sDateTime.Minute, sDateTime.Second, DateTimeKind.Utc);
        }
        protected void OnTimeGUI(Rect sPosition, ref DateTime sDateTime)
        {
            int tHour = sDateTime.Hour;
            int tMinute = sDateTime.Minute;
            int tSecond = sDateTime.Second;

            float tControlWidth = (sPosition.width - 10 * 2) / 3;

            Rect tControlPosition = sPosition;
            tControlPosition.width = tControlWidth;
            tHour = EditorGUI.Popup(tControlPosition, tHour, GetHours());

            tControlPosition.x += tControlPosition.width;
            tControlPosition.width = 10;
            EditorGUI.LabelField(tControlPosition, ":");

            tControlPosition.x += tControlPosition.width;
            tControlPosition.width = tControlWidth;
            tMinute = EditorGUI.Popup(tControlPosition, tMinute, GetMinutes());

            tControlPosition.x += tControlPosition.width;
            tControlPosition.width = 10;
            EditorGUI.LabelField(tControlPosition, ":");

            tControlPosition.x += tControlPosition.width;
            tControlPosition.width = tControlWidth;
            tSecond = EditorGUI.Popup(tControlPosition, tSecond, GetSeconds());

            sDateTime = new DateTime(sDateTime.Year, sDateTime.Month, sDateTime.Day, tHour, tMinute, tSecond, DateTimeKind.Utc);
        }
    }
}
