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
        "avocado", "pear", "plum", "extraterrestrial", "the", "or", "and", "is", "her", "kale",
        "characteristic", "recommendation", "constitutional", "correspondence", "matrix", "division",
        "go", "jaw", "rib", "cashew", "chocolate", "gamedev", "entertainment", "professionals",
        "pledge", "potatoes", "cream", "what", "if", "gods", "mythology", "humans", "war", "swords", "humanity"
        , "had", "its", "time", "reality", "poor", "families", "crash", "runway", "emergency", "couldnt", "changes",
        "worlds", "wars", "totality", "external", "tiffin", "muffin", "computer", "programming", "kidzone",
        "strawberry", "touch", "hands", "great", "man", "woman", "girls", "ramen", "business", "sweet", "bitter",
        "job", "struggle", "mathematics", "overcome", "python", "csharp", "java", "html", "css", "package", "college",
        "university", "max", "jack", "mahesh", "salmankhan", "sayan", "digital", "data", "structures", "algorithms", "android",
        "wish", "develop", "sweetheart", "humanity", "cracker", "water", "priority", "space", "economics", "theory", "timetable",
        "insert", "dates", "cloud", "physicist", "tutor", "oral", "at", "whenever", "wherever"};

   
    public string GetRandomWord()
    {
        int RandomNumber = Random.Range(0, words.Length);
        string RandomWord = words[RandomNumber];
        Debug.Log(RandomWord);
        return RandomWord;

    }
}
