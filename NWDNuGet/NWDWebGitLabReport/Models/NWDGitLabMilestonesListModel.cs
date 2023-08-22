using System.Drawing;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using GitLabApiClient;
using Microsoft.AspNetCore.Http;
using NWDWebGitLabReport.Configuration;
using NWDWebGitLabReport.Managers;

namespace NWDWebGitLabReport.Models
{
    [Serializable]
    public class NWDGitLabMilestonesListModel
    {
        public List<NWDGitLabMilestoneModel> MilestonesList { set; get; } = new List<NWDGitLabMilestoneModel>();

        public Chart? CapacityVelocityChart { set; get; }

        public static async Task<NWDGitLabMilestonesListModel> Prepare(string sSecretToken, HttpContext sHttpContext)
        {
            NWDGitLabMilestonesListModel tGitLabMilestoneList = new NWDGitLabMilestonesListModel();
            NWDProjectGitConnection? tModel = NWDWebGitLabReportConfiguration.KConfig.GetBySecretToken(sSecretToken);
            if (tModel != null)
            {
                tGitLabMilestoneList.MilestonesList = new List<NWDGitLabMilestoneModel>();
                GitLabClient tGitlabClient = new GitLabClient(tModel.GitUrl, tModel.GitToken);
                IList<GitLabApiClient.Models.Milestones.Responses.Milestone> tMilestones = await tGitlabClient.Projects.GetMilestonesAsync(tModel.GitProject);
                if (tMilestones != null)
                {
                    foreach (GitLabApiClient.Models.Milestones.Responses.Milestone tM in tMilestones)
                    {
                        NWDGitLabMilestoneModel tGitLabMilestoneModel = new NWDGitLabMilestoneModel
                        {
                            Name = tM.Title,
                            SecretToken = tModel.LocalTokenReport
                        };
                        tGitLabMilestoneList.MilestonesList.Add(tGitLabMilestoneModel);
                        if (string.IsNullOrEmpty(tM.StartDate) == false)
                        {
                            tGitLabMilestoneModel.Start = DateTime.Parse(tM.StartDate);
                        }
                        if (string.IsNullOrEmpty(tM.DueDate) == false)
                        {
                            tGitLabMilestoneModel.Due = DateTime.Parse(tM.DueDate);
                        }
                        if (NWDSprintInfoManager.GetSprintInfoForMilestone(tM.Title,sHttpContext) == null)
                        {
                            NWDSprintInfo tSprintInfo = new NWDSprintInfo()
                            {
                                MilestoneName = tM.Title,
                            };
                            NWDSprintInfoManager.Save(sHttpContext,tSprintInfo);
                        }
                    }
                    tGitLabMilestoneList.MilestonesList = tGitLabMilestoneList.MilestonesList.OrderBy(sM => sM.Start).ToList();
                   
                }
            }
            tGitLabMilestoneList.PrepareCapacityVelocityChart(sHttpContext);
            return tGitLabMilestoneList;
        }

        private void PrepareCapacityVelocityChart(HttpContext sHttpContext)
        {
            Chart tChart = new Chart
            {
                Type = Enums.ChartType.Line,
                Data = new Data
                {
                    Labels = new List<string>(),
                    Datasets = new List<Dataset>()
                }
            };
            LineDataset tCapacityDataset = new LineDataset()
            {
                Label = "Capacity",
                BackgroundColor = new List<ChartColor> { ChartColor.FromRgba(54, 162, 235, 0.5) } ,
                BorderColor =new List<ChartColor> { ChartColor.FromRgba(54, 162, 235, 0.5) } ,
                BorderWidth =  new List<int> { 1 },
                Fill = false.ToString(),
                Data = new List<double?>()
            };
            LineDataset tVelocityDataset = new LineDataset()
            {
                Label = "Velocity",
                BackgroundColor =new List<ChartColor> { ChartColor.FromRgba(255, 206, 86, 0.5) } ,
                BorderColor = new List<ChartColor> { ChartColor.FromRgba(255, 206, 86, 0.5) } ,
                BorderWidth =  new List<int> { 1 },
                Fill = false.ToString(),
                Data = new List<double?>()
            };
            foreach (NWDGitLabMilestoneModel tM in MilestonesList)
            {
                NWDSprintInfo? tSprintInfo = NWDSprintInfoManager.GetSprintInfoForMilestone(tM.Name,sHttpContext);
                if (tSprintInfo != null && tSprintInfo is not { Capacity: 0, Velocity: 0 })
                {
                    tChart.Data.Labels.Add(tM.Name);
                    tCapacityDataset.Data.Add(tSprintInfo.Capacity);
                    tVelocityDataset.Data.Add(tSprintInfo.Velocity);
                }
            }
            tChart.Data.Datasets.Add(tCapacityDataset);
            tChart.Data.Datasets.Add(tVelocityDataset);

            CapacityVelocityChart = tChart;
        }
    }
}
