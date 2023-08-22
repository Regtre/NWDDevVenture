using NWDFoundation.Models;
using System;
using System.Collections.Generic;

namespace NWDStandardModels.Models
{
    public enum TestEnum : byte
    {
        None = 0,
        Value1 = 1,
        Value2 = 2,
        Value3 = 3,
        Value4 = 4
    }
    [Flags]
    public enum TestFlag : byte
    {
        None =      0b00000000,
        Value1 =    0b00000001,
        Value2 =    0b00000010,
        Value3 =    0b00000100,
        Value4 =    0b00001000,
        All =       0b11111111,
    }
    public class NWDTestStudioData : NWDStudioData
    {
        public string TestString { get; set; }
        public bool TestBool { get; set; }
        public byte TestByte { get; set; }
        public short TestValue { get; set; }
        public int TestInt { get; set; }
        public long TestLong { get; set; }
        public float TestFloat { get; set; }
        public double TestDouble { get; set; }
        public sbyte TestSByte { get; set; }
        public ushort TestUShort { get; set; }
        public uint TestUInt { get; set; }
        public TestEnum TestEnum { get; set; }
        public TestFlag TestFlag { get; set; }
        //public NWDTestSubModel TestSubModel { get; set; }

        public int[] TestIntArray { get; set; }
        //public NWDTestSubModel[] TestSubModelArray { get; set; }
        public List<int> TestIntList { get; set; }

        public NWDReference<NWDTestStudioData> TestStudioData { get; set; }
        public NWDReferences<NWDTestStudioData> TestStudioDataList { get; set; }

        public Dictionary<ulong, float> TestDictionary { get; set; }

        public NWDReferencesAmount<NWDTestStudioData> TestReferenceAmount { get; set; }
        public NWDReferencesQuantity<NWDTestStudioData> TestReferenceQuantity { get; set; }

        public NWDLocalizableText TestLocalizableText { get; set; }
        public NWDLocalizable<NWDAsset<INWDSpriteAsset>> TestLocalizableSprite { get; set; }

        public NWDAsset<INWDSpriteAsset> TestSprite { get; set; }
        public NWDAsset<INWDTextureAsset> TestTexture { get; set; }

        public NWDDateTime TestDateTime { get; set; }
        public NWDVector2 TestVector2 { get; set; }
        public NWDVersion TestVersion { get; set; }
        public NWDColor TestColor { get; set; }
    }
}
