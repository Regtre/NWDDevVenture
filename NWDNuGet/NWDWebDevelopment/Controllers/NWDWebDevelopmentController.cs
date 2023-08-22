using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using NWDCrucial.Configuration;
using NWDCrucial.Models;
using NWDFoundation.Configuration;
using NWDFoundation.Configuration.Environments;
using NWDFoundation.Logger;
using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDFoundation.WebEdition.Attributes;
using NWDHub.Configuration;
using NWDServerMiddle.Managers;
using NWDServerMiddle.Managers.ModelManagers;
using NWDWebEditor.Managers;
using NWDWebRuntime.Configuration;
using NWDWebRuntime.Managers;
using NWDWebRuntime.Models;
using NWDWebRuntime.Models.Enums;
using NWDWebStandard.Controllers;
using NWDWebStandard.Models;

namespace NWDWebDevelopment.Controllers;

public class NWDWebDevelopmentController : NWDBasicController<NWDWebDevelopmentController>
{
    [NWDWebMethodTestExclude()]
    public IActionResult Index()
    {
        if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
        {
            return View();
        }

        return View("/Views/Shared/_Error.cshtml");
    }
    [NWDWebMethodTestExclude()]
    public IActionResult LoggerTest()
    {
        if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
        {
            NWDLogger.TestLayout();
            return View("/Views/Shared/_Error.cshtml");
            //return View();
        }
        return View("/Views/Shared/_Error.cshtml");
    }
    
    private void UpgradeTables()
    {
        NWDCrucialManager.CheckAllTables();
        NWDStatisticsConsolidated.CreateTable();
        
        foreach (NWDLibraryInfos tLibrary in NWDLibrariesInstalled.LibrariesInfoList)
        {
            if (tLibrary.Information != null)
            {
                if (tLibrary.AssemblyDll != null)
                {
                    foreach (Type tType in tLibrary.AssemblyDll.GetTypes().Where(sType => sType.IsClass && !sType.IsAbstract && sType.IsSubclassOf(typeof(NWDDatabaseWebBasicModel))))
                    {
                        // NWDWebDBDataManager.CreateTable<T>();
                        MethodInfo? tMethod = typeof(NWDWebDBDataManager).GetMethod(nameof(NWDWebDBDataManager.CreateTable));
                        if (tMethod != null)
                        {
                            tMethod.MakeGenericMethod(new Type[] { tType }).Invoke(this, null);
                        }
                    }
                }
            }
        }
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult UpgradeAllTables()
    {
        if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
        {
            UpgradeTables();

            return View();
        }

        return View("/Views/Shared/_Error.cshtml");
    }

    [NWDWebMethodTestExclude()]
    public IActionResult Convention()
    {
        //PageInformation.SideBarStyle = NWDSideBarKind.None;
        return View();
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult NoMenu()
    {
        PageInformation = new NWDPageStandard();
        AddViewDataPageStandard(PageInformation);
        return View();
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult Appsettings()
    {
        //PageInformation.SideBarStyle = NWDSideBarKind.None;
        if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment == true)
        {
            return View();
        }
        else
        {
            return RedirectToAction(nameof(Error));
        }
    }

    [NWDWebMethodTestExclude()]
    public IActionResult DropAllTables()
    {
        if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
        {
            NWDCrucialManager.DeleteAllTables();
            NWDStatisticsConsolidated.DeleteTable();
            foreach (NWDLibraryInfos tLibrary in NWDLibrariesInstalled.LibrariesInfoList)
            {
                if (tLibrary.Information != null)
                {
                    if (tLibrary.AssemblyDll != null)
                    {
                        foreach (Type tType in tLibrary.AssemblyDll.GetTypes().Where(sType => sType.IsClass && !sType.IsAbstract && sType.IsSubclassOf(typeof(NWDDatabaseWebBasicModel))))
                        {
                            // NWDWebDBDataManager.DeleteTable<T>();
                            MethodInfo? tMethod = typeof(NWDWebDBDataManager).GetMethod(nameof(NWDWebDBDataManager.DeleteTable));
                            if (tMethod != null)
                            {
                                tMethod.MakeGenericMethod(new Type[] { tType }).Invoke(this, null);
                            }
                        }
                    }
                }
            }

            return View();
        }

        return View("/Views/Shared/_Error.cshtml");
    }
    
    [NWDWebMethodTestExclude()]
    public IActionResult CreateCrucialProject()
    {
        //NWDHubConfiguration.KConfig.CreateDefaultProjectAndAccount(NWDCrucialConfiguration.KConfig.CrucialEnvironmentKind);
        return View();
    }
    
    
    // [NWDWebMethodTestExclude()]
    // public IActionResult TestAuthorize()
    // {
    //     //NWDHubConfiguration.KConfig.CreateDefaultProjectAndAccount(NWDCrucialConfiguration.KConfig.CrucialEnvironmentKind);
    //     return View();
    // }
    //
    // [NWDWebMethodTestExclude()]
    // public IActionResult TestAuthorizeByService()
    // {
    //     //NWDHubConfiguration.KConfig.CreateDefaultProjectAndAccount(NWDCrucialConfiguration.KConfig.CrucialEnvironmentKind);
    //     return View();
    // }
    
    // [NWDWebMethodTestExclude()]
    // public IActionResult CreateAccountAdmin()
    // {
    //     if (NWDWebRuntimeConfiguration.KConfig.IsDevelopment)
    //     {
    //         NWDLogger.WriteLine(nameof(NWDWebDevelopmentController) + " " + nameof(CreateAccountAdmin));
    //         //UpgradeTables();
    //         List<ulong> tServicesList = new List<ulong>();
    //         NWDAccountSign tAccountSignAdmin = null;
    //         foreach (NWDDefaultAccountAndService tAccount in NWDWebDevelopmentConfiguration.KConfig.DefaultAccountAndServiceList)
    //         {
    //             NWDLogger.WriteLine("Try to create an account for " + tAccount.Email + " with admin service equal to " + tAccount.Admin.ToString());
    //             // Create sign-up 
    //             NWDRequestPlayerToken tToken = new NWDRequestPlayerToken(NWDWebRuntimeConfiguration.KConfig.GetProjectId(), NWDWebRuntimeConfiguration.KConfig.Environment);
    //             NWDAccountSign tAccountSign = NWDAccountSign.CreateEmailPassword(tAccount.Email, tAccount.Password, NWDWebRuntimeConfiguration.KConfig.GetProjectId());
    //             if (NWDAccountSignManager.CheckIfSignIsValid(tAccountSign, NWDWebRuntimeConfiguration.KConfig.GetProjectId()))
    //             {
    //                 if (NWDAccountSignManager.CheckIfSignExists(NWDWebRuntimeConfiguration.KConfig.Environment, tAccountSign) == false)
    //                 {
    //                     NWDAccountSign? tAccountSignReserved = NWDAccountSignManager.ReserveSignUp(tToken, tAccountSign);
    //                     if (tAccountSignReserved != null)
    //                     {
    //                         NWDAccountSign? tNewAccountSign = NWDAccountSignManager.CheckReservedSign(tToken, tAccountSignReserved);
    //                         if (tNewAccountSign != null)
    //                         {
    //                             NWDLogger.WriteLine("tNewAccountSign is create at database range " + tNewAccountSign.Range + " with reference = " + tNewAccountSign.Reference + " for " + tAccount.Email + " with admin service equal to " + tAccount.Admin.ToString());
    //                             INWDAccountServiceDao? tDao = NWDAccountServiceManager.GetDaoByRange(tNewAccountSign.Range);
    //                             //Create service 
    //                             foreach (ulong tService in tAccount.Services)
    //                             {
    //                                 if (tServicesList.Contains(tService) == false)
    //                                 {
    //                                     tServicesList.Add(tService);
    //                                 }
    //                                 tDao?.InsertOrUpdate(NWDWebRuntimeConfiguration.KConfig.Environment, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.Environment, tNewAccountSign.Account, tService, DateTime.UtcNow, DateTime.UtcNow.AddYears(10)));
    //                             }
    //
    //                             if (tAccount.Admin)
    //                             {
    //                                 tDao?.InsertOrUpdate(NWDWebRuntimeConfiguration.KConfig.Environment, NWDWebRuntimeConfiguration.KConfig.GetProjectId(), new NWDAccountService(NWDWebRuntimeConfiguration.KConfig.Environment, tNewAccountSign.Account, NWDWebRuntimeConfiguration.KConfig.AdminService, DateTime.UtcNow, DateTime.UtcNow.AddYears(10)));
    //                                 tAccountSignAdmin = tAccountSign;
    //                             }
    //                         }
    //                         else
    //                         {
    //                             NWDLogger.WriteLine("CheckReservedSign is false");
    //                         }
    //                     }
    //                     else
    //                     {
    //                         NWDLogger.WriteLine("ReserveSignUp is false");
    //                     }
    //                 }
    //                 else
    //                 {
    //                     NWDLogger.WriteLine("CheckIfSignExists is true, this account already sign-up!");
    //                 }
    //             }
    //             else
    //             {
    //                 NWDLogger.WriteLine("CheckIfSignIsValid is false");
    //             }
    //         }
    //         // Create Services
    //         
    //         List<NWDService> tServiceRowList = new List<NWDService>();
    //         foreach (ulong tService in tServicesList)
    //         {
    //             if (tService == NWDWebRuntimeConfiguration.KConfig.AdminService)
    //             {
    //                 //TODO Create service with name "admin"
    //                 NWDService tServiceData = new NWDService()
    //                 {
    //                     Reference = tService,
    //                     Name = "Admin of website",
    //                     GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
    //                     Environment= NWDWebRuntimeConfiguration.KConfig.Environment,
    //
    //                 };
    //                 tServiceRowList.Add(tServiceData);
    //             }
    //             else
    //             {
    //                 //Todo Create service with empty name or name = serviceID
    //                 NWDService tServiceData = new NWDService()
    //                 {
    //                     Reference = tService,
    //                     Name = " Service n° "+tService.ToString(),
    //                     GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
    //                     Environment= NWDWebRuntimeConfiguration.KConfig.Environment,
    //
    //                 };
    //                 tServiceRowList.Add(tServiceData);
    //             }
    //         }
    //         // Create Project
    //         if (tAccountSignAdmin == null)
    //         {
    //             tAccountSignAdmin = new NWDAccountSign();
    //         }
    //         NWDProjectByEnvironment tDev = new NWDProjectByEnvironment()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
    //             Environment = NWDEnvironmentKind.Dev,
    //             Status = NWDEnvironmentStatus.Active,
    //         };
    //         NWDProjectByEnvironment tPlayTest = new NWDProjectByEnvironment()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
    //             Environment = NWDEnvironmentKind.PlayTest,
    //             Status = NWDEnvironmentStatus.Active,
    //         };
    //         NWDProjectByEnvironment tQualification = new NWDProjectByEnvironment()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
    //             Environment = NWDEnvironmentKind.Qualification,
    //             Status = NWDEnvironmentStatus.Active,
    //         };
    //         NWDProjectByEnvironment tPreProduction = new NWDProjectByEnvironment()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
    //             Environment = NWDEnvironmentKind.PreProduction,
    //             Status = NWDEnvironmentStatus.Active,
    //         };
    //         NWDProjectByEnvironment tProduction = new NWDProjectByEnvironment()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
    //             Environment = NWDEnvironmentKind.Production,
    //             Status = NWDEnvironmentStatus.Active,
    //         };
    //         NWDProjectByEnvironment tPostProduction = new NWDProjectByEnvironment()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId(),
    //             Environment = NWDEnvironmentKind.PostProduction,
    //             Status = NWDEnvironmentStatus.Active,
    //         };
    //         NWDProjectDataTrack tDevTrack = new NWDProjectDataTrack()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             ProjectReference = new NWDReference<NWDProject>(NWDWebRuntimeConfiguration.KConfig.GetProjectId()),
    //             Kind = NWDEnvironmentKind.Dev,
    //             Deletable = false,
    //             Track = 0,
    //             TrackFollow = 0,
    //         };
    //         NWDProjectDataTrack tPlayTestTrack = new NWDProjectDataTrack()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             ProjectReference = new NWDReference<NWDProject>(NWDWebRuntimeConfiguration.KConfig.GetProjectId()),
    //             Kind = NWDEnvironmentKind.PlayTest,
    //             Deletable = false,
    //             Track = 1,
    //             TrackFollow = 0,
    //         };
    //         NWDProjectDataTrack tQualificationTrack = new NWDProjectDataTrack()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             ProjectReference = new NWDReference<NWDProject>(NWDWebRuntimeConfiguration.KConfig.GetProjectId()),
    //             Kind = NWDEnvironmentKind.PlayTest,
    //             Deletable = false,
    //             Track = 2,
    //             TrackFollow = 1,
    //         };
    //         NWDProjectDataTrack tPreProductionTrack = new NWDProjectDataTrack()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             ProjectReference = new NWDReference<NWDProject>(NWDWebRuntimeConfiguration.KConfig.GetProjectId()),
    //             Kind = NWDEnvironmentKind.PlayTest,
    //             Deletable = false,
    //             Track = 2,
    //             TrackFollow = 0,
    //         };
    //         NWDProjectDataTrack tProductionTrack = new NWDProjectDataTrack()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             ProjectReference = new NWDReference<NWDProject>(NWDWebRuntimeConfiguration.KConfig.GetProjectId()),
    //             Kind = NWDEnvironmentKind.PlayTest,
    //             Deletable = false,
    //             Track = 2,
    //             TrackFollow = 0,
    //         };
    //         NWDProjectDataTrack tPostProductionTrack = new NWDProjectDataTrack()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             ProjectReference = new NWDReference<NWDProject>(NWDWebRuntimeConfiguration.KConfig.GetProjectId()),
    //             Kind = NWDEnvironmentKind.PlayTest,
    //             Deletable = false,
    //             Track = 2,
    //             TrackFollow = 0,
    //         };
    //         NWDProject tProject = new NWDProject()
    //         {
    //             AccountReference = tAccountSignAdmin.Account,
    //             Reference = 1,
    //             Name = "Admin project",
    //             Development = new NWDReference<NWDProjectByEnvironment>(){Reference = tDev.Reference},
    //             PlayTest = new NWDReference<NWDProjectByEnvironment>(){Reference = tPlayTest.Reference},
    //             Qualification = new NWDReference<NWDProjectByEnvironment>(){Reference = tQualification.Reference},
    //             PreProduction = new NWDReference<NWDProjectByEnvironment>(){Reference = tPreProduction.Reference},
    //             Production = new NWDReference<NWDProjectByEnvironment>(){Reference = tProduction.Reference},
    //             PostProduction = new NWDReference<NWDProjectByEnvironment>(){Reference = tPostProduction.Reference},
    //             Tracks = new NWDReferencesArray<NWDProjectDataTrack>(new ulong[]{tDevTrack.Reference,tPlayTestTrack.Reference,tQualificationTrack.Reference, tPreProductionTrack.Reference,tProductionTrack.Reference, tPostProductionTrack.Reference}),
    //             Services = new NWDReferencesArray<NWDService>(),
    //             ClassesCustomStudio = new NWDReferencesArray<NWDClassConstruction>(),
    //             ClassesCustomPLayer = new NWDReferencesArray<NWDClassConstruction>(),
    //             ProjectName = new NWDReference<NWDProjectName>(new NWDProjectName(){Name = "Empty admin project", GetProjectId = NWDWebRuntimeConfiguration.KConfig.GetProjectId()}.Reference),
    //             ProjectPlan = new NWDReference<NWDProjectPlan>(new NWDProjectPlan(){Plan = NWDPlan.Standard}.Reference),
    //             ProjectTags = new NWDReference<NWDProjectTags>(new NWDProjectTags()),
    //         };
    //
    //         return View();
    //     }
    //
    //     return View("/Views/Shared/_Error.cshtml");
    // }
}