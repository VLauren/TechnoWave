using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puerta : MonoBehaviour
{
    public void Abrir()
    {
        //gameObject.SetActive(false);
        transform.Translate(new Vector3(4.6f, 0, 0));
    }

    public void Cerrar()
    {
        //gameObject.SetActive(true);
        transform.Translate(new Vector3(-4.6f, 0, 0));
    }
}
