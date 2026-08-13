using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace util
{
    // A helper class for various string related functions.
    public class StringHelper : MonoBehaviour
    {
        // Returns the number of English alphabet letters.
        public static int GetNumberOfEnglishAlphabetLetters()
        {
            return 26;
        }

        // Returns the English alphabet number of the provided letter.
        // -1 is returned if the char isn't a recognized lettter.
        public static int GetEnglishAlphabetLetterNumber(char letter)
        {
            // The number of the letter.
            int letterNumber;

            // OLD - uses an array to store the letters and provide the number.
            // // The uppercase verison of the character.
            // // This is used to search the list of letters.
            // char upperChar = char.ToUpper(letter);
            // 
            // // The letters of the alphabet.
            // // An extra space is added in slot (0) to make the numbers match up.
            // List<char> letters = new List<char>()
            // {
            //     ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            //     'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
            //     'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            // };
            // 
            // // Checks if the character is in the letter list.
            // if (letters.Contains(upperChar))
            // {
            //     // Gets the index of the letter and uses it as the number.
            //     // The blank slot at index (0) allows the index number to match up to the alphabet number.
            //     letterNumber = letters.IndexOf(upperChar);
            // }
            // else
            // {
            //     // the letter isn't in the list, so put -1.
            //     letterNumber = -1;
            // }
            // 
            // // Returns the letter number.
            // return letterNumber;


            // NEW - uses a switch statement and doesn't do conversion.
            // This avoids unneeded memory allogation from making a list.
            switch (letter)
            {
                default:
                    letterNumber = -1;
                    break;

                case 'A':
                case 'a':
                    letterNumber = 1;
                    break;

                case 'B':
                case 'b':
                    letterNumber = 2;
                    break;

                case 'C':
                case 'c':
                    letterNumber = 3;
                    break;

                case 'D':
                case 'd':
                    letterNumber = 4;
                    break;

                case 'E':
                case 'e':
                    letterNumber = 5;
                    break;

                case 'F':
                case 'f':
                    letterNumber = 6;
                    break;

                case 'G':
                case 'g':
                    letterNumber = 7;
                    break;

                case 'H':
                case 'h':
                    letterNumber = 8;
                    break;

                case 'I':
                case 'i':
                    letterNumber = 9;
                    break;

                case 'J':
                case 'j':
                    letterNumber = 10;
                    break;

                case 'K':
                case 'k':
                    letterNumber = 11;
                    break;

                case 'L':
                case 'l':
                    letterNumber = 12;
                    break;

                case 'M':
                case 'm':
                    letterNumber = 13;
                    break;

                case 'N':
                case 'n':
                    letterNumber = 14;
                    break;

                case 'O':
                case 'o':
                    letterNumber = 15;
                    break;

                case 'P':
                case 'p':
                    letterNumber = 16;
                    break;

                case 'Q':
                case 'q':
                    letterNumber = 17;
                    break;

                case 'R':
                case 'r':
                    letterNumber = 18;
                    break;

                case 'S':
                case 's':
                    letterNumber = 19;
                    break;

                case 'T':
                case 't':
                    letterNumber = 20;
                    break;

                case 'U':
                case 'u':
                    letterNumber = 21;
                    break;

                case 'V':
                case 'v':
                    letterNumber = 22;
                    break;

                case 'W':
                case 'w':
                    letterNumber = 23;
                    break;

                case 'X':
                case 'x':
                    letterNumber = 24;
                    break;

                case 'Y':
                case 'y':
                    letterNumber = 25;
                    break;

                case 'Z':
                case 'z':
                    letterNumber = 26;
                    break;
            }

            // Returns the result.
            return letterNumber;
        }

        // Gets the alphabet letter that the number corresponds to (in uppercase).
        // Returns a space character (' ') if it's an invalid number.
        public static char GetEnglishAlphabetLetterByNumber(int number)
        {
            // The result to return.
            char letter;

            // OLD - Uses an array.
            // // The letters of the alphabet.
            // // An extra space is added in slot (0) to make the numbers match up.
            // List<char> letters = new List<char>()
            // {
            //     ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
            //     'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q',
            //     'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            // };
            // 
            // // If the number is greater than 0 and less than the letter count, get the number.
            // // Index 0 is ignored since it's a space character and is not actually part of the alphabet.
            // // The space is just there to make the letters match up with their numbers.
            // if(number > 0 && number < letters.Count)
            // {
            //     // Gets the letter.
            //     letter = letters[number];
            // }
            // else
            // {
            //     // Wasn't a valid number, so return a space.
            //     letter = ' ';
            // }

            // NEW - uses a switch statement to avoid allocating memory.
            // Checks the number to know what letter to set.
            switch(number)
            {
                default:
                    letter = ' ';
                    break;

                case 1:
                    letter = 'A';
                    break;

                case 2:
                    letter = 'B';
                    break;

                case 3:
                    letter = 'C';
                    break;

                case 4:
                    letter = 'D';
                    break;

                case 5:
                    letter = 'E';
                    break;

                case 6:
                    letter = 'F';
                    break;

                case 7:
                    letter = 'G';
                    break;

                case 8:
                    letter = 'H';
                    break;

                case 9:
                    letter = 'I';
                    break;

                case 10:
                    letter = 'J';
                    break;

                case 11:
                    letter = 'K';
                    break;

                case 12:
                    letter = 'L';
                    break;

                case 13:
                    letter = 'M';
                    break;

                case 14:
                    letter = 'N';
                    break;

                case 15:
                    letter = 'O';
                    break;

                case 16:
                    letter = 'P';
                    break;

                case 17:
                    letter = 'Q';
                    break;

                case 18:
                    letter = 'R';
                    break;

                case 19:
                    letter = 'S';
                    break;

                case 20:
                    letter = 'T';
                    break;

                case 21:
                    letter = 'U';
                    break;

                case 22:
                    letter = 'V';
                    break;

                case 23:
                    letter = 'W';
                    break;

                case 24:
                    letter = 'X';
                    break;

                case 25:
                    letter = 'Y';
                    break;

                case 26:
                    letter = 'Z';
                    break;
            }

            return letter;
        }

        // Gets the alphabet letter that the number corresponds to, and returns ' ' if the number provided was invalid.
        // inUpperCase: determines if the returned character is uppercase (true) or lowercase (false).
        public static char GetEnglishAlphabetLetterByNumber(int number, bool inUpperCase)
        {
            // The base letter.
            char baseLetter = GetEnglishAlphabetLetterByNumber(number);

            // The converted letter, which saves the requested case of the character.
            // If another case (upper or lower) isn't available, the character remains unchanged.
            char covertedLetter = (inUpperCase) ? char.ToUpper(baseLetter) : char.ToLower(baseLetter);

            // Returns the converted letter.
            return covertedLetter;
        }


        // Gets the number of instances of a substring.
        public static int GetSubstringCount(string str, string substr)
        {
            // If the substring is blank.
            if(substr == string.Empty)
            {
                // If the string is also empty, count this as 1 instance.
                if(str == string.Empty)
                {
                    return 1;
                }
                // The string isn't empty, so return a instance count of 0.
                else
                {
                    return 0;
                }
            }

            // If the substring is not in the string at all, return 0.
            if(!str.Contains(substr))
            {
                return 0;
            }

            // OLD - checks each index.
            // // First check if the substring exists in the first place.
            // if(str.Contains(substr))
            // {
            //     // THe index of the current substring instance.
            //     int index = 0;
            // 
            //     // The number of instances.
            //     int instances = 0;
            // 
            //     // While the index is valid.
            //     while(index >= 0 && index < str.Length)
            //     {
            //         // Get the index
            //         index = str.IndexOf(substr, index);
            // 
            //         // If an instance was found.
            //         if(index != -1)
            //         {
            //             // Increase the number of instances.
            //             instances++;
            // 
            //             // Shift the index.
            //             index += substr.Length;
            //         }
            //         else // No instance found.
            //         {
            //             break;
            //         }
            //     }
            // 
            //     // Return the instances count.
            //     return instances;
            // 
            // }
            // else
            // {
            //     return 0;
            // }

            // V2 - uses replace function to compare strings
            
            // Makes a temporary string with all instances of the substring removed.
            string tempStr = str.Replace(substr, "");

            // Gets the difference in length between the two strings.
            int lenDiff = str.Length - tempStr.Length;

            // Calculates the number of instances.
            // Divide by 0 should not be possibly at this stage.
            int instances = lenDiff / substr.Length;

            // Returns the instance count.
            return instances;
        }

    }
}