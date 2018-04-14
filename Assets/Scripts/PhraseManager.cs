using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhraseManager : Singleton<PhraseManager>
{
    private readonly List<string> phrases = new List<string>();

    public string GetRandomPhrase() {
        if (phrases.Count <= 0) {
            PopulatePhrases();
        }
        int randomIndex = Random.Range(0, phrases.Count - 1);
        return phrases[randomIndex];
    }

    private void PopulatePhrases() {
        phrases.Add("old heads can't type");
        phrases.Add("this is like a test");
        phrases.Add("triggering me bro");
        phrases.Add("I like to kill deer");
        phrases.Add("Mister T is a badass");
        phrases.Add("the cat stole my keys");
        phrases.Add("the quick brown\nfox jumps over\nthe lazy dog");
    }

}
