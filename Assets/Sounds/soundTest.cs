using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTest : MonoBehaviour
{
    [SerializeField]
    private AudioClip deathSoundClip;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.instance.PlaySoundEffect(deathSoundClip, transform, 1.0f);
    }


}
