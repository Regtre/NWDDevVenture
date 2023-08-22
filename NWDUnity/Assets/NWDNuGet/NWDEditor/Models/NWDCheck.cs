using System;

namespace NWDEditor
{
    [Serializable]
    [Flags]
    public enum NWDCheck : int
    {
        None = 0,

        Test = 1 << 0,

        CheckTwo = 1 << 1,
        CheckThree = 1 << 2,
        CheckFour = 1 << 3,
        CheckFive = 1 << 4,
        CheckSix = 1 << 5,
        CheckSeven = 1 << 6,
        CheckHeight = 1 << 7,
        CheckNine = 1 << 8,
        CheckTen = 1 << 9,

        ToTranslate = 1 << 10,

        ToPublish = 1 << 11,
    }
}