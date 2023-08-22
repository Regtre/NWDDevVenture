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

public class NWDEnvironmentQualificationEditionController : NWDModelEditionAsyncController<NWDProjectQualificationDataTrack>
{
    private const NWDEnvironmentKind _ENVIRONMENT_KIND = NWDEnvironmentKind.Qualification;
    private const int _TRACK_LENGHT = 2;
    private const int _TRACK_MIN = 300;

    public override string IndexJavascriptPostOperation(NWDProjectQualificationDataTrack? sObject)
    {
        return "";
    }

    public override string ListAllJavascriptPostOperation(NWDProjectQualificationDataTrack? sObject)
    {
        return "" +
               // "alert('Yes I nock you!');" +
               "startRequestAndReplaceContent('NWDEnvironmentProductionEdition');" +
               "startRequestAndReplaceContent('NWDEnvironmentPreProductionEdition');" +
               "startRequestAndReplaceContent('NWDEnvironmentPostProductionEdition');";
    }

    public override string ViewAllJavascriptPostOperation(NWDProjectQualificationDataTrack? sObject)
    {
        return "";
    }

    public override string PreviewJavascriptPostOperation(NWDProjectQualificationDataTrack? sObject)
    {
        return "";
    }

    public override string EditJavascriptPostOperation(NWDProjectQualificationDataTrack? sObject)
    {
        return "";
    }

    public override string ModifyJavascriptPostOperation(NWDProjectQualificationDataTrack? sObject)
    {
        return "";
    }

    public override string TrashJavascriptPostOperation(NWDProjectQualificationDataTrack? sObject)
    {
        return "";
    }

    public override string ShowJavascriptPostOperation(NWDProjectQualificationDataTrack? sObject)
    {
        return "";
    }

    public void TestDevTrackZero(NWDProject? sProject, HttpContext sHttpContext)
    {
        if (sProject != null)
        {
            sProject.TestDevTrackZero(sHttpContext);
        }
    }

    public override void PrepareBeforeEdit(NWDProjectQualificationDataTrack? sObject, Dictionary<string, string>? sDictionary, HttpContext sHttpContext)
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

                    List<NWDProjectPlayTestDataTrack> tAllListB = NWDWebDataManager.GetAllData<NWDProjectPlayTestDataTrack>(sHttpContext);
                    foreach (NWDProjectDataTrack tObject in tAllListB)
                    {
                        if (tObject.Trashed == false && tObject.Project == tProjectRef && tObject.Kind == NWDEnvironmentKind.PlayTest && tObject.Reference != sObject.Reference)
                        {
                            tList.Add(tObject);
                            tTestCircular.TryAdd(tObject.Track, tObject.TrackFollow);
                        }
                    }

                    List<NWDProjectQualificationDataTrack> tAllListC = NWDWebDataManager.GetAllData<NWDProjectQualificationDataTrack>(sHttpContext);
                    foreach (NWDProjectDataTrack tObject in tAllListC)
                    {
                        if (tObject.Trashed == false && tObject.Active == true && tObject.Project == tProjectRef && tObject.Kind == NWDEnvironmentKind.Qualification && tObject.Reference != sObject.Reference)
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
        Func<NWDProjectQualificationDataTrack, bool> tTest = (sData => sData.Reference > 0);
        if (sDictionary.ContainsKey(nameof(NWDProjectSubObject.Project)))
        {
            ulong tProject = ulong.Parse(sDictionary[nameof(NWDProjectSubObject.Project)]);
            tTest = (sData => sData.Project == tProject && sData.Kind == _ENVIRONMENT_KIND);
        }

        return (Func<T, bool>)tTest;
    }

    public override void NewAddon(NWDProjectQualificationDataTrack sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        sObject.Kind = _ENVIRONMENT_KIND;
        sObject.Publish = NWDProjectPartStatus.New;
        ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
        NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
        if (tProject != null)
        {
            sObject.AssociateToProject(tProject);
            List<NWDProjectQualificationDataTrack> tList = NWDWebDataManager.GetAllData<NWDProjectQualificationDataTrack>(sHttpContext);
            tList.Remove(sObject);
            bool tTestTrack = false;
            sObject.Track = _TRACK_MIN;
            while (tTestTrack == false)
            {
                tTestTrack = true;
                foreach (NWDProjectQualificationDataTrack tObject in tList)
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

    public override void UpdateAddon(NWDProjectQualificationDataTrack sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
        ulong tProjectReference = ulong.Parse(sValues[nameof(NWDProjectSubObject.Project)]);
        NWDProject? tProject = NWDWebDataManager.GetDataByReference<NWDProject>(sHttpContext, tProjectReference);
        if (tProject != null)
        {
            sObject.AssociateToProject(tProject);
            //Compare sObject to database
            NWDProjectQualificationDataTrack? tOldObject = NWDWebDataManager.GetDataByReference<NWDProjectQualificationDataTrack>(sHttpContext, sObject.Reference);
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


                NWDProjectPublishDataTrack? tPreProd = null;
                NWDProjectPublishDataTrack? tProd = null;
                NWDProjectPublishDataTrack? tPostProd = null;

                foreach (NWDProjectPublishDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectPublishDataTrack>(sHttpContext))
                {
                    if (tObject.Project == sObject.Project)
                    {
                        if (tObject.TrackFollow == sObject.Track)
                        {
                            if (tObject.Kind == NWDEnvironmentKind.PreProduction)
                            {
                                tPreProd = tObject;
                            }

                            if (tObject.Kind == NWDEnvironmentKind.Production)
                            {
                                tProd = tObject;
                            }

                            if (tObject.Kind == NWDEnvironmentKind.PostProduction)
                            {
                                tPostProd = tObject;
                            }
                        }
                    }
                }

                if (tPreProd == null)
                {
                    tPreProd = new NWDProjectPublishDataTrack(tProject)
                    {
                        Kind = NWDEnvironmentKind.PreProduction,
                        UsualName = sObject.UsualName,
                        Color = sObject.Color,
                        Track = sObject.Track + 1000,
                        TrackFollow = sObject.Track,
                    };
                }

                if (tProd == null)
                {
                    tProd = new NWDProjectPublishDataTrack(tProject)
                    {
                        Kind = NWDEnvironmentKind.Production,
                        UsualName = sObject.UsualName,
                        Color = sObject.Color,
                        Track = sObject.Track + 2000,
                        TrackFollow = sObject.Track,
                    };
                }

                if (tPostProd == null)
                {
                    tPostProd = new NWDProjectPublishDataTrack(tProject)
                    {
                        Kind = NWDEnvironmentKind.PostProduction,
                        UsualName = sObject.UsualName,
                        Color = sObject.Color,
                        Track = sObject.Track + 3000,
                        TrackFollow = sObject.Track,
                    };
                }

                tPreProd.UsualName = sObject.UsualName;
                tPreProd.Color = sObject.Color;
                tPreProd.Publish = sObject.Publish;

                tProd.UsualName = sObject.UsualName;
                tProd.Color = sObject.Color;
                tProd.Publish = sObject.Publish;

                tPostProd.UsualName = sObject.UsualName;
                tPostProd.Color = sObject.Color;
                tPostProd.Publish = sObject.Publish;

                NWDWebDataManager.SaveData(sHttpContext, tPreProd);
                NWDWebDataManager.SaveData(sHttpContext, tProd);
                NWDWebDataManager.SaveData(sHttpContext, tPostProd);
            }
        }
    }

    public override void DeleteAddon(NWDProjectQualificationDataTrack sObject, Dictionary<string, string> sValues, HttpContext sHttpContext)
    {
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

        // foreach (NWDProjectDevDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectDevDataTrack>(sHttpContext))
        // {
        //     if (tObject.TrackFollow == sObject.Track && tObject.Project == sObject.Project && tObject.Kind == NWDEnvironmentKind.Dev && tObject.Reference != sObject.Reference)
        //     {
        //         tObject.TrackFollow = 100;
        // if (tObject.Publish != NWDProjectPartStatus.New)
        // {
        //     tObject.Publish = NWDProjectPartStatus.Upgrading;
        // }
        //         NWDWebDataManager.SaveData(sHttpContext, tObject);
        //         AddJavascript("startRequestAndReplaceContent('NWDEnvironmentDevEdition');");
        //     }
        // }
        //
        // foreach (NWDProjectPlayTestDataTrack tObject in NWDWebDataManager.GetAllData<NWDProjectPlayTestDataTrack>(sHttpContext))
        // {
        //     if (tObject.TrackFollow == sObject.Track && tObject.Project == sObject.Project && tObject.Kind == NWDEnvironmentKind.PlayTest && tObject.Reference != sObject.Reference)
        //     {
        //         tObject.TrackFollow = 100;
        // if (tObject.Publish != NWDProjectPartStatus.New)
        // {
        //     tObject.Publish = NWDProjectPartStatus.Upgrading;
        // }
        //         NWDWebDataManager.SaveData(sHttpContext, tObject);
        //         AddJavascript("startRequestAndReplaceContent('NWDEnvironmentPlayTestEdition');");
        //     }
        // }
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
        List<NWDProjectPublishDataTrack> tList = NWDWebDataManager.GetAllData<NWDProjectPublishDataTrack>(sHttpContext);
        foreach (NWDProjectPublishDataTrack tObject in tList)
        {
            if (tObject.Project == sObject.Project)
            {
                if (tObject.TrackFollow == sObject.Track)
                {
                    tObject.Publish = NWDProjectPartStatus.Inactive;
                    NWDWebDataManager.DeleteData(sHttpContext, tObject);
                }
            }
        }
    }
}