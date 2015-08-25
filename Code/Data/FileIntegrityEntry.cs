using System.Runtime.InteropServices;

namespace IntegrityChecker.Data
{
    [StructLayout(LayoutKind.Auto)]
    public struct FileIntegrityEntry
    {
        public short FileNameLen;
        public string FileName;
        public long LastModifiedTime;
        public int Size;
        public int Checksum;
    }
}
