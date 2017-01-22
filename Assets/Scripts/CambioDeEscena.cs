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
        if (indiceNivel <= 4)
            SceneManager.LoadScene(indiceNivel, LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(0, LoadSceneMode.Single);

        if (jugYCam != null)
            jugYCam.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MusicaA();
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public GameObject mA, mB;

    public void MusicaA()
    {
        mA.SetActive(true);
        mB.SetActive(false);
    }

    public void MusicaB()
    {
        mA.SetActive(false);
        mB.SetActive(true);
    }

}
