using UnityEngine;
using System.Collections;

public class GenericSingleton<T> : MonoBehaviour
    where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs.Length > 0)
                {
                    if (objs.Length > 1)
                    {
                        Debug.LogError("There are more than one" + typeof(T).Name + "Singleton");
                    }
                    else _instance = objs[0] as T;
                }
                else
                {
                    GameObject gameobj = new GameObject();
                    _instance = gameobj.AddComponent<T>();
                }
            }
            return _instance;
        }
        private set { }
    }
}
