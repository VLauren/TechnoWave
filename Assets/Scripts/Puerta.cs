using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public void Abrir()
    {
        gameObject.SetActive(false);
    }

    public void Cerrar()
    {
        gameObject.SetActive(true);
    }
}
