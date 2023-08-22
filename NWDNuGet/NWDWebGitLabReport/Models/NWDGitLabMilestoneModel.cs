using System.Drawing;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using GitLabApiClient;
using GitLabApiClient.Models.Commits.Responses;
using GitLabApiClient.Models.Issues.Responses;
using GitLabApiClient.Models.Milestones.Responses;
using GitLabApiClient.Models.Projects.Responses;
using NWDFoundation.Logger;
using NWDFoundation.Tools;
using NWDWebGitLabReport.Configuration;

namespace NWDWebGitLabReport.Models
{
    [Serializable]
    public class NWDGitLabMilestoneModel
    {
        public string Name { set; get; } = string.Empty;
        public string SecretToken { set; get; } = NWDRandom.RandomStringAlpha(64);

        [NonSerialized] public string RandomKey = NWDRandom.RandomStringAlpha(32);

        public int IssueId { set; get; } = 0;
        public string LabelToAdd { set; get; } = string.Empty;
        public string LabelToRemove { set; get; } = string.Empty;

        public Milestone? MyMilestone { set; get; }
        public List<Issue>? IssuesList { set; get; }
        public Commit? StopAtCommit { set; get; }
        public float TaskScoreTotal { set; get; } = 0;
        public int AvailableDays { set; get; } = 0;
        public List<DateTime>? Days { set; get; }
        public List<int>? DaysInt { set; get; }
        public List<string>? Pokers { set; get; }
        public DateTime Start { set; get; }
        public DateTime Due { set; get; }
        public Chart? BurndownGraph { set; get; }
        public Chart? BurnupGraph { set; get; }

        public Chart? TasksGraph { set; get; }

        public Chart? TasksStateGraph { set; get; }

        //public MyChart myChart { set; get; }
        public Dictionary<string, Label>? LabelsDico { set; get; }
        public Dictionary<string, string>? LabelsDicoColor { set; get; }

        public NWDProjectGitConnection? GitLabProject { set; get; }

        public async Task Prepare(string sSecretToken, GitLabApiClient.Models.Commits.Responses.Commit? tCommit,
            NWDGitLabCommitModel? sGitLabCommit)
        {
            NWDProjectGitConnection? tModel = NWDWebGitLabReportConfiguration.KConfig.GetBySecretToken(sSecretToken);
            if (tModel != null)
            {
                // NWDLogger.WriteLine("I FOUND GitLabProjectModel with sSecretToken " + sSecretToken);
                GitLabClient tGitlabClient = new GitLabClient(tModel.GitUrl, tModel.GitToken);

                LabelsDico = new Dictionary<string, GitLabApiClient.Models.Projects.Responses.Label>();
                LabelsDicoColor = new Dictionary<string, string>();
                IList<Label>? labels = await tGitlabClient.Projects.GetLabelsAsync(tModel.GitProject);

                foreach (Label tLabel in labels)
                {
                    //NWDLogger.WriteLine("Label : " + tLabel.Id + ", " + tLabel.Priority + ", " + tLabel.Name + ", " + tLabel.Description + ", " + tLabel.Color.ToString());
                    LabelsDico.Add(tLabel.Name, tLabel);
                    LabelsDicoColor.Add(tLabel.Name, tLabel.Color);
                }


                Milestone? tMilestone = await GetMilestoneByName(tGitlabClient, tModel);


                if (tMilestone != null)
                {
                    Pokers = new List<string>();
                    MyMilestone = tMilestone;
                    IssuesList = new List<Issue>();


                    IList<Issue>? tIssueMilles = await tGitlabClient.Issues.GetAllAsync(tModel.GitProject,
                        options: o =>
                        {
                            o.MilestoneTitle = tMilestone.Title;
                            o.State = IssueState.All;
                        });
                    tIssueMilles = tIssueMilles.OrderBy(pet => pet.ClosedAt).ToList();
                    if (string.IsNullOrEmpty(MyMilestone.StartDate) == false)
                    {
                        Start = DateTime.Parse(MyMilestone.StartDate);
                    }

                    if (tCommit != null)
                    {
                        Name = MyMilestone.Title + " - Commit " + tCommit.ShortId + " ";
                        StopAtCommit = tCommit;
                        Due = tCommit.CommittedDate;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(MyMilestone.DueDate) == false)
                        {
                            Due = DateTime.Parse(MyMilestone.DueDate);
                        }
                    }

                    foreach (Issue tIssue in tIssueMilles)
                    {
                        //NWDLogger.WriteLine(tMilestone.Title + " -> Issue : #" + tIssue.Iid + " " + tIssue.Title);
                        bool tToUse = false;
                        if (tCommit != null)
                        {
                            DateTime? tIssueClosedAt = tIssue.ClosedAt ?? null;
                            if (tIssue.ClosedAt != null)
                            {
                                tIssueClosedAt = (DateTime)tIssue.ClosedAt;
                                if (tIssueClosedAt <= Due)
                                {
                                    if (sGitLabCommit != null)
                                    {
                                        if (string.IsNullOrEmpty(sGitLabCommit.Label) == false)
                                        {
                                            if (tIssue.Labels.Contains(sGitLabCommit.Label))
                                            {
                                                tToUse = true;
                                            }
                                        }
                                        else
                                        {
                                            tToUse = true;
                                        }
                                    }
                                    else
                                    {
                                        tToUse = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            tToUse = true;
                        }

                        if (tToUse)
                        {
                            IssuesList.Add(tIssue);
                            float tTaskScore = GetIssueValue(tIssue);
                            Pokers.Add("" + tTaskScore);
                            TaskScoreTotal += tTaskScore;
                        }
                    }

                    IssuesList = IssuesList.OrderBy(pet => pet.ClosedAt).ToList();
                    PrepareChart();
                    if (tModel.ShowBurnDownChart)
                    {
                        GenerateBurndownChart();
                    }

                    if (tModel.ShowBurnUpChart)
                    {
                        GenerateBurnUpChart();
                    }

                    if (tModel.ShowTaskChart)
                    {
                        GenerateTaskChart();
                    }

                    if (tModel.ShowTaskStateChart)
                    {
                        GenerateTaskStateChart();
                    }
                }
            }
            else
            {
                NWDLogger.Warning("I DIDN'T FOUND GitLabProjectModel with sSecretToken  '" + sSecretToken + "'");
            }
        }

        private async Task<Milestone?> GetMilestoneByName(GitLabClient sGitlabClient, NWDProjectGitConnection sModel)
        {
            IList<Milestone>? tMilestones = await sGitlabClient.Projects.GetMilestonesAsync(sModel.GitProject);
            Milestone? tMilestone = null;
            foreach (Milestone tM in tMilestones)
            {
                if (Name == tM.Title)
                {
                    tMilestone = tM;
                }
            }

            return tMilestone;
        }

        private enum _Color : int
        {
            red = 0,
            blue = 1,
            yellow = 2,
            green = 3,
            purple = 4,

            orange = 5,
            grey = 6,
        }

        private static List<ChartColor> GetChartColor(_Color sIndex)
        {
            // red is default
            ChartColor rReturn = ChartColor.FromRgba(255, 99, 132, 0.2);
            switch (sIndex)
            {
                case _Color.blue:
                    rReturn = ChartColor.FromRgba(54, 162, 235, 0.2);
                    break;
                case _Color.yellow:
                    rReturn = ChartColor.FromRgba(255, 206, 86, 0.2);
                    break;
                case _Color.green:
                    rReturn = ChartColor.FromRgba(75, 192, 192, 0.2);
                    break;
                case _Color.purple:
                    rReturn = ChartColor.FromRgba(153, 102, 255, 0.2);
                    break;
                case _Color.orange:
                    rReturn = ChartColor.FromRgba(255, 159, 64, 0.2);
                    break;
                case _Color.grey:
                    rReturn = ChartColor.FromRgba(128, 128, 128, 0.2);
                    break;
            }

            return new List<ChartColor>() { rReturn };
        }

        private static List<ChartColor> GetBorderColor(_Color sIndex)
        {
            // red is default
            ChartColor rReturn = ChartColor.FromRgb(255, 99, 132);
            switch (sIndex)
            {
                case _Color.blue:
                    rReturn = ChartColor.FromRgb(54, 162, 235);
                    break;
                case _Color.yellow:
                    rReturn = ChartColor.FromRgb(255, 206, 86);
                    break;
                case _Color.green:
                    rReturn = ChartColor.FromRgb(75, 192, 192);
                    break;
                case _Color.purple:
                    rReturn = ChartColor.FromRgb(153, 102, 255);
                    break;
                case _Color.orange:
                    rReturn = ChartColor.FromRgb(255, 159, 64);
                    break;
                case _Color.grey:
                    rReturn = ChartColor.FromRgb(128, 128, 128);
                    break;
            }

            return new List<ChartColor>() { rReturn };
        }

        public static float GetIssueValue(Issue sIssue)
        {
            float rScore = 1; //  value by default
            foreach (string tL in sIssue.Labels)
            {
                if (tL == "🃏0" || tL == "0")
                {
                    rScore += 0.0F;
                }

                if (tL == "🃏1/2" || tL == "1/2" || tL == "🃏0.5" || tL == "0.5")
                {
                    rScore += 0.5F;
                }

                if (tL == "🃏1" || tL == "1")
                {
                    rScore += 1F;
                }

                if (tL == "🃏2" || tL == "2")
                {
                    rScore += 2F;
                }

                if (tL == "🃏3" || tL == "3")
                {
                    rScore += 3F;
                }

                if (tL == "🃏5" || tL == "5")
                {
                    rScore += 5F;
                }

                if (tL == "🃏8" || tL == "8")
                {
                    rScore += 8F;
                }

                if (tL == "🃏13" || tL == "13")
                {
                    rScore += 13F;
                }

                if (tL == "🃏20" || tL == "20")
                {
                    rScore += 20F;
                }
            }

            return rScore;
        }


        public void PrepareChart()
        {
            Days = new List<DateTime>();
            DaysInt = new List<int>();
            TimeSpan tDelt = Due.Subtract(Start);
            int tDayToexams = tDelt.Days;
            for (int i = 0; i <= tDayToexams; i++)
            {
                DateTime tThisDay = Start.AddDays(i);
                DayOfWeek tD = tThisDay.DayOfWeek;
                if (tD != DayOfWeek.Saturday && tD != DayOfWeek.Sunday)
                {
                    DaysInt.Add(i);
                    Days.Add(tThisDay);
                }
            }

            AvailableDays = (DaysInt.Count - 1);
        }

        public void GenerateBurndownChart()
        {
            Chart chart = InitChart();

            List<double?> tDataLinear = new List<double?>();
            List<double?> tDataTaskScoreLeft = new List<double?>();
            List<double?> tDataC = new List<double?>();

            float tTaskScoreLeft = TaskScoreTotal;

            if (DaysInt != null)
            {
                float tScoreStep = TaskScoreTotal / (DaysInt.Count - 1);
                for (int i = 0; i < DaysInt.Count; i++)
                {
                    DateTime tDateTime = Start.AddDays(DaysInt[i]);
                    DateTime tDateTimeNow = DateTime.Now;
                    float tStep = TaskScoreTotal - (tScoreStep * i);
                    tStep = MathF.Max(tStep, 0);
                    tDataLinear.Add(tStep);
                    if (tDateTime <= tDateTimeNow)
                    {
                        if (IssuesList != null)
                            foreach (Issue tIssue in IssuesList)
                            {
                                DateTime? tIssueClosedAt = tIssue.ClosedAt ?? null;
                                if (tIssue.ClosedAt != null)
                                {
                                    tIssueClosedAt = (DateTime)tIssue.ClosedAt;
                                }

                                if (tIssueClosedAt != null)
                                {
                                    if (tIssueClosedAt.Value.Date == tDateTime.Date)
                                    {
                                        tTaskScoreLeft -= GetIssueValue(tIssue);
                                    }
                                }
                            }

                        tDataTaskScoreLeft.Add(tTaskScoreLeft);
                    }
                }
            }

            int tC = tDataTaskScoreLeft.Count;
            if (tC > 0)
            {
                double tA = TaskScoreTotal;
                double? tB = tDataTaskScoreLeft[^1];
                double? P = (tB - tA) / tC;

                if (DaysInt != null)
                    for (int i = 0; i < DaysInt.Count; i++)
                    {
                        double? tStep = TaskScoreTotal + P * i;
                        tDataC.Add(tStep);
                    }

                chart.Data.Datasets.Add(InitLineDataset(tDataTaskScoreLeft, "Done", _Color.blue));
                chart.Data.Datasets.Add(InitLineDataset(tDataLinear, "Required Burn", _Color.grey));
                chart.Data.Datasets.Add(InitLineDataset(tDataC, "Forecast Burn", _Color.yellow, true));
            }

            SetPadding(chart);
            SetTitle(chart, "BurnDown Chart");
            BurndownGraph = chart;
        }

        private static void SetPadding(Chart chart, int tPaddingLeft = 10, int tPaddingRight = 12)
        {
            chart.Options = new Options
            {
                Scales = new Dictionary<string, Scale>(),
                Layout = new Layout
                {
                    Padding = new Padding
                    {
                        PaddingObject = new PaddingObject
                        {
                            Left = tPaddingLeft,
                            Right = tPaddingRight
                        }
                    }
                }
            };
        }

        public void GenerateBurnUpChart()
        {
            Chart tChart = InitChart();

            IList<double?> tDataRequiredBurn = new List<double?>();
            IList<double?> tDataTaskScoreLeft = new List<double?>();
            IList<double?> tDataForecastBurn = new List<double?>();

            float tTaskScoreLeft = 0;
            if (DaysInt != null)
            {
                float tScoreStep = TaskScoreTotal / (DaysInt.Count - 1);
                for (int i = 0; i < DaysInt.Count; i++)
                {
                    DateTime tDateTime = Start.AddDays(DaysInt[i]);
                    DateTime tDateTimeNow = DateTime.Now;

                    // --------- tDataRequiredBurn ------------
                    float tStep = (tScoreStep * i);
                    tStep = MathF.Max(tStep, 0);
                    tDataRequiredBurn.Add(tStep);
                    // --------- tDataRequiredBurn ------------

                    // --------- tDataTaskScoreLeft ------------
                    if (tDateTime <= tDateTimeNow)
                    {
                        if (IssuesList != null)
                            foreach (Issue tIssue in IssuesList)
                            {
                                DateTime? tIssueClosedAt = tIssue.ClosedAt ?? null;
                                if (tIssue.ClosedAt != null)
                                {
                                    tIssueClosedAt = (DateTime)tIssue.ClosedAt;
                                }

                                if (tIssueClosedAt == null) continue;
                                if (tIssueClosedAt.Value.Date == tDateTime.Date)
                                    tTaskScoreLeft += GetIssueValue(tIssue);
                            }

                        tDataTaskScoreLeft.Add(tTaskScoreLeft);
                    }
                    // --------- tDataTaskScoreLeft ------------
                }
            }

            int tDaysPassed = tDataTaskScoreLeft.Count;
            if (tDaysPassed > 0)
            {
                // --------- tDataForecastBurn ------------
                {
                    double? tB = tDataTaskScoreLeft[^1];
                    double? tP = (tB) / tDaysPassed;
                    if (DaysInt != null)
                        for (int tI = 0; tI < DaysInt.Count; tI++)
                        {
                            tDataForecastBurn.Add(tP * tI);
                        }
                }

                // --------- tDataForecastBurn ------------


                tChart.Data.Datasets.Add(InitLineDataset(tDataTaskScoreLeft, "Done", _Color.blue));
                tChart.Data.Datasets.Add(InitLineDataset(tDataRequiredBurn, "Required Burn", _Color.grey));
                tChart.Data.Datasets.Add(InitLineDataset(tDataForecastBurn, "Forecast Burn", _Color.yellow, true));
            }

            SetPadding(tChart);
            SetTitle(tChart, "BurnUp Chart");
            BurnupGraph = tChart;
        }

        private Chart InitChart()
        {
            Chart tChart = new Chart
            {
                Type = Enums.ChartType.Line,
                Data = new Data()
                {
                    Labels = new List<string>(),
                    Datasets = new List<Dataset>()
                }
            };
            if (tChart == null) throw new ArgumentNullException(nameof(tChart));
            if (Days != null)
                foreach (DateTime tDay in Days)
                {
                    tChart.Data.Labels.Add(tDay.ToString("yyyy/MM/dd"));
                }

            return tChart;
        }


        private static LineDataset InitLineDataset(IList<double?> sDataRequiredBurn, string sLabel, _Color sColor,
            bool sDashedLine = false)
        {
            LineDataset tLineDataset = new LineDataset()
            {
                Label = sLabel,
                Data = sDataRequiredBurn,
                BackgroundColor = GetChartColor(sColor),
                BorderColor = GetBorderColor(sColor),
                BorderWidth = new List<int>() { 1 }
            };

            if (sDashedLine)
            {
                tLineDataset.BorderDash = new List<int>() { 10, 5 };
            }

            return tLineDataset;
        }

        private static void SetTitle(Chart sChart, string sTitle)
        {
            sChart.Options.Plugins = new Plugins
            {
                Title = new Title()
                {
                    Display = true,
                    Text = new List<string>() { sTitle },
                }
            };
        }

        public void GenerateTaskChart()
        {
            Chart tChart = new Chart
            {
                Type = Enums.ChartType.Bar
            };
            Data data = new Data
            {
                Labels = new List<string>()
            };
            if (Days != null)
                foreach (DateTime tDay in Days)
                {
                    data.Labels.Add(tDay.ToString("yyyy/MM/dd"));
                }

            data.Datasets = new List<Dataset>();

            IList<double?> tData = new List<double?>();
            IList<double?> tDataB = new List<double?>();
            IList<double?> tDataE = new List<double?>();
            IList<double?> tDataF = new List<double?>();
            IList<double?> tDataG = new List<double?>();


            //int tOpened = IssuesList.Count;
            //NWDLogger.WriteLine("task at start = " + tOpened);
            if (DaysInt != null)
                foreach (int tDays in DaysInt)
                {
                    int tNewOpen = 0;
                    int tNewClosed = 0;
                    int tOpened = 0;
                    int tClosed = 0;
                    int tTaskTotal = 0;
                    if (IssuesList != null)
                        foreach (GitLabApiClient.Models.Issues.Responses.Issue tIssue in IssuesList)
                        {
                            DateTime tDateTime = Start.AddDays(tDays);
                            DateTime? tDdd = tIssue.ClosedAt ?? null;
                            if (tIssue.CreatedAt.Date <= tDateTime.Date)
                            {
                                tTaskTotal++;
                                tOpened++;
                            }

                            if (tIssue.CreatedAt.Date == tDateTime.Date)
                            {
                                tOpened--;
                                tNewOpen++;
                            }

                            if (tDdd != null)
                            {
                                if (tDdd.Value.Date < tDateTime.Date)
                                {
                                    tOpened--;
                                    tClosed++;
                                }

                                if (tDdd.Value.Date == tDateTime.Date)
                                {
                                    tNewClosed++;
                                }
                            }
                        }

                    tData.Add(tOpened);
                    tDataB.Add(tClosed);
                    tDataE.Add(tNewOpen);
                    tDataF.Add(tNewClosed);
                    tDataG.Add(tTaskTotal);
                    //NWDLogger.WriteLine("tOpened at " + i + " = " + tOpened);
                }

            BarDataset tDataset = new BarDataset()
            {
                Label = "Tasks opened",
                Data = tData,
                BackgroundColor = GetChartColor(_Color.blue),
                BorderColor = GetBorderColor(_Color.blue),
                BorderWidth = new List<int>() { 1 }
            };
            BarDataset tDatasetC = new BarDataset()
            {
                Label = "Tasks added",
                Data = tDataE,
                BackgroundColor = GetChartColor(_Color.green),
                BorderColor = GetBorderColor(_Color.green),
                BorderWidth = new List<int>() { 1 }
            };
            BarDataset tDatasetF = new BarDataset()
            {
                Label = "Tasks closing",
                Data = tDataF,
                BackgroundColor = GetChartColor(_Color.purple),
                BorderColor = GetBorderColor(_Color.purple),
                BorderWidth = new List<int>() { 1 }
            };
            BarDataset tDatasetB = new BarDataset()
            {
                Label = "Tasks closed",
                Data = tDataB,
                BackgroundColor = GetChartColor(_Color.red),
                BorderColor = GetBorderColor(_Color.red),
                BorderWidth = new List<int>() { 1 }
            };
            BarDataset tDatasetG = new BarDataset()
            {
                Label = "Tasks at moment",
                Data = tDataG,
                BackgroundColor = GetChartColor(_Color.grey),
                BorderColor = GetBorderColor(_Color.grey),
                BorderWidth = new List<int>() { 1 }
            };

            data.Datasets.Add(tDataset);
            data.Datasets.Add(tDatasetC);
            data.Datasets.Add(tDatasetF);
            data.Datasets.Add(tDatasetB);
            data.Datasets.Add(tDatasetG);

            tChart.Data = data;
            Options tOptions = new Options
            {
                Scales = new Dictionary<string, Scale>()
            };
            tChart.Options = tOptions;
            tChart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            tChart.Options.Plugins = new Plugins
            {
                Title = new Title()
                {
                    Display = true,
                    Text = new List<string>() { "Tasks Chart", },
                }
            };
            TasksGraph = tChart;
        }

        public void GenerateTaskStateChart()
        {
            Chart tChart = new Chart
            {
                Type = Enums.ChartType.Bar
            };
            Data tChartData = new Data
            {
                Labels = new List<string>()
            };
            if (Days != null)
                foreach (DateTime tDay in Days)
                {
                    tChartData.Labels.Add(tDay.ToString("yyyy/MM/dd"));
                }

            tChartData.Datasets = new List<Dataset>();

            IList<double?> tData = new List<double?>();
            IList<double?> tDataB = new List<double?>();
            IList<double?> tDataE = new List<double?>();

            if (DaysInt != null)
                foreach (int tDays in DaysInt)
                {
                    int tNewOpen = 0;
                    int tOpened = 0;
                    int tClosed = 0;
                    if (IssuesList != null)
                        foreach (Issue tIssue in IssuesList)
                        {
                            DateTime tDateTime = Start.AddDays(tDays);
                            DateTime? tDdd = tIssue.ClosedAt ?? null;
                            if (tIssue.CreatedAt.Date <= tDateTime.Date)
                            {
                                tOpened++;
                            }

                            if (tIssue.CreatedAt.Date == tDateTime.Date)
                            {
                                tOpened--;
                                tNewOpen++;
                            }

                            if (tDdd != null)
                            {
                                if (tDdd.Value.Date < tDateTime.Date)
                                {
                                    tOpened--;
                                    tClosed++;
                                }

                                if (tDdd.Value.Date == tDateTime.Date) { }
                            }
                        }

                    tData.Add(tOpened);
                    tDataB.Add(tClosed);
                    tDataE.Add(tNewOpen);
                }

            tChartData.Datasets.Add(new BarDataset()
            {
                Label = "Tasks opened",
                Data = tData,
                BackgroundColor = GetChartColor(_Color.blue),
                BorderColor = GetBorderColor(_Color.blue),
                BorderWidth = new List<int>() { 1 }
            });
            tChartData.Datasets.Add(new BarDataset()
            {
                Label = "Tasks added",
                Data = tDataE,
                BackgroundColor = GetChartColor(_Color.green),
                BorderColor = GetBorderColor(_Color.green),
                BorderWidth = new List<int>() { 1 }
            });
            tChartData.Datasets.Add(new BarDataset()
            {
                Label = "Tasks closed",
                Data = tDataB,
                BackgroundColor = GetChartColor(_Color.red),
                BorderColor = GetBorderColor(_Color.red),
                BorderWidth = new List<int>() { 1 }
            });

            tChart.Data = tChartData;

            tChart.Options = new Options
            {
                Plugins = new Plugins
                {
                    Title = new Title() { Display = true, Text = new List<string>() { "Tasks state", }, }
                },
                Layout = new Layout
                {
                    Padding = new Padding { PaddingObject = new PaddingObject { Left = 10, Right = 12 } }
                },
                Scales = new Dictionary<string, Scale>()
                {
                    { "x", new Scale { Stacked = true, } },
                    { "y", new Scale { Stacked = true, } },
                }
            };

            TasksStateGraph = tChart;
        }
    }
}