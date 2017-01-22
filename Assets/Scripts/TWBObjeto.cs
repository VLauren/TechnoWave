using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWBObjeto : MonoBehaviour
{

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * 90, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Jugador.instancia.TWBBlock = false;
            if (Jugador.instancia.TWBEquipado != null)
                Jugador.instancia.TWBEquipado.SetActive(true);
            Efectos.instancia.PillarTWB(transform.position);
            Destroy(gameObject);
        }
    }
}
