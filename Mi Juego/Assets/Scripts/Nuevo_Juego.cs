using UnityEngine;


public class Nuevo_Juego : MonoBehaviour
{
	
	public void OnGUI(){
		if(GUI.Button(new Rect(380,200,250,100), "Empezar")){
			Application.LoadLevel("fase1");
		}
		if(GUI.Button(new Rect(380,310,250,100), "Controles")){
			Application.LoadLevel("controles");
		}
		if(GUI.Button(new Rect(380,420,250,100), "Salir")){
			Application.Quit();
		}
	
	}
}