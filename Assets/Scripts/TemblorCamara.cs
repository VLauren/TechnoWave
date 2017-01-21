using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemblorCamara : MonoBehaviour
{
    public static TemblorCamara instancia { get; private set; }

    Transform camTransform;
    float fuerza = 0.1f;
    int frames = 30;

    private void Awake()
    {
        instancia = this;
    }

    public void Temblor()
    {
        camTransform = Camera.main.transform.parent.parent;
        StartCoroutine(Sacude());
    }

    float intensidad;

    IEnumerator Sacude()
    {
        intensidad = fuerza;
        for (int i = 0; i < frames; i++)
        {
            camTransform.position += Random.onUnitSphere * intensidad;
            intensidad -= fuerza / frames;
            yield return null;
        }

    }
}
