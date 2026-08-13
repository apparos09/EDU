using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace util
{
    public class FileReaderLines : FileReader
    {
        // the lines from the file.
        public string[] lines;

        // Read from the file, and put the content into lines.
        public override void ReadFile()
        {
            // checks if the file exists.
            if (!FileExists())
            {
                Debug.LogError("File does not exist.");
                return;
            }

            // Clears the lines array. Not sure if necessary.
            if(lines != null)
            {
                // Clears the lines array. This sets all entries to 'null' or an equivalent value.
                Array.Clear(lines, 0, lines.Length);
            }

            // Sets lines to null.
            lines = null;

            // Gets all the lines from the file (@ specifies where the path is relative to - may not be needed).
            string f = filePath + fileName;
            lines = File.ReadAllLines(@f);
        }
    }
}