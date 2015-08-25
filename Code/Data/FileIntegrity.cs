using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntegrityChecker.Data
{
    /// <summary>
    /// Provides a class for data integrity checking.
    /// </summary>
    public class FileIntegrity
    {
        // Properties
        /// <summary>
        /// Gets a collection of all file integrity entries.
        /// </summary>
        public List<FileIntegrityEntry> Entries { get; private set; }
        /// <summary>
        /// Gets a number of all file integrity entries.
        /// </summary>
        public int EntryCount 
        { 
            get { return Entries.Count; } 
        }

        // Constants
        public const float FileVersion = 1.0f;
        public const string FileTag = "IDF\0\0";
        public const string FileName = "FILE_INTEGRITY.BIN";

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrityChecker.Data.FileIntegrity"/> class.
        /// </summary>
        public FileIntegrity()
        {
            Entries = new List<FileIntegrityEntry>();
        }

        // Methods
        /// <summary>
        /// Creates a new file integrity.
        /// </summary>
        /// <param name="path">The specified path.</param>
        public void Create(string path)
        {
            var header = new FileIntegrityHeader()
            {
                Tag = FileIntegrity.FileTag,
                Version = FileIntegrity.FileVersion
            };

            Scan(Environment.CurrentDirectory);

            header.EntryCount = EntryCount;

            using (var writer = new BinaryWriter(File.Open(FileIntegrity.FileName, FileMode.Create, FileAccess.Write)))
            {
                writer.MarshalAndWriteStructure<FileIntegrityHeader>(header);

                foreach (var entry in Entries)
                {
                    writer.Write(entry.FileNameLen);
                    writer.Write(entry.FileName.ToCharArray(), 0, entry.FileNameLen);
                    writer.Write(entry.LastModifiedTime);
                    writer.Write(entry.Size);
                    writer.Write(entry.Checksum);
                }
            }
        }

        /// <summary>
        /// Scans the specified directory for file changes.
        /// </summary>
        /// <param name="path">The specified path of the directory.</param>
        public void Scan(string path)
        {
            foreach (var filename in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                var info = new FileInfo(filename);
                var entry = new FileIntegrityEntry()
                {
                    FileName = info.Name.Insert(0, "\\"),
                    FileNameLen = (short)(info.Name.Length + 1),
                    LastModifiedTime = info.LastWriteTime.ToFileTime(),
                    Size = (int)info.Length
                };

                Entries.Add(entry);
            }
        }
    }
}
