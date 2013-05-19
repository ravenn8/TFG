using UnityEngine;
using System.Collections;

public class consigueDobleSalto : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		Fisicas acFisicas = other.gameObject.GetComponent<Fisicas>();
		if (acFisicas)
		{
			// se consigue doble salto y se destruye el item cuando se toca
			acFisicas.dobleSalto = true;
			Destroy(gameObject);
		}
	}
}