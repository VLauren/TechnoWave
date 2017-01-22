using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    private const string NOMBRE_MODELO = "Modelo";
    private const string NOMBRE_ANIM_VELOCIDAD = "Velocidad";
    private const string NOMBRE_ANIM_SUELO = "Suelo";
    private const string NOMBRE_ANIM_SHOOTA = "ShootA";
    private const string NOMBRE_ANIM_SHOOTB = "ShootB";

    public static Jugador instancia { get; private set; }

    // referencias
    public Transform modelo { get; private set; }
    public CharacterController cc { get; private set; }
    public bool moviendose { get; private set; }
    public bool TWBBlock = true;

    Animator anim;
    public Transform ptoLanzar;
    public GameObject TWBEquipado;

    public float velocidad, fuerzaSalto;

    void Start()
    {
        // referencias
        modelo = transform.Find(NOMBRE_MODELO);
        instancia = this;
        anim = GetComponentInChildren<Animator>();
        cc = GetComponent<CharacterController>();
        //ptoLanzar = transform.Find("Modelo/PuntoLanzar");
    }

    private Vector3 movimiento;
    private Vector3 gravedad;
    public float accelGravedad = 0.7f;
    float contPaso;
    /*float tPaso = (2.4f/20f)*50;
    float iniPaso = (2.4f/10)*50;*/
    float tPaso = 0.38f;
    float iniPaso = 0.19f;

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

        if (moviendose && cc.isGrounded)
        {
            contPaso += Time.fixedDeltaTime;
            if (contPaso >= tPaso)
            {
                Sonido.instancia.Paso();
                contPaso = 0;
            }
        }
        else
            contPaso = iniPaso;
    }

    Quaternion rotObjetivo;
    bool cargando;

    // convierte vector de dirección en movimiento del jugador
    public void Mover(Vector3 direccion)
    {
        if (cargando)
        {
            modelo.rotation = Quaternion.RotateTowards(modelo.rotation, rotObjetivo, Time.deltaTime * 360);
            movimiento = Vector3.zero;
            if (anim)
                anim.SetFloat(NOMBRE_ANIM_VELOCIDAD, 0);
            return;
        }

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
            moviendose = true;
        }
        else
            moviendose = false;

        if (anim)
            anim.SetFloat(NOMBRE_ANIM_VELOCIDAD, direccion.magnitude);

        // actualiza el vector de movimiento
        movimiento = direccion * Time.fixedDeltaTime * velocidad;
    }

    public void LanzarOnda()
    {
        if(!cargando && !TWBBlock)
            StartCoroutine(Onda());
    }

    float delay = 1.5f;

    IEnumerator Onda()
    {
        // onda anterior -> fuera
        Deformador.instancia.FadeOut();

        // Anim
        if (anim)
            anim.SetTrigger(NOMBRE_ANIM_SHOOTA);

        // FX
        Efectos.instancia.FXCargarOnda(ptoLanzar);

        // el personaje se gira hacia donde apunta la camara
        cargando = true;
        Vector3 alante = Camera.main.transform.forward;
        alante.y = 0;
        rotObjetivo = Quaternion.LookRotation(alante, Vector3.up);
        if (modelo)
            modelo.rotation = Quaternion.RotateTowards(modelo.rotation, rotObjetivo, Time.deltaTime * 360);

        // zoom
        StartCoroutine(ZoomCam(110, delay));

        // sonido
        Sonido.instancia.Play("carga",0.25f);

        // mespero
        yield return new WaitForSeconds(delay);

        // inicio la deformacion
        Deformador.instancia.Lanzar(transform.position, alante);

        // FX
        Efectos.instancia.FXCrearOnda(ptoLanzar.position, Quaternion.LookRotation(alante, Vector3.up));

        // sonido
        Sonido.instancia.Play("disparo", 2);

        // Anim
        if (anim)
            anim.SetTrigger(NOMBRE_ANIM_SHOOTB);

        yield return new WaitForSeconds(0.2f);
        // des-zoom
        StartCoroutine(ZoomCam(75, 0.1f));

        // cam shakeeee
        TemblorCamara.instancia.Temblor();
        cargando = false;
    }

    IEnumerator ZoomCam(float anguloObj, float tiempo)
    {
        float fovIni = Camera.main.fieldOfView;
        float gradosPorSegundo = (anguloObj - fovIni) / tiempo;
        float cont = 0;
        while(cont < tiempo)
        {
            Camera.main.fieldOfView += Time.deltaTime * gradosPorSegundo;
            cont += Time.deltaTime;

            yield return null;
        }
        Camera.main.fieldOfView = anguloObj;
    }

    public void Saltar()
    {
        if(cc.isGrounded)
            gravedad += new Vector3(0, fuerzaSalto, 0);
    }

    public void PegarAlSuelo()
    {
        if (!cc.isGrounded)
            return;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 2, -Vector3.up, out hit))
        {
            float dif = hit.point.y - transform.position.y + 0.765f;
            if (dif < 0.15f && dif > 0)
                transform.position = hit.point + Vector3.up * 0.765f;
        }
    }
}
