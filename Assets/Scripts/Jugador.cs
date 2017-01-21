using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    private const string NOMBRE_MODELO = "Modelo";
    private const string NOMBRE_ANIM_VELOCIDAD = "Velocidad";
    private const string NOMBRE_ANIM_SUELO = "Suelo";

    public static Jugador instancia { get; private set; }

    // referencias
    public Transform modelo { get; private set; }
    public CharacterController cc { get; private set; }
    private Animator anim;

    public float velocidad, fuerzaSalto;

    void Start()
    {
        // referencias
        modelo = transform.Find(NOMBRE_MODELO);
        instancia = this;
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
    }

    private Vector3 movimiento;
    private Vector3 gravedad;
    public float accelGravedad = 0.7f;

    void FixedUpdate()
    {
        // gravedad
        if (cc.isGrounded && gravedad.y < 0)
            gravedad = Vector3.zero;
        gravedad += new Vector3(0, -accelGravedad * Time.fixedDeltaTime, 0);

        // cc.Move
        if (cc)
            cc.Move(movimiento + gravedad);

        anim.SetBool(NOMBRE_ANIM_SUELO, cc.isGrounded);
    }

    private Quaternion rotObjetivo;

    // convierte vector de dirección en movimiento del jugador
    public void Mover(Vector3 direccion)
    {
        if (direccion != Vector3.zero)
        {
            // rotacion del modelo
            if (movimiento != Vector3.zero)
            {
                rotObjetivo = Quaternion.LookRotation(movimiento, Vector3.up);
                if (modelo)
                    modelo.rotation = Quaternion.RotateTowards(modelo.rotation, rotObjetivo, Time.deltaTime * 360);
            }

            // tomo la rotacion en el eje y de la cámara
            float rCam = Camera.main.transform.eulerAngles.y;

            // el movimiento es relativo a la direccion de la cámara
            direccion = Quaternion.Euler(0, rCam, 0) * direccion;
        }

        if (anim)
            anim.SetFloat(NOMBRE_ANIM_VELOCIDAD, direccion.magnitude);

        // actualiza el vector de movimiento
        movimiento = direccion * Time.fixedDeltaTime * velocidad;
    }

    public void LanzarOnda()
    {
        //TODO efectos
        //TODO animacion
        StartCoroutine(Onda());
    }

    float delay = 1f;

    IEnumerator Onda()
    {
        Deformador.instancia.FadeOut();
        yield return new WaitForSeconds(delay);
        Deformador.instancia.Lanzar(transform.position, modelo.transform.forward);
    }

    public void Saltar()
    {
        if(cc.isGrounded)
            gravedad += new Vector3(0, fuerzaSalto, 0);
    }
}
