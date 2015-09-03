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
                writer.WriteStructure<FileIntegrityHeader>(header);

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

        public void Read(string path)
        {
            if (!File.Exists(path))
            {
                System.Windows.Forms.MessageBox.Show("File not found - " + path, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            var header = new FileIntegrityHeader();
            var list = new List<FileIntegrityEntry>();

            using (var reader = new BinaryReader(File.Open(FileIntegrity.FileName, FileMode.Open, FileAccess.Read)))
            {
                header = reader.ReadStructure<FileIntegrityHeader>();

                for (int i = 0; i < header.EntryCount; i++)
                {
                    var entry = new FileIntegrityEntry();
                    entry.FileNameLen = reader.ReadInt16();
                    entry.FileName = new string(reader.ReadChars(entry.FileNameLen));
                    entry.LastModifiedTime = reader.ReadInt64();
                    entry.Size = reader.ReadInt32();
                    entry.Checksum = reader.ReadInt32();

                    list.Add(entry);
                }
            }

            Scan(Environment.CurrentDirectory);

            foreach (var entry in Entries)
            {
                if (list.Contains(entry, new FileIntegrityEntryComparer()))
                {
                    var lentry = list.Find(e => e.FileName == entry.FileName);
                    if (lentry.LastModifiedTime != entry.LastModifiedTime)
                    {
                        System.Windows.Forms.MessageBox.Show("File time missmatch - " + entry.FileName);
                    }
                    if (lentry.Size != entry.Size)
                    {
                        System.Windows.Forms.MessageBox.Show("File size missmatch - " + entry.FileName);
                    }
                    if (lentry.Checksum != entry.Checksum)
                    {
                        System.Windows.Forms.MessageBox.Show("File checksum missmatch - " + entry.FileName);
                    }
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("File not found - " + entry.FileName);
                }
            }
        }
    }
}
