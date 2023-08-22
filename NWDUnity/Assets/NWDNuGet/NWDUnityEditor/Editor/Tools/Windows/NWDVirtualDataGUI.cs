using Newtonsoft.Json;
using NWDEditor;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Configuration.Permissions;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDUnityEditor.Constants;
using NWDUnityEditor.Coroutine;
using NWDUnityEditor.Engine;
using NWDUnityEditor.Services;
using NWDUnityEditor.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NWDUnityEditor.Tools
{
    [Serializable]
    public enum PreviewTextureState
    {
        A = 0,
        B = 1,
        C = 2,
        D = 3,
    }

    [Serializable]
    public class NWDVirtualDataGUI // TODO : turn this into an extension class instead !
    {
        static NWDUnityEditorMultiGUIContent kDataIsLockGUI;
        static NWDUnityEditorMultiGUIContent kDataIsUnLockGUI;
        static NWDUnityEditorMultiGUIContent kDataIsFreeGUI;

        public NWDMetaData MetaData { get; private set; }
        public List<NWDSubMetaData> SubMetaData { get; private set; }
        public bool Selected = false;

        public GUIContent TitleContent;
        public NWDUnityEditorMultiGUIContent IconContent;
        [NonSerialized]
        public UnityEngine.Object PreviewObject;
        [NonSerialized]
        PreviewTextureState PreviewTextureIsLoaded = PreviewTextureState.A;
        [NonSerialized]
        Texture PreviewTexture;
        [NonSerialized]
        Texture PreviewTextureIcon;
        [NonSerialized]
        int Previewtry;

        [NonSerialized]
        NWDEditorCoroutine UpdateCoroutine;

        public void SetMetaData (NWDMetaData sMetaData)
        {
            MetaData = sMetaData;
            NWDSubMetaData[] tSubMetaData = null;
            if (sMetaData != null && !string.IsNullOrEmpty(sMetaData.DataByDataTrack))
            {
                tSubMetaData = JsonConvert.DeserializeObject<NWDSubMetaData[]>(sMetaData.DataByDataTrack);
            }

            if (tSubMetaData != null)
            {
                SubMetaData = tSubMetaData.ToList();
            }
            else
            {
                SubMetaData = new List<NWDSubMetaData>();
            }
        }

        public bool IsLockForEdition()
        {
            return LockState() == NWDMetaDataLockerState.LockByMe;
        }

        public bool IsLockForEdition(NWDSubMetaData sSubMetaData)
        {
            return IsLockForEdition() || !sSubMetaData.CanEditData();
        }

        public NWDMetaDataLockerState LockState()
        {
            NWDMetaDataLockerState tState = NWDMetaDataLockerState.Unlock;
            if (string.IsNullOrEmpty(MetaData.LockerName) == false)
            {
                tState = NWDMetaDataLockerState.LockByAnother;
                if (MetaData.LockerName == NWDMetaDataEditorService.UniqueLockerName())
                {
                    tState = NWDMetaDataLockerState.LockByMe;
                }
            }
            return tState;
        }

        public void Update()
        {
            if (UpdateCoroutine != null)
            {
                NWDEditorCoroutineToolbox.StopCoroutine(UpdateCoroutine);
                UpdateCoroutine = null;
            }
            UpdateCoroutine = NWDEditorCoroutineToolbox.StartCoroutine(Execute());
        }

        // TO DO: This is called for every input in the data panel.
        private IEnumerator Execute()
        {
            NWDLogger.Trace("Launch Update MetaData Coroutine Request");

            try
            {
                NWDMetaDataEditorService.SyncAll(new List<NWDMetaData> { MetaData });
            }
            catch (Exception e)
            {
                NWDLogger.Error("Error while synchronizing data: " + e.Message);
            }

            yield return NWDEditorCoroutineWaitingFor.WaitingFor(NWDEditorConstants.UpdateDataSelectedRepeatEvery);
        }

        public void GenerateGuiContent(NWDUnityEditorMultiGUIContent sOrigin)
        {
            IconContent = NWDUnityEditorMultiGUIContent.NewContent(sOrigin, "", "", "");
            if (MetaData != null)
            {
                TitleContent = new GUIContent(MetaData.Title, MetaData.Description);
            }
            else
            {
                TitleContent = new GUIContent("None");
            }
            PreviewToIcon();
        }

        public void ReloadIcon()
        {
            PreviewTextureIsLoaded = PreviewTextureState.A;
        }

        private void PreviewToIcon()
        {
            switch (PreviewTextureIsLoaded)
            {
                case PreviewTextureState.A:
                    //Debug.Log("start preview " + Title);
                    Previewtry = 0;
                    GUID tPreviewGUIDA;
                    /*if (GUID.TryParse(VirtualData.PreviewGUIDstring, out tPreviewGUIDA))
                    {
                        PreviewObject = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(tPreviewGUIDA), typeof(UnityEngine.Object));
                        if (PreviewObject != null)
                        {
                            AssetPreview.GetAssetPreview(PreviewObject);
                            PreviewTextureIsLoaded = PreviewTextureState.B;
                            PreviewTextureIcon = AssetDatabase.GetCachedIcon(AssetDatabase.GUIDToAssetPath(tPreviewGUIDA));
                            if (PreviewTextureIcon != null)
                            {
                                IconContent.Pro.image = PreviewTextureIcon;
                                IconContent.NotPro.image = PreviewTextureIcon;
                            }
                        }
                        else
                        {
                            PreviewTextureIsLoaded = PreviewTextureState.D;
                        }
                    }
                    else
                    {
                        PreviewTextureIsLoaded = PreviewTextureState.D;
                    }*/
                    PreviewTextureIsLoaded = PreviewTextureState.D;
                    break;
                case PreviewTextureState.B:
                    //Debug.Log("preview is loading " + Title);
                    Previewtry++;
                    if (AssetPreview.IsLoadingAssetPreview(PreviewObject.GetInstanceID()) == false)
                    {
                        PreviewTextureIsLoaded = PreviewTextureState.C;
                    }
                    if (Previewtry > 10)
                    {
                        //Debug.Log("preview is loading " + Title + " but TOO LONG! =>  Force again!");
                        Previewtry = 0;
                        AssetPreview.GetAssetPreview(PreviewObject);
                    }
                    break;
                case PreviewTextureState.C:
                    Previewtry = 0;
                    //Debug.Log("preview draw good big icon " + Title);
                    PreviewTexture = AssetPreview.GetAssetPreview(PreviewObject);
                    if (PreviewTexture != null)
                    {
                        IconContent.Pro.image = PreviewTexture;
                        IconContent.NotPro.image = PreviewTexture;
                    }
                    else
                    {
                        //Debug.Log("preview draw good big icon " + Title + " but is null");
                    }
                    PreviewTextureIsLoaded = PreviewTextureState.D;
                    break;
                case PreviewTextureState.D:
                    //Debug.Log("finish preview");
                    break;
            }
        }

        //private void PreviewToTexture()
        //{
        //    //PreviewTexture = null;
        //    //PreviewObject = null;
        //    //if (string.IsNullOrEmpty(PreviewPath) == false)
        //    //{
        //    //    PreviewObject = AssetDatabase.LoadAssetAtPath(PreviewPath, typeof(UnityEngine.Object)) as UnityEngine.Object;
        //    //    if (PreviewObject is GameObject)
        //    //    {
        //    //        ((GameObject)PreviewObject).transform.rotation = Quaternion.Euler(-180, -180, -180);
        //    //    }
        //    //    if (PreviewObject != null)
        //    //    {
        //    //        PreviewTexture = AssetPreview.GetAssetPreview(PreviewObject);
        //    //        //PreviewTextureIsLoaded = true;
        //    //    }
        //    //}
        //}

        public Rect Draw(float sHeight, float sWidth, int sIndex, bool sInInspector, bool sVisible = true)
        {
            PreviewToIcon();
            if (kDataIsLockGUI == null)
            {
                kDataIsLockGUI = NWDUnityEditorMultiGUIContent.NewContent("NWDVirtualDataLock");
            };
            if (kDataIsUnLockGUI == null)
            {
                kDataIsUnLockGUI = NWDUnityEditorMultiGUIContent.NewContent("NWDVirtualDataUnLock");
            };
            if (kDataIsFreeGUI == null)
            {
                kDataIsFreeGUI = NWDUnityEditorMultiGUIContent.NewContent("NWDVirtualDataFree");
            };
            GUIStyle tStyle = NWDGUI.KTableRowNotSelected;
            if (sInInspector == true)
            {
                tStyle = NWDGUI.KTableRowSelected;
            }
            sHeight -= 2;
            NWDGUILayout.LineWhite();
            EditorGUILayout.BeginHorizontal(tStyle, GUILayout.MaxWidth(sWidth), GUILayout.Height(sHeight));
            Selected = GUILayout.Toggle(Selected, "", GUILayout.Width(20), GUILayout.Height(sHeight));
            if (sVisible == true)
            {
                GUILayout.Label(IconContent.GetContent(), GUILayout.Width(sHeight), GUILayout.Height(sHeight));
                EditorGUILayout.BeginVertical(GUILayout.Width(200));
                if (MetaData.Reference != 0)
                {
                    GUILayout.Label(TitleContent, EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
                    GUILayout.Label(MetaData.Description, EditorStyles.miniLabel, GUILayout.ExpandWidth(true));
                }
                else
                {
                    GUIStyle sTitleStyle = new GUIStyle(EditorStyles.boldLabel);
                    sTitleStyle.fontStyle = FontStyle.Italic;
                    GUILayout.Label(TitleContent, sTitleStyle, GUILayout.ExpandWidth(true));
                }
                EditorGUILayout.EndVertical();
                switch (LockState())
                {
                    case NWDMetaDataLockerState.Unlock:
                        GUILayout.Label(kDataIsFreeGUI.GetContent(), NWDGUI.kIconLock);
                        break;
                    case NWDMetaDataLockerState.LockByMe:
                        GUILayout.Label(kDataIsUnLockGUI.GetContent(), NWDGUI.kIconLock);
                        break;
                    case NWDMetaDataLockerState.LockByAnother:
                        GUILayout.Label(kDataIsLockGUI.GetContent(), NWDGUI.kIconLock);
                        break;
                }
                GUILayout.Label(NWDMetaDataEditorService.NicknameFromLockerName (MetaData.LockerName), GUILayout.Width(100));
                //foreach (NWDEnvironmentUnityEditor tEnv in NWDUnityEngineEditor.Instance.GetConfig().ReturnAllEnabledEnvironments())
                //{
                //    NWDVirtualDataState tState = NWDVirtualDataState.Unknow;
                //    GUILayout.Label(tState.ToString(), GUILayout.Width(20)); ;
                //}
            }
            EditorGUILayout.EndHorizontal();
            Rect rResult = GUILayoutUtility.GetLastRect();
            NWDGUILayout.Line();
            return rResult;
        }

        static public float DrawDataTracksWidth (float sHeight)
        {
            float rResult = 0;
            Dictionary<NWDEnvironmentKind, NWDDataTrackDescription[]> tEnvironments = NWDUnityEngineEditor.Instance.GetConfig().GetEnvironmentsByKind();
            foreach (KeyValuePair<NWDEnvironmentKind, NWDDataTrackDescription[]> tEnvironment in tEnvironments)
            {
                if (rResult != 0)
                {
                    rResult += 20;
                }
                foreach (NWDDataTrackDescription tDataTrack in tEnvironment.Value)
                {
                    rResult += sHeight;
                }
            }
            return rResult;
        }

        static public void DrawDataTracksTunnels(float sHeight)
        {
            bool sStarted = false;
            NWDDataTrackDescription tCurrent = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();
            GUIStyle tStyle = NWDGUI.KTableRowNotSelected;

            EditorGUILayout.BeginHorizontal(tStyle, GUILayout.Height(sHeight));

            foreach (KeyValuePair<NWDEnvironmentKind, NWDDataTrackDescription[]> tEnvironment in NWDUnityEngineEditor.Instance.GetConfig().GetEnvironmentsByKind())
            {
                if (!sStarted)
                {
                    sStarted = true;
                }
                else
                {
                    GUILayout.Space(20);
                }
                foreach (NWDDataTrackDescription tDataTrack in tEnvironment.Value)
                {
                    tDataTrack.DrawTunnel(GUILayoutUtility.GetRect(sHeight, sHeight, GUILayout.Width(sHeight)), tDataTrack == tCurrent);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        static public void DrawDataTracksTrain(float sHeight, float sWidth)
        {
            bool sStarted = false;
            NWDDataTrackDescription tCurrent = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();
            GUIStyle tStyle = NWDGUI.KTableRowNotSelected;

            EditorGUILayout.BeginHorizontal(tStyle, GUILayout.MaxWidth(sWidth), GUILayout.Height(sHeight));

            foreach (KeyValuePair<NWDEnvironmentKind, NWDDataTrackDescription[]> tEnvironment in NWDUnityEngineEditor.Instance.GetConfig().GetEnvironmentsByKind())
            {
                if (!sStarted)
                {
                    sStarted = true;
                }
                else
                {
                    GUILayout.Space(20);
                }
                foreach (NWDDataTrackDescription tDataTrack in tEnvironment.Value)
                {
                    tDataTrack.DrawLocomotive(GUILayoutUtility.GetRect(sHeight, sHeight, GUILayout.Width(sHeight)), tDataTrack == tCurrent);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        public void DrawDataTracks(bool sSelected, float sHeight, float sWidth)
        {
            bool sStarted = false;
            NWDDataTrackDescription tCurrent = NWDUnityEngineEditor.Instance.GetConfig().GetSelectedEnvironment();
            GUIStyle tStyle = NWDGUI.KTableRowNotSelected;

            if (sSelected == true)
            {
                tStyle = NWDGUI.KTableRowSelected;
            }
            EditorGUILayout.BeginHorizontal(tStyle, GUILayout.MaxWidth(sWidth), GUILayout.Height(sHeight));

            NWDSubMetaData tSubMetaData = null;
            NWDDataTrackDescription tOrigin = null;

            foreach (KeyValuePair<NWDEnvironmentKind, NWDDataTrackDescription[]> tEnvironment in NWDUnityEngineEditor.Instance.GetConfig().GetEnvironmentsByKind())
            {
                if (!sStarted)
                {
                    sStarted = true;
                }
                else
                {
                    GUILayout.Space(20);
                }
                foreach (NWDDataTrackDescription tDataTrack in tEnvironment.Value)
                {
                    tSubMetaData = GetSubMetaData(tDataTrack, false);

                    if (tSubMetaData != null && !string.IsNullOrEmpty(tSubMetaData.Data) && !tSubMetaData.Trashed)
                    {
                        if (tSubMetaData.State.HasFlag(NWDSubMetaDataState.Overriden))
                        {
                            tDataTrack.DrawWagon(GUILayoutUtility.GetRect(sHeight, sHeight, GUILayout.Width(sHeight)), tDataTrack == tCurrent);
                        }
                        else
                        {
                            tOrigin = NWDUnityEngineEditor.Instance.GetConfig().GetEnvironment(tSubMetaData.Origin);
                            if (tOrigin != null)
                            {
                                tOrigin.DrawWagon(GUILayoutUtility.GetRect(sHeight, sHeight, GUILayout.Width(sHeight)), tDataTrack == tCurrent);
                            }
                            else
                            {
                                tDataTrack.DrawWagon(GUILayoutUtility.GetRect(sHeight, sHeight, GUILayout.Width(sHeight)), tDataTrack == tCurrent);
                            }
                        }
                    }
                    else
                    {
                        tDataTrack.DrawRailway(GUILayoutUtility.GetRect(sHeight, sHeight, GUILayout.Width(sHeight)), tDataTrack == tCurrent);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        internal void UpateInformation(NWDTypeGUIInformation sInformation)
        {
            //Debug.Log("UpateInformation()");
            GUID tPreviewGUID = AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(PreviewObject));
            /*VirtualData.PreviewGUIDstring = tPreviewGUID.ToString();*/
            ReloadIcon();
            GenerateGuiContent(sInformation.tClass);
        }

        public Type DataType ()
        {
            return Type.GetType(MetaData.ClassName);
        }
        public object ExtractData (NWDDataTrackDescription sDataTrack)
        {
            Type tType = DataType();
            object rResult = null;

            if (tType != null)
            {
                NWDSubMetaData sSubMetaData = GetSubMetaData(sDataTrack);

                if (string.IsNullOrEmpty(sSubMetaData.Data))
                {
                    rResult = tType.GetConstructor(new Type[0]).Invoke(new object[0]);
                }
                else
                {
                    rResult = JsonConvert.DeserializeObject(sSubMetaData.Data, tType);
                }
            }
            else
            {
                NWDLogger.Error("Couldn't find type: " + MetaData.ClassName);
            }

            return rResult;
        }

        public NWDSubMetaData GetSubMetaData (NWDDataTrackDescription sDataTrack, bool sCreateIfNotExist = true)
        {
            NWDSubMetaData sSubMetaData = SubMetaData.FirstOrDefault(x => x.TrackId == sDataTrack.Track && x.TrackKind == sDataTrack.Kind);

            if (sCreateIfNotExist && sSubMetaData == null)
            {
                sSubMetaData = new NWDSubMetaData
                {
                    TrackId = sDataTrack.Track,
                    TrackKind = sDataTrack.Kind,
                    Data = ""
                };

                SubMetaData.Add(sSubMetaData);
            }

            return sSubMetaData;
        }

        public void Revert (NWDDataTrackDescription sOrigin, NWDDataTrackDescription sDataTrack)
        {
            Type tType = DataType();

            if (tType != null)
            {
                NWDSubMetaData tSubMetaData = GetSubMetaData(sDataTrack);
                NWDSubMetaData tOriginSubMetaData = GetSubMetaData(sOrigin);

                tSubMetaData.State = tSubMetaData.State & NWDSubMetaDataState.VisibleInBuild;
                tSubMetaData.Data = tOriginSubMetaData.Data;


                PropagateSubMetaData(sOrigin, sDataTrack, tOriginSubMetaData.Data);

                MetaData.DataByDataTrack = JsonConvert.SerializeObject(SubMetaData.ToArray());
            }
            else
            {
                NWDLogger.Error("Couldn't find type: " + MetaData.ClassName);
            }
        }

        /// <summary>
        /// Set the trash value of a SubMetaData.
        /// </summary>
        /// <param name="sDataTrack"></param>
        /// <param name="sSubMetaData"></param>
        /// <param name="sTrashed"></param>
        /// <returns>Can apply changes ?</returns>
        public bool SetTrashState(NWDDataTrackDescription sDataTrack, NWDSubMetaData sSubMetaData, bool sTrashed)
        {
            Type tType = DataType();
            object rResult = null;

            if (tType != null)
            {
                if (tType != null && !string.IsNullOrEmpty(sSubMetaData.Data))
                {
                    rResult = JsonConvert.DeserializeObject(sSubMetaData.Data, tType);

                    NWDDatabaseBasicModel tDbModel = rResult as NWDDatabaseBasicModel;
                    tDbModel.Trashed = sTrashed;

                    string sJSonData = JsonConvert.SerializeObject(rResult);
                    EditSubMetaData(sSubMetaData, null, sDataTrack, sJSonData, false);

                    PropagateSubMetaData(sDataTrack, sDataTrack, sJSonData);

                    MetaData.DataByDataTrack = JsonConvert.SerializeObject(SubMetaData.ToArray());
                    return true;
                }
                return false;
            }

            NWDLogger.Error("Couldn't find type: " + MetaData.ClassName);
            return false;

        }

        public void InsertSubMetaData (NWDDataTrackDescription sDataTrack, object sData)
        {
            Type tType = DataType();

            if (tType != null)
            {
                string sJSonData = JsonConvert.SerializeObject(sData);
                EditSubMetaData(null, sDataTrack, sJSonData, false);

                PropagateSubMetaData(sDataTrack, sDataTrack, sJSonData);

                MetaData.DataByDataTrack = JsonConvert.SerializeObject(SubMetaData.ToArray());
            }
            else
            {
                NWDLogger.Error("Couldn't find type: " + MetaData.ClassName);
            }
        }

        private NWDSubMetaData EditSubMetaData (NWDDataTrackDescription sOrigin, NWDDataTrackDescription sDataTrack, string sData, bool fromPropagation)
        {
            NWDSubMetaData tSubMetaData = GetSubMetaData(sDataTrack);
            return EditSubMetaData(tSubMetaData, sOrigin, sDataTrack, sData, fromPropagation);
        }

        public NWDSubMetaData EditSubMetaData(NWDSubMetaData sSubMetaData, NWDDataTrackDescription sOrigin, NWDDataTrackDescription sDataTrack, string sData, bool fromPropagation)
        {
            NWDDatabaseBasicModel tDbModel = JsonConvert.DeserializeObject<NWDDatabaseBasicModel>(sData);

            if (tDbModel != null)
            {
                sSubMetaData.Trashed = tDbModel.Trashed;
            }

            if (fromPropagation)
            {
                if (sSubMetaData.State.HasFlag(NWDSubMetaDataState.Overriden))
                {
                    sSubMetaData.State |= NWDSubMetaDataState.Outdated;
                }
                else
                {
                    sSubMetaData.Data = sData;
                    sSubMetaData.State = NWDSubMetaDataState.None;
                }
            }
            else
            {
                sSubMetaData.Data = sData;
                sSubMetaData.State = NWDSubMetaDataState.Overriden;
            }

            if (sOrigin != null)
            {
                sSubMetaData.Origin = sOrigin.Reference;
            }

            if (sDataTrack.Kind != NWDEnvironmentKind.Dev)
            {
                sSubMetaData.State |= NWDSubMetaDataState.VisibleInBuild;
            }

            return sSubMetaData;
        }

        private void PropagateSubMetaData (NWDDataTrackDescription sOrigin, NWDDataTrackDescription sDataTrack, string sData)
        {
            List<NWDDataTrackDescription> tStalkers = sDataTrack.GetStalkers();

            foreach (NWDDataTrackDescription tStalker in tStalkers)
            {
                NWDSubMetaData sSubMetaData = GetSubMetaData(tStalker);

                if (!sSubMetaData.State.HasFlag(NWDSubMetaDataState.Overriden))
                {
                    EditSubMetaData(sSubMetaData, sOrigin, tStalker, sData, true);
                    PropagateSubMetaData(sOrigin, tStalker, sData);
                }
                else // Make sure to update the data origin if it is overriden!
                {
                    EditSubMetaData(sSubMetaData, sOrigin, tStalker, sSubMetaData.Data, true);
                }
            }
        }
    }
}