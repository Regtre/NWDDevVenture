using System;
using System.Collections.Generic;

namespace NWDFoundation.Tools
{
    public static class NWDRandomList
    {
        #region static constants

        static readonly Random KRandom = new Random();

        #endregion

        #region static methods
        public static void Shuffle<T>(this IList<T> sList)  
        {  
            int tN = sList.Count;  
            while (tN > 1) 
            {  
                tN--;  
                int tK = KRandom.Next(tN + 1);  
                T tValue = sList[tK];  
                sList[tK] = sList[tN];  
                sList[tN] = tValue;  
            }  
        }

        #endregion
    }
}