using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Titulo : MonoBehaviour
{
	void Update ()
    {
        // cualquier tecla -> go
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            CambioDeEscena.instancia.MusicaB();
            CambioDeEscena.instancia.SiguienteNivel();
        }

        // escape -> salir
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}
}
