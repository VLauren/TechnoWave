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

    Vector3 puntoDeDeformacion;
    Vector3 direccion = Vector3.zero;

    Vector3[][] verticesOriginales, verticesDesplazados;
    Mesh[] mallasDeformadas;

    public float altura, velocidad, ancho;
     
    void Awake()
    {
        instancia = this;
        objetos = new List<GameObject>();
        foreach (Transform t in objetosDeformables)
            objetos.Add(t.gameObject);
    }

	void Start ()
	{
        puntoDeDeformacion = new Vector3(-1, 0, 0);

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
	
	void Update () 
	{
        if (direccion == Vector3.zero)
            return;


        // por cada objeto
        for (int i = 0; i < verticesOriginales.Length; i++)
        {
            Vector3 punto = objetos[i].transform.InverseTransformPoint(puntoDeDeformacion);
            float escala = objetos[i].transform.localScale.x;

            // por cada punto
            for (int j = 0; j < verticesOriginales[i].Length; j++)
            {
                // calculo cuanto debo deformar
                Vector3 pto = PtoMasCercanoALina(punto, Quaternion.Euler(0,90,0) * direccion, verticesOriginales[i][j]);
                float distancia = Vector3.Distance(pto, verticesOriginales[i][j]);

                // ajuste por la escala
                distancia *= escala;

                float cantidad = ancho - distancia;
                if (cantidad < 0) cantidad = 0;
                else
                    cantidad /= escala;

                // asigno el nuevo punto
                verticesDesplazados[i][j] = verticesOriginales[i][j] - Vector3.up * cantidad * altura;
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
        puntoDeDeformacion = ini;
        direccion = dir;
    }

    // ========================================

    public static Vector3 PtoMasCercanoALina(Vector3 lineaPtn, Vector3 lineaDir, Vector3 ptn)
    {
        lineaDir.Normalize();
        var v = ptn - lineaPtn;
        var d = Vector3.Dot(v, lineaDir);
        return lineaPtn + lineaDir * d;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(puntoDeDeformacion, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(puntoDeDeformacion+direccion, 0.1f);
    }
}
