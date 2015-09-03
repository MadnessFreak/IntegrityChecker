using System.Collections.Generic;
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

    public class FileIntegrityEntryComparer : IEqualityComparer<FileIntegrityEntry>
    {
        public bool Equals(FileIntegrityEntry x, FileIntegrityEntry y)
        {
            return x.FileName == y.FileName;
        }

        public int GetHashCode(FileIntegrityEntry obj)
        {
            return obj.FileName.GetHashCode() + obj.Checksum.GetHashCode();
        }
    }
}
