using UnityEngine;
using System.Collections;

public class Plano_Muerte : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		Control_Personaje control = other.gameObject.GetComponent<Control_Personaje>();
		if (control && control.TieneControl())
		{
			// dejamos al personaje morir
			StartCoroutine(Personaje_Muere(other.gameObject));
			// la StartCouroutine es un tipo de rutina que se puede pausar en cualquier momento usando unyield, en este caso ideal para darnos un tiempo para "resucitar"

		}
	}

	IEnumerator Personaje_Muere(GameObject jugador)
	{
		jugador.GetComponent<Control_Personaje>().QuitaControl();

		yield return new WaitForSeconds(0.5f);

		jugador.GetComponent<Fisicas_Personaje>().Reset();
		jugador.GetComponent<Control_Personaje>().DaControl();
	}
}
