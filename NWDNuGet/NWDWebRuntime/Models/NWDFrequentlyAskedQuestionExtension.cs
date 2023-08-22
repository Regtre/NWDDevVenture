using NWDFoundation.Models;
using NWDFoundation.Models.Enums;
using NWDWebEditor.Managers;

namespace NWDWebRuntime.Models
{
    public static class NWDFrequentlyAskedQuestionExtension
    {
        static NWDFrequentlyAskedQuestionExtension()
        {
            // NewQuestion(
            //     "NWDAccount",
            //     "SignInForm",
            //     "Mais que s'est-il passé pour que le site soit dégradé? ",
            //     "Notre fournisseur de serveur n'a pas entretenu correctement le matériel, ni les systèmes de secours. Et le matériel est tombé en panne! Heureusement nous avions des sauvegardes régulières sur un autre site. Nous sommes en train d'étudier les récours juridiques car depuis l'incident aucune action n'est mise en place par le prestataire. Par ailleurs nos demandes de résiliation restent lettres mortes.",
            //     NWDNavigatorFlag.Safari, NWDNavigatorOSFlag.MacOS);
            // NewQuestion(
            //     "NWDAccount",
            //     "SignUpForm",
            //     "Quand la boutique sera-t-elle disponible? ", "Dès que possible, nous y travaillons!",
            //     NWDNavigatorFlag.All, NWDNavigatorOSFlag.All);
            // NewQuestion(
            //     "NWDAccount",
            //     "SignUpForm","Mon compte est-il perdu?",
            //     "A priori, non! Des sauvegardes étaient faites régulierement.", NWDNavigatorFlag.All,
            //     NWDNavigatorOSFlag.All);
            // NewQuestion(
            //     "NWDAccount",
            //     "SignInForm","Quand est-il de mon abonnement?",
            //     "Pour éviter tout désagrément nous avons mis le site en mode secours et vous pouvez donc bénéficier de votre abonnement. Nous prolongerons les abonnements du temps de la réparation de la boutique",
            //     NWDNavigatorFlag.All, NWDNavigatorOSFlag.All);
            // NewQuestion(
            //     "NWDAccount",
            //     "SignInForm",
            //     "Pourquoi le site a changé de design?",
            //     "Vous avez un aperçu du nouveau site. C'est le nouveau design.", NWDNavigatorFlag.All,
            //     NWDNavigatorOSFlag.All);
            // NewQuestion(
            //     "NWDAccount",
            //     "SignInForm",
            //     "Pourquoi le site a changé de design?",
            //     "Nous avons recodé entièrement le site. C'est le nouveau design.", NWDNavigatorFlag.All,
            //     NWDNavigatorOSFlag.All);
            // NewQuestion(
            //     "NWDAccount",
            //     "SignInForm","Je n'arrive plus à m'authentifier!",
            //     "Vous devez maintenant vous authentifier avec votre email et votre mot-de-passe. Si vous aviez un mot-de-passe trop faible il faudra demander un nouveau mot-de-passe!",
            //     NWDNavigatorFlag.All, NWDNavigatorOSFlag.All);
            // NewQuestion(
            //     "NWDAccount",
            //     "SignInForm","Qu'en est-il de mon abonnement?",
            //     "Il a été automatiquement reporté de 6 mois, quelque soit sa durée initiale!", NWDNavigatorFlag.All,
            //     NWDNavigatorOSFlag.All);
            // NewQuestion(
            //     "NWDAccount",
            //     "SignInForm",
            //     "Pourquoi l'analyse de texte a disparu?",
            //     "Nous nous efforçons de finir les développements pour rendre de nouveau cet outil fonctionnel. Nous prenons aussi en compte les demandes des professeurs de latin.",
            //     NWDNavigatorFlag.All, NWDNavigatorOSFlag.All);
        }

        static public Dictionary<string,Dictionary<string, NWDFrequentlyAskedQuestionsList>> K_ListOfQuestion =
            new Dictionary<string,Dictionary<string, NWDFrequentlyAskedQuestionsList>>();


        public static NWDFrequentlyAskedQuestionsList Find(string sController, string sAction)
        {
            NWDFrequentlyAskedQuestionsList rReturn = new NWDFrequentlyAskedQuestionsList();
            rReturn.Domain = sController;
            rReturn.SubDomain = sAction;
            rReturn.ShowIfEmpty = false;
            rReturn.ListOfQuestion.AddRange(NWDWebDBDataManager.GetBy<NWDFrequentlyAskedQuestion>(new Dictionary<string, string>(){{nameof(NWDFrequentlyAskedQuestion.Domain), sController},{nameof(NWDFrequentlyAskedQuestion.SubDomain), sAction}}));
            rReturn.ListOfQuestion.AddRange(NWDWebDBDataManager.GetBy<NWDFrequentlyAskedQuestion>(new Dictionary<string, string>(){{nameof(NWDFrequentlyAskedQuestion.Domain), sController},{nameof(NWDFrequentlyAskedQuestion.SubDomain), ""}}));
            return rReturn;
        }

        public static NWDFrequentlyAskedQuestionsList GetList(string[] sControllersArray, string sAction, bool sShowIfEmpty = true)
        {
            NWDFrequentlyAskedQuestionsList rReturn = new NWDFrequentlyAskedQuestionsList();
            rReturn.SubDomain = sAction;
            List<string> tDomains = new List<string>();
            rReturn.ShowIfEmpty = sShowIfEmpty;
            if (sControllersArray != null && sControllersArray.Length>0)
            {
                rReturn.Domain = sControllersArray[0];
                foreach (string tController in sControllersArray)
                {
                    if (K_ListOfQuestion.ContainsKey(tController))
                    {
                        if (K_ListOfQuestion[tController].ContainsKey(sAction))
                        {
                            rReturn.ListOfQuestion.AddRange(K_ListOfQuestion[tController][sAction].ListOfQuestion);
                        }
                    }
                }
            }
            return rReturn;
        }

        // public static NWDFrequentlyAskedQuestionsList GetList(string sDomain = "", bool sShowIfEmpty = false)
        // {
        //     return GetList(new string[] {sDomain}, null,  sShowIfEmpty);
        // }
        //
        // public static NWDFrequentlyAskedQuestionsList GetList(NWDFrequentlyAskedQuestionDomains sDomain, bool sShowIfEmpty = true)
        // {
        //     return GetList(null,new NWDFrequentlyAskedQuestionDomains[] {sDomain},  sShowIfEmpty);
        // }

        // public static void AddThisQuestion(this NWDFrequentlyAskedQuestion sObject)
        // {
        //     if (K_ListOfQuestion.ContainsKey(sObject.Domain) == false)
        //     {
        //         K_ListOfQuestion.Add(sObject.Domain, new Dictionary<string, NWDFrequentlyAskedQuestionsList>());
        //     }
        //     if (K_ListOfQuestion[sObject.Domain].ContainsKey(sObject.SubDomain) == false)
        //     {
        //         K_ListOfQuestion[sObject.Domain].Add(sObject.SubDomain, new NWDFrequentlyAskedQuestionsList());
        //         K_ListOfQuestion[sObject.Domain][sObject.SubDomain].Domain = sObject.Domain;
        //         K_ListOfQuestion[sObject.Domain][sObject.SubDomain].SubDomain = sObject.SubDomain;
        //     }
        //     K_ListOfQuestion[sObject.Domain][sObject.SubDomain].ListOfQuestion.Add(sObject);
        // }

        // public static NWDFrequentlyAskedQuestion NewQuestion(string sDomain, string sSubDomain, string sQuestion, string sAnswer, NWDNavigatorFlag sNwdNavigatorFlag, NWDNavigatorOSFlag sSystem)
        // {
        //     NWDFrequentlyAskedQuestion rReturn = new NWDFrequentlyAskedQuestion();
        //     rReturn.Domain = sDomain;
        //     rReturn.SubDomain = sSubDomain;
        //     rReturn.Question = sQuestion;
        //     rReturn.Answer = sAnswer;
        //     rReturn.AppConcerned = sNwdNavigatorFlag;
        //     rReturn.SystemConcerned = sSystem;
        //     rReturn.AddThisQuestion();
        //     return rReturn;
        // }
    }
}