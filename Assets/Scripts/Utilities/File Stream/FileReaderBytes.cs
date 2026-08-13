using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace util
{
    public class FileReaderBytes : FileReader
    {
        // the bytes from the file.
        public byte[] bytes;

        // Read from the file, and put the content into lines.
        public override void ReadFile()
        {
            // checks if the file exists.
            if (!FileExists())
            {
                Debug.LogError("File does not exist.");
                return;
            }

            // TODO: see if this works for clearing the array. C# garbage collection should take care of this?
            bytes = null;

            // Gets all the bytes from the file (@ specifies where the path is relative to - may not be needed).
            string f = filePath + fileName;
            bytes = File.ReadAllBytes(@f);
        }
    }
}