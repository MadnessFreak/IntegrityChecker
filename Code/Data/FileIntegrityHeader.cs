using System.Runtime.InteropServices;

namespace IntegrityChecker.Data
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct FileIntegrityHeader
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5)]
        public string Tag;
        [FieldOffset(5)]
        public byte Padding1;
        [FieldOffset(6)]
        public byte Padding2;
        [FieldOffset(7)]
        public byte Padding3;
        [FieldOffset(8)]
        public float Version;
        [FieldOffset(12)]
        public int EntryCount;
    }
}
