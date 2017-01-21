using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJugador : MonoBehaviour
{
    private Jugador jugador;

    void Start()
    {
        jugador = GetComponent<Jugador>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //leer input y envíar mensajes al movedor
        Vector3 direccion = Vector3.zero;

        direccion.x = Input.GetAxisRaw("Horizontal");
        direccion.z = Input.GetAxisRaw("Vertical");

        jugador.Mover(direccion);

        if (Input.GetButtonDown("Fire1"))
            jugador.LanzarOnda();

        if(Input.GetButtonDown("Jump"))
            jugador.Saltar();

        //if(jugador.cc.isGrounded)
            jugador.PegarAlSuelo();
    }
}
