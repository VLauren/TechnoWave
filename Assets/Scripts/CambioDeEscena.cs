using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    public static CambioDeEscena instancia { get; private set; }

    Scene escenaActual;
    int indiceNivel;

    private void Awake()
    {
        instancia = this;
        indiceNivel = 1;

        SceneManager.LoadScene(indiceNivel, LoadSceneMode.Additive);
    }

    public GameObject jugYCam;

    public void SiguienteNivel()
    {
        SceneManager.UnloadSceneAsync(indiceNivel);
        indiceNivel++;
        SceneManager.LoadScene(indiceNivel, LoadSceneMode.Additive);

        // chapuza
        if(jugYCam != null)
            jugYCam.SetActive(true);
    }
}
