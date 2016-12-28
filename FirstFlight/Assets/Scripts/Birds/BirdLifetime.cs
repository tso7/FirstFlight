using UnityEngine;
using System.Collections;

public class BirdLifetime : MonoBehaviour {

    public float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
