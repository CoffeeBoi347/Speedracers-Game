using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGenerator : MonoBehaviour
{
    // it generates a random word from the given list.
    public string[] words = new string[] {"apple", "banana", "cherry", "date", "elderberry",
        "fig", "grape", "honeydew", "kiwi", "lemon", "mango", "nectarine", "orange", "papaya",
        "quince", "raspberry", "strawberry", "tangerine", "ugli", "watermelon", "apricot", "blackberry",
        "blueberry", "cantaloupe", "dragonfruit", "elderflower", "fig", "guava", "huckleberry", "ita",
        "jackfruit", "kumquat", "lychee", "melon", "nectarine", "olive", "passionfruit", "quince",
        "rhubarb", "soursop", "tamarind", "ugli", "vanilla", "wolfberry", "xigua", "yam", "zucchini",
        "avocado", "pear", "plum"};

    public string GetRandomWord()
    {
        int RandomNumber = Random.Range(0, words.Length);
        string RandomWord = words[RandomNumber];
        Debug.Log(RandomWord);
        return RandomWord;

    }
}
