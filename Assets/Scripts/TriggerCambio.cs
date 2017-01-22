using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCambio : MonoBehaviour
{
    public Puerta puertaEntrar, puertaSalir;

    private void Start()
    {
        puertaEntrar.Abrir();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(Secuencia());
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }

    IEnumerator Secuencia()
    {
        puertaEntrar.Cerrar();
        Sonido.instancia.Play("puerta");
        yield return new WaitForSeconds(0.5f);
        CambioDeEscena.instancia.SiguienteNivel();
        yield return new WaitForSeconds(0.5f);
        Sonido.instancia.Play("puerta");
        puertaSalir.Abrir();
    }
}
