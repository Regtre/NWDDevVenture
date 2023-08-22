using NWDFoundation.Models;
using System;

namespace NWDStandardModels.Models
{
    public class NWDTestSubModel : INWDSubModel
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

        public int[] TestIntArray { get; set; }
    }
}
