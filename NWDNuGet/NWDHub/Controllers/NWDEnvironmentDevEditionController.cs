using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.Tools;
using NWDHub.Models;
using NWDWebRuntime.Managers;
using NWDWebStandard.Controllers;
using NWDCrucial.Models;
using NWDFoundation.Logger;
using NWDWebEditor.Controllers;

namespace NWDHub.Controllers;

public class NWDEnvironmentDevEditionController : NWDModelEditionAsyncController<NWDProjectDevDataTrack>
{
    private const NWDEnvironmentKind _ENVIRONMENT_KIND = NWDEnvironmentKind.Dev;
    private const int _TRACK_LENGHT = 2;
    private const int _TRACK_MIN = 100;

    public void TestDevTrackZero(NWDProject? sProject, HttpContext sHttpContext)
    {
        if (sProject != null)
        {
            sProject.TestDevTrackZero(sHttpContext);
        }
    }

    public override void PrepareBeforeEdit(NWDProjectDevDataTrack? sObject, Dictionary<string, string>? sDictionary, HttpContext sHttpContext)
    {
        ulong tProjectRef = 0;
        Dictionary<string, int>? tDico = new Dictionary<string, int>();
        if (sDictionary != null)
        {
            if (sDictionary.ContainsKey(nameof(NWDProjectSubObject.Project)))
            {
                tProjectRef = ulong.Parse(sDictionary[nameof(NWDProjectSubObject.Project)]);
            }
        }

        if (sObject != null)
        {
            TestDevTrackZero(NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, sObject.Project), sHttpContext);
            if (sObject.Project != 0)
            {
                tProjectRef = sObject.Project;
            }
            NWDLogger.TraceAttention(tProjectRef.ToString());
            if (tProjectRef != 0)
            {
                NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectRef);
                if (sObject.Track != 100)
                {
                    NWDLogger.TraceAttention(" track " + sObject.Track.ToString());
                    List<NWDProjectDataTrack> tList = new List<NWDProjectDataTrack>();
                    Dictionary<int, int> tTestCircular = new Dictionary<int, int>();
                    List<NWDProjectDevDataTrack> tAllList = NWDWebDataManager.GetAllData<NWDProjectDevDataTrack>(sHttpContext);
                    foreach (NWDProjectDataTrack tObject in tAllList)
                    {
                        if (tObject.Trashed == false && tObject.Active == true && tObject.Project == tProjectRef && tObject.Kind == NWDEnvironmentKind.Dev && tObject.Reference != sObject.Reference)
                        {
                            NWDLogger.TraceAttention(" tObject track " + tObject.Track.ToString());
                            tList.Add(tObject);
                            tTestCircular.TryAdd(tObject.Track, tObject.TrackFollow);
                        }
                    }

                    if (tTestCircular.ContainsKey(100) == false)
                    {
                        tTestCircular.Add(100,0);
                    }
                    foreach (NWDProjectDataTrack tObject in tList)
                    {
                        // calcul de circularité si on utilise cette valeur  
                        bool tValid = true;
                        bool tBreak = false;
                        List<int> tPath = new List<int>();
                        int tActual = sObject.Track;
                        tPath.Add(tActual);
                        tActual = tObject.Track;
                        while (tBreak == false)
                        {
                            tPath.Add(tActual);
                            if (tTestCircular.ContainsKey(tActual))
                            {
                                tActual = tTestCircular[tActual];
                                if (tPath.Contains(tActual))
                                {
                                    tValid = false;
                                    tBreak = true;
                                }
                            }
                            else
                            {
                                if (tActual == 0)
                                {
                                    tBreak = true;
                                }
                                else
                                {
                                    tValid = false;
                                    tBreak = true;
                                }
                            }
                        }

                        if (tValid == true)
                        {
                            string tName = tObject.UsualName + " ( track : " + tObject.Track + ")";
                            tDico.TryAdd(tName, tObject.Track);
                        }
                    }
                }

                NWDLogger.TraceAttention(" tDico count => " + tDico.Count().ToString());
                ViewData["TracksAvailable"] = tDico;
            }
        }
    }

    protected override Func<T, bool> Filter<T>(Dictionary<string, string> sDictionary)
    {
        Func<NWDProjectDevDataTrack, bool> tTest = (sData => sData.Reference > 0);
        if (sDictionary.ContainsKey(nameof(NWDProjectSubObject.Project)))
        {
            ulong tProject = ulong.Parse(sDictionary[nameof(NWDProjectSubObject.Project)]);
            tTest = (sData => sData.Project == tProject && sData.Kind == _ENVIRONMENT_KIND);
        }

        return (Func<T, bool>)tTest;
    }

    public override void NewAddon(NWDProjectDevDataTrack sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        sObject.Kind = _ENVIRONMENT_KIND;
        sObject.Publish = NWDProjectPartStatus.New;
        ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
        NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
        if (tProject != null)
        {
            TestDevTrackZero(tProject, sHttpContext);
            sObject.AssociateToProject(tProject);
            List<NWDProjectDevDataTrack> tList = NWDWebDataManager.GetAllData<NWDProjectDevDataTrack>(sHttpContext);
            tList.Remove(sObject);
            bool tTestTrack = false;
            sObject.Track = _TRACK_MIN;
            while (tTestTrack == false)
            {
                tTestTrack = true;
                foreach (NWDProjectDevDataTrack tObject in tList)
                {
                    if (tObject.Track == sObject.Track)
                    {
                        tTestTrack = false;
                        sObject.Track = _TRACK_MIN + NWDRandom.IntNumeric(_TRACK_LENGHT);
                        break;
                    }
                }
            }
        }
    }

    public override void UpdateAddon(NWDProjectDevDataTrack sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
        NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
        if (sObject.TrackFollow < 100)
        {
            sObject.TrackFollow = 100;
        }

        if (sObject.Track == 100)
        {
            sObject.TrackFollow = 0;
        }

        if (tProject != null)
        {
            TestDevTrackZero(tProject, sHttpContext);
            sObject.AssociateToProject(tProject);
            //Compare sObject to database
            NWDProjectDevDataTrack? tOldObject = NWDWebDataManager.GetDataByReference<NWDProjectDevDataTrack>(sHttpContext, sObject.Reference);
            string tOldJson = JsonConvert.SerializeObject(tOldObject);
            string tNewJson = JsonConvert.SerializeObject(sObject);
            if (tOldJson != tNewJson)
            {
                if (sObject.Publish != NWDProjectPartStatus.New)
                {
                    sObject.Publish = NWDProjectPartStatus.Upgrading;
                }
                tProject.NeedToBePublish = true;
                AddJavascript( "ShowOrHiddeContent('ProjectNeedBeUpdate', true);");
                AddJavascript( "ShowOrHiddeContent('ProjectNotBeUpdate', false);");
                NWDWebDataManager.SaveData(sHttpContext, tProject);
                tProject.TestTrackFollow(sHttpContext);
            }
        }
    }

    public override void DeleteAddon(NWDProjectDevDataTrack sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        TestDevTrackZero(NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, sObject.Project), sHttpContext);
        ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
        NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
        if (tProject != null)
        {
            sObject.AssociateToProject(tProject);
            tProject.NeedToBePublish = true;
        AddJavascript( "ShowOrHiddeContent('ProjectNeedBeUpdate', true);");
        AddJavascript( "ShowOrHiddeContent('ProjectNotBeUpdate', false);");
            NWDWebDataManager.SaveData(sHttpContext, tProject);
            tProject.TestTrackFollow(sHttpContext);
        }
        foreach (NWDProjectDevDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectDevDataTrack>(sHttpContext))
        {
            if (tObject.TrackFollow == sObject.Track && tObject.Project == sObject.Project && tObject.Kind == NWDEnvironmentKind.Dev && tObject.Reference != sObject.Reference)
            {
                tObject.TrackFollow = 100;
                if (tObject.Publish != NWDProjectPartStatus.New)
                {
                    tObject.Publish = NWDProjectPartStatus.Upgrading;
                }
                NWDWebDataManager.SaveData(sHttpContext, tObject);
                //AddJavascript("startRequestAndReplaceContent('NWDEnvironmentDevEdition');");
            }
        }
        foreach (NWDProjectPlayTestDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectPlayTestDataTrack>(sHttpContext))
        {
            if (tObject.TrackFollow == sObject.Track && tObject.Project == sObject.Project && tObject.Kind == NWDEnvironmentKind.PlayTest && tObject.Reference != sObject.Reference)
            {
                tObject.TrackFollow = 100;
                if (tObject.Publish != NWDProjectPartStatus.New)
                {
                    tObject.Publish = NWDProjectPartStatus.Upgrading;
                }
                NWDWebDataManager.SaveData(sHttpContext, tObject);
                AddJavascript( "startRequestAndReplaceContent('NWDEnvironmentPlayTestEdition');");
            }
        }
        foreach (NWDProjectQualificationDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectQualificationDataTrack>(sHttpContext))
        {
            if (tObject.TrackFollow == sObject.Track && tObject.Project == sObject.Project && tObject.Kind == NWDEnvironmentKind.Qualification && tObject.Reference != sObject.Reference)
            {
                tObject.TrackFollow = 100;
                if (tObject.Publish != NWDProjectPartStatus.New)
                {
                    tObject.Publish = NWDProjectPartStatus.Upgrading;
                }
                NWDWebDataManager.SaveData(sHttpContext, tObject);
                AddJavascript("startRequestAndReplaceContent('NWDEnvironmentQualificationEdition');");
            }
        }
        sObject.Publish = NWDProjectPartStatus.Inactive;
    }
}