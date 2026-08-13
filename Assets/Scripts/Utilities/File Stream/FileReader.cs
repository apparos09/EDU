/*
 * References:
 * - https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/file-system/how-to-read-from-a-text-file
 * - https://support.unity.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
 * - https://docs.microsoft.com/en-us/dotnet/api/system.io.file.exists?view=net-6.0
 * - https://learn.microsoft.com/en-us/dotnet/api/system.io.path?view=net-9.0
 * - https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-9.0
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace util
{
    // File reader for excel text file exports.
    // NOTE: if the file is in the Assets folder (or a subfolder of the Assets folder), you can just do it from there.
    public abstract class FileReader : MonoBehaviour
    {
        // File Name
        public string fileName = "";

        // File Path
        public string filePath = "";

        // Returns the file with its path.
        // If 'useBackSlash' is true, a backslash is put at the end of the file path if one does not exist.
        // If 'useBackSlash' is false, a forward slash is added to the end of the file path if one does not exist.
        public string GetFileWithPath(bool useBackSlash = true)
        {
            // The resulting filepath.
            string result = "";

            // Make sure they're set properly.
            SetFile(fileName);
            SetFilePath(filePath);

            // Combines the two.
            result = filePath + fileName;

            return result;
        }

        // Uses 'Path.Combine' to combine the provided file path and file.
        // If 'convertSlashes' is true, the function will alter the result to contain only back slashes ("\") or forward slashes ("/").
        // If the file path contains both a forward slash ("/") and a backwards slash ("\")...
        // All slashes are changed to whichever one appears first. This is because...
        // Path.Combine() can end up combining forward and backward slashes based on what's provided.
        public static string CombineFilePathAndFile(string filePath, string file, bool convertSlashes = true)
        {
            // Gets the combination as a string.
            string combo = Path.Combine(filePath, file);

            // The result to be returned.
            string result;

            // If the combination contains a forward and backward slash, convert all the slashes to one type.
            if(convertSlashes && combo.Contains("/") && combo.Contains("\\"))
            {
                // Gets the back and forward slash.
                int forwardSlashIndex = combo.IndexOf("/");
                int backSlashIndex = combo.IndexOf("\\");

                // If forward slash comes first, convert all back slashes to forward slashes.
                if(forwardSlashIndex < backSlashIndex)
                {
                    result = combo.Replace("\\", "/");
                }
                // If backslash comes first, or if the two indexes are the same (which shouldn't be possible)...
                // Convert all forward slashes to back slashes.
                else
                {
                    result = combo.Replace("/", "\\");
                }
            }
            else
            {
                // No need to make changes.
                result = combo;
            }

            // Return the result.
            return result;
        }

        // Sets the file.
        public void SetFile(string newFile)
        {
            fileName = newFile;
        }

        // sets the file and the file path.
        public void SetFile(string newFile, string newFilePath, bool useBackSlash = true)
        {
            SetFile(newFile);
            SetFilePath(newFilePath, useBackSlash);
        }

        // sets the file path 9make sure to use back slashes ('\'), not forward slashes.
        // If useBackSlash is set to false, forward slashes are used.
        public void SetFilePath(string newFilePath, bool useBackSlash = true)
        {
            // set new file path.
            filePath = newFilePath;

            // if the file path is not empty.
            if (filePath.Length != 0)
            {
                // if the last character is not a slash, add one.
                if (filePath[filePath.Length - 1] != '\\' && useBackSlash)
                {
                    filePath += "\\";
                }
                else if (filePath[filePath.Length - 1] != '/' && !useBackSlash)
                {
                    filePath += "/";
                }
            }
        }

        // Sets the file path and the file.
        public void SetFilePath(string newFilePath, string newFile)
        {
            SetFilePath(newFilePath);
            SetFile(newFile);
        }

        // Returns 'true' if the provided file exists.
        public static bool FileExists(string file)
        {
            bool result = File.Exists(file);

            return result;
        }

        // Checks if the file exists.
        public bool FileExists()
        {
            // Sets the file and file path to make sure they're formatted properly.
            SetFile(fileName, filePath);

            // Returns true if the file exists.
            bool result = File.Exists(GetFileWithPath());

            return result;
        }

        // Deletes the file.
        public bool DeleteFile()
        {
            // Checks if the file exists.
            if (FileExists())
            {
                // Deletes the file.
                File.Delete(GetFileWithPath());
                return true;
            }
            else
            {
                return false;
            }
        }

        // Returns 'true' if the provided file path exists.
        public static bool FilePathExists(string filePath)
        {
            bool result = Directory.Exists(filePath);

            return result;
        }

        // Checks if a file path exists.
        public bool FilePathExists()
        {
            bool result = Directory.Exists(filePath);

            return result;
        }

        // Makes the provided file directory.
        public static bool CreateFilePath(string filePath)
        {
            // Checks if the file path already exists.
            if (FilePathExists(filePath))
            {
                Debug.LogWarning("File directory already exists.");
                return false;
            }
            else
            {
                // Genrate the filePath.
                DirectoryInfo direcInfo = Directory.CreateDirectory(filePath);

                // Checks if it was successful.
                bool result = direcInfo.Exists;
                return result;
            }
        }

        // Makes the file filePath. Returns false if it fails, or if the filePath already exists.
        public bool CreateFilePath()
        {
            // If there is no file path, return false.
            if (filePath == string.Empty)
            {
                return false;
            }
            else // There's a file path set, so try to create it.
            {
                return CreateFilePath(filePath);
            }             
        }


        // Checks if the file is empty.
        public bool IsFileEmpty()
        {
            // Checks if the file exists.
            bool result = FileExists();

            // File exists, so open it and see if it has data.
            if (result)
            {
                // Opens a reading file stream.
                FileStream fs = File.OpenRead(GetFileWithPath());

                // Checks if the file stream is set.
                result = fs.Length == 0;


                // Close the file stream.
                fs.Close();
            }

            return result;
        }

        // Read from the file.
        public abstract void ReadFile();

    }
}