using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		Fisicas fisicas = other.gameObject.GetComponent<Fisicas>();
		if (fisicas)
		{
			// coloca nuevo punto de respawn dando la posicion actual donde se ha activado este collider
			fisicas.Checkpoint(transform.position);
		}
	}
}