using System;
using System.IO;
using System.Runtime.InteropServices;

namespace IntegrityChecker.Data
{
    public static class ResourceSystem
    {
        /// <summary>
        /// Marshals a structure to an <see cref="System.Byte"/>-Array.
        /// </summary>
        /// <typeparam name="T">The <see cref="System.Type"/> of the structure.</typeparam>
        /// <param name="structure">The structure.</param>
        /// <returns>An <see cref="System.Byte"/>-Array of the structure.</returns>
        public static byte[] MarshalStructure<T>(object structure)
        {
            var structSize = Marshal.SizeOf(typeof(T));
            var buffer = new byte[structSize];
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            Marshal.StructureToPtr(structure, handle.AddrOfPinnedObject(), false);
            handle.Free();

            return buffer;
        }

        /// <summary>
        /// Writes a value to the current stream.
        /// </summary>
        /// <typeparam name="T">The <see cref="System.Type"/> of the structure.</typeparam>
        /// <param name="writer">The binary writer.</param>
        /// <param name="structure">The structure.</param>
        public static void MarshalAndWriteStructure<T>(this BinaryWriter writer, object structure)
        {
            if (writer != null && writer.BaseStream.CanWrite)
            {
                writer.Write(MarshalStructure<T>(structure));
            }
        }
    }
}
