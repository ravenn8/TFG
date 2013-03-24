using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		Fisicas_Personaje fisicas = other.gameObject.GetComponent<Fisicas_Personaje>();
		if (fisicas)
		{
			// coloca nuevo punto de respawn dando la posicion actual donde se ha activado este collider
			fisicas.SetRespawn(transform.position);
		}
	}
}