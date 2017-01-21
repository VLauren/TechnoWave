using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efectos : MonoBehaviour 
{
    public static Efectos instancia { get; private set; }

    public GameObject cargarOnda;
    public GameObject crearOnda;

    private void Awake()
    {
        instancia = this;
    }

    public void FXCargarOnda(Vector3 pos)
    {
        Instantiate(cargarOnda, pos, Quaternion.identity);
    }

    public void FXCrearOnda(Vector3 pos, Quaternion rot)
    {
        Instantiate(crearOnda, pos, rot * Quaternion.Euler(0,90,0));
    }

}
