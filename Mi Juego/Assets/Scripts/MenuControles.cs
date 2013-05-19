using UnityEngine;
using System.Collections;

public class MenuControles : MonoBehaviour {
	public GUIStyle estilo;
	public GUIStyle estilo2;

	public void OnGUI(){
		GUI.Label(new Rect(400,200,Screen.height,Screen.width), "Controles",estilo);
		
		GUI.Label(new Rect(300,300,Screen.height,Screen.width), "Flechas Izq/Der - Moverse",estilo2);
		GUI.Label(new Rect(300,350,Screen.height,Screen.width), "Flechas Abajo - Deslizarse",estilo2);
		GUI.Label(new Rect(300,400,Screen.height,Screen.width), "Raton1 - Golpear",estilo2);
		GUI.Label(new Rect(300,450,Screen.height,Screen.width), "Raton2 - Saltar",estilo2);
		GUI.Label(new Rect(300,500,Screen.height,Screen.width), "R - Reiniciar nivel",estilo2);
		GUI.Label(new Rect(300,550,Screen.height,Screen.width), "P - Menu pausa",estilo2);
		
		if(GUI.Button(new Rect(580,310,250,100), "Volver Menu")){
			Application.LoadLevel("mainMenu");
		}
		
	}
}
