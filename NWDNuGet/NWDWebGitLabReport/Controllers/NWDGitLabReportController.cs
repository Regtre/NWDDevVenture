using GitLabApiClient;
using GitLabApiClient.Models.Issues.Requests;
using Microsoft.AspNetCore.Mvc;
using NWDFoundation.Logger;
using NWDWebGitLabReport.Configuration;
using NWDWebGitLabReport.Models;
using NWDWebRuntime.Controllers;
using NWDWebStandard.Controllers;

namespace NWDWebGitLabReport.Controllers
{
    public class NWDGitLabReportController : NWDBasicController<NWDGitLabReportController>
    {
        //GET: /<controller>/
        //[HttpGet]
        //public async Task<IActionResult> ViewSecretToken(GitLabProjectModel sModel)
        //{
        //    string tSecretToken = string.Empty;
        //    string param1 = HttpUtility.ParseQueryString(HttpContext.Request.Query).Get("param1");
        //    if (string.IsNullOrEmpty(tSecretToken) == false)
        //    {
        //        NWDGitLabMilestonesListModel tGitLabMilestoneList = await NWDGitLabMilestonesListModel.Prepare(tSecretToken);
        //        ViewData.Add(nameof(NWDGitLabMilestonesListModel), tGitLabMilestoneList);
        //    }
        //    return View(ViewsConstants.ViewsPath<NWDGitLabReportController>("Index"));
        //}
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> View(NWDProjectGitConnection sModel)
        {
            try
            {
                NWDGitLabMilestonesListModel tGitLabMilestoneList = await NWDGitLabMilestonesListModel.Prepare(sModel.LocalTokenReport,HttpContext);
                ViewData.Add(nameof(NWDGitLabMilestonesListModel), tGitLabMilestoneList);
                NWDProjectGitConnection? tModel = NWDWebGitLabReportConfiguration.KConfig.GetBySecretToken(sModel.LocalTokenReport);
                if (tModel != null)
                {
                    ViewData.Add(nameof(NWDProjectGitConnection), tModel);
                    ViewData.Add("Title", tModel.Name);
                }
            }
            catch (Exception tException)
            {
                NWDLogger.Exception(tException);
                return View("/Views/Shared/_Error.cshtml");
            }
            return View("/Views/NWDGitLabReport/Index.cshtml");
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> ViewLastReport()
        {
            // NWDLogger.WriteLine(nameof(ViewLastReport));

            List<NWDProjectGitConnection> tList = NWDWebGitLabReportConfiguration.KConfig.Repositories;

            List<NWDGitLabMilestoneModel> tGitLabMilestoneModelList = new List<NWDGitLabMilestoneModel>();

            foreach (NWDProjectGitConnection tGit in tList)
            {
                // NWDLogger.WriteLine(tGit.GitProject);
                GitLabClient tGitlabClient = new GitLabClient(tGit.GitUrl, tGit.GitToken);
                var tMilestones = await tGitlabClient.Projects.GetMilestonesAsync(tGit.GitProject);
                foreach (GitLabApiClient.Models.Milestones.Responses.Milestone tM in tMilestones)
                {
                    // NWDLogger.WriteLine(tM.Title);
                    NWDGitLabMilestoneModel tGitLabMilestoneModel = new NWDGitLabMilestoneModel();
                    tGitLabMilestoneModel.Name = tM.Title;
                    tGitLabMilestoneModel.SecretToken = tGit.LocalTokenReport;
                    if (string.IsNullOrEmpty(tM.StartDate) == false)
                    {
                        tGitLabMilestoneModel.Start = DateTime.Parse(tM.StartDate);
                    }
                    if (string.IsNullOrEmpty(tM.DueDate) == false)
                    {
                        tGitLabMilestoneModel.Due = DateTime.Parse(tM.DueDate);
                    }
                    if (tGitLabMilestoneModel.Start <= DateTime.Now && tGitLabMilestoneModel.Due >= DateTime.Now)
                    {
                        //NWDLogger.WriteLine("I ADD MILESTONES " + tGitLabMilestoneModel.Name);
                        await tGitLabMilestoneModel.Prepare(tGit.LocalTokenReport, null, null);
                        tGitLabMilestoneModel.GitLabProject = tGit;
                        tGitLabMilestoneModelList.Add(tGitLabMilestoneModel);
                    }

                }
            }

            ViewData.Add(nameof(NWDGitLabMilestoneModel), tGitLabMilestoneModelList);
            return View("/Views/NWDGitLabReport/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Index(NWDGitLabMilestoneModel sGitLabMilestone)
        {
            NWDProjectGitConnection? tModel = NWDWebGitLabReportConfiguration.KConfig.GetBySecretToken(sGitLabMilestone.SecretToken);
            if (tModel != null)
            {
                ViewData.Add("Title", tModel.Name);

                if (sGitLabMilestone.IssueId != 0 && string.IsNullOrEmpty(sGitLabMilestone.LabelToAdd) == false)
                {
                    // add label to Issue
                    //NWDLogger.WriteLine("sGitLabMilestone.IssueId = " + sGitLabMilestone.IssueId + " LabelToAdd " + sGitLabMilestone.LabelToAdd);
                    GitLabClient tGitlabClient = new GitLabClient(tModel.GitUrl, tModel.GitToken);
                    var tIssue = await tGitlabClient.Issues.GetAsync(tModel.GitProject, sGitLabMilestone.IssueId);
                    if (tIssue != null)
                    {
                        if (tIssue.Labels.Contains(sGitLabMilestone.LabelToAdd) == false)
                        {
                            tIssue.Labels.Add(sGitLabMilestone.LabelToAdd);
                            UpdateIssueRequest tRequest = new UpdateIssueRequest();
                            tRequest.Labels = tIssue.Labels;
                            await tGitlabClient.Issues.UpdateAsync(tModel.GitProject, sGitLabMilestone.IssueId, tRequest);
                        }
                    }
                }
                if (sGitLabMilestone.IssueId != 0 && string.IsNullOrEmpty(sGitLabMilestone.LabelToRemove) == false)
                {
                    // remove label to Issue
                    GitLabClient tGitlabClient = new GitLabClient(tModel.GitUrl, tModel.GitToken);
                    var tIssue = await tGitlabClient.Issues.GetAsync(tModel.GitProject, sGitLabMilestone.IssueId);
                    if (tIssue != null)
                    {
                        if (tIssue.Labels.Contains(sGitLabMilestone.LabelToRemove) == true)
                        {
                            tIssue.Labels.Remove(sGitLabMilestone.LabelToRemove);
                            UpdateIssueRequest tRequest = new UpdateIssueRequest();
                            tRequest.Labels = tIssue.Labels;
                            await tGitlabClient.Issues.UpdateAsync(tModel.GitProject, sGitLabMilestone.IssueId, tRequest);
                        }
                    }
                }
                if (string.IsNullOrEmpty(sGitLabMilestone.Name) == false)
                {
                    sGitLabMilestone.GitLabProject = tModel;
                    await sGitLabMilestone.Prepare(sGitLabMilestone.SecretToken, null, null);
                    List<NWDGitLabMilestoneModel> tGitLabMilestoneModelList = new List<NWDGitLabMilestoneModel>();
                    tGitLabMilestoneModelList.Add(sGitLabMilestone);
                    ViewData.Add(nameof(NWDGitLabMilestoneModel), tGitLabMilestoneModelList);
                }
                else
                {
                    NWDGitLabMilestonesListModel tGitLabMilestoneList = await NWDGitLabMilestonesListModel.Prepare(sGitLabMilestone.SecretToken,HttpContext);
                    ViewData.Add(nameof(NWDGitLabMilestonesListModel), tGitLabMilestoneList);
                }
            }
            return View("/Views/NWDGitLabReport/Index.cshtml");
        }

        // public IActionResult Indexd()
        // {
        //     List<string> tList = new List<string>();
        //     string tError = tList[tList.Count];
        //     return View("/Views/NWDGitLabReport/Index.cshtml");
        // }

        //[HttpPost]
        //public async Task<IActionResult> Commit(NWDGitLabCommitModel sGitLabCommit)
        //{
        //    //NWDLogger.WriteLine("sGitLabCommit Hash : " + sGitLabCommit.Hash);
        //    GitLabProjectModel tModel = GitLabProjectModel.GetBySecretToken(sGitLabCommit.LocalTokenReport);
        //    if (tModel != null)
        //    {
        //        NWDProject tProject = NWDAppProjectExtension.GetByReference(tModel.Project);
        //        ViewData.Add(nameof(NWDProject), tProject);
        //        //ViewData.Add(nameof(GitLabProjectModel), tModel);
        //        ViewData.Add("Title", tModel.Name);
        //        try
        //        {
        //            GitLabClient tGitlabClient = new GitLabClient(tModel.GitUrl, tModel.GitToken);

        //            NWDGitLabMilestonesListModel tGitLabMilestoneList = await NWDGitLabMilestonesListModel.Prepare(tModel.LocalTokenReport);
        //            NWDGitLabMilestoneModel sGitLabMilestone = null;
        //            GitLabApiClient.Models.Commits.Responses.Commit tCommit = await tGitlabClient.Commits.GetAsync(tModel.GitProject, sGitLabCommit.Hash);
        //            //NWDLogger.WriteLine("tCommit.CommittedDate : " + tCommit.CommittedDate.ToLongDateString());

        //            foreach (NWDGitLabMilestoneModel sMilestone in tGitLabMilestoneList.MilestonesList)
        //            {
        //                //NWDLogger.WriteLine("sMilestone : " + sMilestone.Start.ToLongDateString() + " -> " + sMilestone.Due.ToLongDateString());
        //                if (sMilestone.Start.Date <= tCommit.CommittedDate.Date && sMilestone.Due.Date >= tCommit.CommittedDate.Date)
        //                {
        //                    sGitLabMilestone = sMilestone;
        //                    break;
        //                }
        //            }
        //            if (sGitLabMilestone != null)
        //            {
        //                sGitLabMilestone.GitLabProject = tModel;
        //                await sGitLabMilestone.Prepare(tModel.LocalTokenReport, tCommit, sGitLabCommit);
        //                List<NWDGitLabMilestoneModel> tGitLabMilestoneModelList = new List<NWDGitLabMilestoneModel>();
        //                tGitLabMilestoneModelList.Add(sGitLabMilestone);
        //                ViewData.Add(nameof(NWDGitLabMilestoneModel), tGitLabMilestoneModelList);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            NWDLogger.WriteLine(e);
        //        }
        //    }
        //    return View(ViewsConstants.ViewsPath<NWDGitLabReportController>("Index"));
        //}

    }
}
