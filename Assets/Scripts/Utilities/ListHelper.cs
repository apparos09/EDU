using System.Collections.Generic;
using UnityEngine;

namespace util
{
    // The list helper.
    public class ListHelper : MonoBehaviour
    {
        // Randomizes the array order.
        public static T[] RandomizeArrayOrder<T>(T[] arr)
        {
            // Converts the array to a list, randomizes it, turns it into an array, and returns it.
            return RandomizeListOrder(new List<T>(arr)).ToArray();
        }

        // Randomizes the order of the list elements.
        public static List<T> RandomizeListOrder<T>(List<T> list)
        {
            // Creates a copy of the list and makes a list for random values.
            List<T> listCopy = new List<T>(list);
            List<T> randList = new List<T>();

            // While the list copy has values, add them to randList in a random order.
            while(listCopy.Count > 0)
            {
                // Generates a random index, grabs the element from listCopy, and adds it to the randList.
                // The element is then removed from the list copy.
                int randIndex = Random.Range(0, listCopy.Count);
                randList.Add(listCopy[randIndex]);
                listCopy.RemoveAt(randIndex);
            }

            // Returns the randomized list.
            return randList;
        }
    }
}