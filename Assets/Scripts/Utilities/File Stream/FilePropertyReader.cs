/*
 * References:
 * - https://stackoverflow.com/questions/7861886/how-to-get-file-properties
 * - https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo?view=net-6.0
 * - https://stackoverflow.com/questions/6505870/how-to-get-the-properties-of-a-mp3-file-in-c-sharp
 * - https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.serializationinfo.getvalue?view=net-6.0
 */
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace util
{
    // reads the properties of a file.
    public class FilePropertyReader : MonoBehaviour
    {
        // information from the file.
        private FileInfo fileInfo;

        // the audio clip being read from.
        public string fileName = "";

        // the location of the file.
        public string filePath = "";

        // Start is called before the first frame update
        void Start()
        {
            // reads the file properties.
            if (fileName != "")
                ReadFileProperties();
        }

        // reads the properties of the file.
        public bool ReadFileProperties()
        {
            // the file.
            string file = filePath + fileName;

            // grabs the information.
            fileInfo = new FileInfo(file);

            return fileInfo.Exists;
        }

        // TODO: figure out how to do this.
        // // gets a property from the file.
        // public string GetProperty(string propertyName)
        // {
        //     // https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.serializationinfo?view=net-6.0
        // }

        // Update is called once per frame
        void Update()
        {

        }
    }
}