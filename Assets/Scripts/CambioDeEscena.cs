using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioDeEscena : MonoBehaviour
{
    public static CambioDeEscena instancia { get; private set; }

    Scene escenaActual;
    int indiceNivel;
    public GameObject jugYCam;
    //Vector3 posIni;

    private void Awake()
    {
        instancia = this;
        indiceNivel = 1;

        SceneManager.LoadScene(indiceNivel, LoadSceneMode.Additive);
        //posIni = jugYCam.transform.position;
    }


    public void SiguienteNivel()
    {
        SceneManager.UnloadSceneAsync(indiceNivel);
        indiceNivel++;
        SceneManager.LoadScene(indiceNivel, LoadSceneMode.Additive);

        if(jugYCam != null)
            jugYCam.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
