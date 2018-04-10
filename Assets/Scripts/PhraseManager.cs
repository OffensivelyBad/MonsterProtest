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
        phrases.Add("this is a phrase");
        phrases.Add("another phrase");
        phrases.Add("and one more");
        phrases.Add("i like to kill deer");
        phrases.Add("Mister T is a badass");
        phrases.Add("he had a no hitter and I was completely oblivious to it");
    }

}
