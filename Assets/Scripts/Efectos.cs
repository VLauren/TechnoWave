using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Efectos : MonoBehaviour 
{
    public static Efectos instancia { get; private set; }

    public GameObject cargarOnda;
    public GameObject crearOnda;
    public GameObject pillarTWB;

    private void Awake()
    {
        instancia = this;
    }

    public void FXCargarOnda(Transform pos)
    {
        GameObject fx = Instantiate(cargarOnda, pos.position, Quaternion.identity);
        fx.transform.parent = pos;
    }

    public void FXCrearOnda(Vector3 pos, Quaternion rot)
    {
        Instantiate(crearOnda, pos, rot * Quaternion.Euler(0, 90, 0));
    }

    public void PillarTWB(Vector3 pos)
    {
        Instantiate(pillarTWB, pos, Quaternion.identity);
    }
}
