using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError($"Failed to find '{typeof(T).Name}'object\nPlease make {typeof(T).Name} with object");
                }
            }
            return instance;
        }
    }
}
