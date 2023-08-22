using NWDFoundation.Models;

namespace NWDWebRuntime.Models
{
    public class NWDFrequentlyAskedQuestionsList
    {
        public bool ShowIfEmpty = true;
        public string Domain = "unknown";
        public string SubDomain = "unknown";
        public List<NWDFrequentlyAskedQuestion> ListOfQuestion = new List<NWDFrequentlyAskedQuestion>();
        public void AddFromFind(string sController, string sAction)
        {
            if (NWDFrequentlyAskedQuestionExtension.K_ListOfQuestion.ContainsKey(sController))
            {
                if (NWDFrequentlyAskedQuestionExtension.K_ListOfQuestion[sController].ContainsKey(sAction))
                {
                    ListOfQuestion.AddRange(NWDFrequentlyAskedQuestionExtension.K_ListOfQuestion[sController][sAction].ListOfQuestion);
                }
            }
        }
    }

}