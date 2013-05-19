using UnityEngine;
using System.Collections;



public class Plano_Muerte : MonoBehaviour 
{

	void OnTriggerEnter(Collider other)
	{
		Controles control = other.gameObject.GetComponent<Controles>();
		if (control && control.EnControl())
		{
			// dejamos al personaje morir
			StartCoroutine(Personaje_Muere(other.gameObject));
			// la StartCouroutine es un tipo de rutina que se puede pausar en cualquier momento usando unyield, en este caso ideal para darnos un tiempo para "resucitar"

		}
	}

	IEnumerator Personaje_Muere(GameObject jugador)
	{
		jugador.GetComponent<Animacion>().Muere();
		jugador.GetComponent<Controles>().QuitaControl();
		jugador.GetComponent<Fisicas>().QuitaVida();
		jugador.GetComponent<Ataque>().QuitaControl();

		yield return new WaitForSeconds(1.0f);

		jugador.GetComponent<Fisicas>().Reset();
		jugador.GetComponent<Animacion>().Vive();
		jugador.GetComponent<Controles>().DaControl();
		jugador.GetComponent<Ataque>().DaControl();
	}
	
}
