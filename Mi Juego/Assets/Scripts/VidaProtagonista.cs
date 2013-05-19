using UnityEngine;
using System.Collections;

public class VidaProtagonista : MonoBehaviour {

	public int Health = 100;
	Fisicas fisicasPersonaje;
	Controles controlesPersonaje;
	Animacion animacionesPersonaje;
	Ataque ataquePersonaje;
	public GUIStyle estilo;
	
	void Start(){
		fisicasPersonaje = GetComponent<Fisicas>();
		controlesPersonaje = GetComponent<Controles>();
		animacionesPersonaje = GetComponent<Animacion>();
		ataquePersonaje = GetComponent<Ataque>();
	}
	void ApplyDammage (int TheDammage)
	{
		Health -= TheDammage;
		Debug.Log(Health);
		if(Health <= 0)
		{
			StartCoroutine(Dead ());
		}
	}

	//void Dead()
	IEnumerator Dead()
	{
		animacionesPersonaje.Muere();
		controlesPersonaje.QuitaControl();
		fisicasPersonaje.QuitaVida();
		ataquePersonaje.QuitaControl();
		

		yield return new WaitForSeconds(1.0f);
		Health = 100;
		fisicasPersonaje.Reset ();
		animacionesPersonaje.Vive();
		controlesPersonaje.DaControl();
		ataquePersonaje.DaControl();
	}
	
	void OnGUI(){
		if(Health < 0){
			GUI.Label(new Rect(400,0,Screen.height,Screen.width), "Vida actual "+ "0" + " de 100",estilo);
		}else{
			GUI.Label(new Rect(400,0,Screen.height,Screen.width), "Vida actual "+ Health+ " de 100",estilo);
		}
	}
}
