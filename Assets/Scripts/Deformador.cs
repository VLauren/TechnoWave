using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deformador : MonoBehaviour
 {
    public static Deformador instancia { get; private set; }

    // =========================================

    public Transform objetosDeformables;
    List<GameObject> objetos;

    // =========================================

    Vector3 puntoDeDeformacion, puntoDeDeformacion2;
    Vector3 direccion = Vector3.zero;

    Vector3[][] verticesOriginales, verticesDesplazados;
    Mesh[] mallasDeformadas;

    public static float altura = 1.3f;
    public static float velocidad = 1.4f;
    public static float ancho = 3;
    public static int unoDeCada = 2;

    void Awake()
    {
        instancia = this;
        objetos = new List<GameObject>();
        foreach (Transform t in objetosDeformables)
            objetos.Add(t.gameObject);

        velocidad += unoDeCada;
    }

	void Start ()
	{
        //puntoDeDeformacion = new Vector3(-1, 0, 0);

        mallasDeformadas = new Mesh[objetos.Count];
        verticesOriginales = new Vector3[objetos.Count][];
        verticesDesplazados = new Vector3[objetos.Count][];

        // guardo las posiciones originales
        for (int i = 0; i < objetos.Count; i++)
        {
            mallasDeformadas[i] = objetos[i].GetComponent<MeshFilter>().mesh;
            verticesOriginales[i] = objetos[i].GetComponent<MeshFilter>().mesh.vertices;
        }


        // hago que el mesh use los nuevos vertices
        for (int i = 0; i < verticesOriginales.Length; i++)
        {
            verticesDesplazados[i] = new Vector3[verticesOriginales[i].Length];
            for (int j = 0; j < verticesOriginales[i].Length; j++)
                verticesDesplazados[i][j] = verticesOriginales[i][j];
        }
    }

    int cont;

	void Update () 
	{
        if (direccion == Vector3.zero)
            return;

        cont++;
        if (cont >= unoDeCada)
            cont = 0;
        else
            return;

        puntoDeDeformacion2 = puntoDeDeformacion - direccion * ancho * 2;

        // por cada objeto
        for (int i = 0; i < verticesOriginales.Length; i++)
        {
            Vector3 punto = objetos[i].transform.InverseTransformPoint(puntoDeDeformacion);
            Vector3 punto2 = objetos[i].transform.InverseTransformPoint(puntoDeDeformacion2);
            float escala = objetos[i].transform.localScale.x;

            // por cada punto
            for (int j = 0; j < verticesOriginales[i].Length; j++)
            {
                // calculo cuanto debo deformar A
                Vector3 pto = PtoMasCercanoALinea(punto, Quaternion.Euler(0, 90, 0) * direccion, verticesOriginales[i][j]);
                pto = new Vector3(pto.x, verticesOriginales[i][j].y, pto.z);
                float distancia = Vector3.Distance(pto, verticesOriginales[i][j]);

                // ajuste por la escala
                distancia *= escala;

                // calculo cuanto debo deformar B
                pto = PtoMasCercanoALinea(punto2, Quaternion.Euler(0, 90, 0) * direccion, verticesOriginales[i][j]);
                pto = new Vector3(pto.x, verticesOriginales[i][j].y, pto.z);
                float distancia2 = Vector3.Distance(pto, verticesOriginales[i][j]);

                // ajuste por la escala
                distancia2 *= escala;


                float cantidad2 = 0;
                cantidad2 = ancho - distancia2;
                if (cantidad2 < 0) cantidad2 = 0;
                else
                {
                    cantidad2 = Mathf.Sin((cantidad2 / (2 * ancho)) * Mathf.PI);
                    cantidad2 /= escala;
                }

                float cantidad = 0;
                cantidad = ancho - distancia;
                if (cantidad < 0) cantidad = 0;
                else
                {
                    cantidad = Mathf.Sin((cantidad/(2 * ancho)) * Mathf.PI);
                    cantidad /= escala;
                }



                verticesDesplazados[i][j] = verticesOriginales[i][j] + Vector3.up * cantidad * altura;
                verticesDesplazados[i][j] = verticesDesplazados[i][j] - Vector3.up * cantidad2 * altura;

            }
        }

        // asigno a la malla los puntos calculados
        for (int i = 0; i < mallasDeformadas.Length; i++)
        {
            mallasDeformadas[i].vertices = verticesDesplazados[i];
            mallasDeformadas[i].RecalculateNormals();

            objetos[i].GetComponent<MeshCollider>().sharedMesh = mallasDeformadas[i];
        }

        // actualizo la posicion de la onda
        puntoDeDeformacion += Time.deltaTime * direccion * velocidad;
    }

    public void Lanzar(Vector3 ini, Vector3 dir)
    {
        puntoDeDeformacion = ini + dir * 1.5f;
        direccion = dir;
    }

    // ========================================

    public static Vector3 PtoMasCercanoALinea(Vector3 lineaPtn, Vector3 lineaDir, Vector3 ptn)
    {
        lineaDir.Normalize();
        var v = ptn - lineaPtn;
        var d = Vector3.Dot(v, lineaDir);
        return lineaPtn + lineaDir * d;
    }

    /*
    public static Vector3 PtoMasCercanoAPlano(Vector3 lineaPtn, Vector3 lineaDir, Vector3 ptn)
    {
        Vector3 lnDir = new Vector3(lineaDir.x, 0, lineaDir.z);
        Vector3 lnPtn = new Vector3(lineaPtn.x, 0, lineaPtn.z);
        Vector3 punto = new Vector3(ptn.x, 0, ptn.z);

        lnDir.Normalize();
        var v = punto - lnPtn;
        var d = Vector3.Dot(v, lnDir);
        return lnPtn + lnDir * d;
    }
    */

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(puntoDeDeformacion, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(puntoDeDeformacion+direccion, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(puntoDeDeformacion2, 0.1f);
    }
}
