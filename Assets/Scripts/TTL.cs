using UnityEngine;
using System.Collections;

public class TTL : MonoBehaviour
{
    public float tiempo = 1;

	void Start ()
    {
	    Invoke("Destruir",tiempo);
	}
	
	void Destruir ()
    {
        Destroy(gameObject);
	}
}
