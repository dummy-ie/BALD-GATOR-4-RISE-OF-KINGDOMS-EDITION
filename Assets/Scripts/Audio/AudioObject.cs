using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    void Update()
    {
        if (!GetComponent<AudioSource>().isPlaying)
            Destroy(gameObject);
    }
}
