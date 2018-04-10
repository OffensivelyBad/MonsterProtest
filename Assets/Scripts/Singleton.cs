using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static T instance;

    public static T Instance {
        get {
            T theObject = FindObjectOfType<T>();
            if (instance == null)
                instance = theObject;
            else if (instance != theObject)
                Destroy(theObject);

            DontDestroyOnLoad(theObject);
            return instance;
        }
    }

}
