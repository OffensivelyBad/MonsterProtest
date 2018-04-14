using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {

    [SerializeField] AudioClip hit = null;
    [SerializeField] AudioClip miss = null;
    [SerializeField] AudioClip death = null;
	
    public AudioClip Hit
    {
        get
        {
            return hit;
        }
    }
    public AudioClip Miss
    {
        get
        {
            return miss;
        }
    }
    public AudioClip Death
    {
        get
        {
            return death;
        }
    }

}
