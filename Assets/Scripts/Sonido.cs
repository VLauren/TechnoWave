using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonido : MonoBehaviour
{
    public static Sonido instancia { get; private set; }

    private void Awake()
    {
        instancia = this;
    }

    public void Play(string sonido, float vol = 1)
    {
        GetComponent<AudioSource>().PlayOneShot(Resources.Load(sonido) as AudioClip, vol);
    }

    public void Paso()
    {
        GetComponent<AudioSource>().PlayOneShot(Resources.Load("paso" + Random.Range(1, 3)) as AudioClip);
    }
}
