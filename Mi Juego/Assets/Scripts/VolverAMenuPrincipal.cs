using UnityEngine;


public class VolverAMenuPrincipal : MonoBehaviour
{
	
	public void OnGUI(){
		if(GUI.Button(new Rect(380,200,250,100), "Volver al Menu")){
			Application.LoadLevel("mainMenu");
		}	
	}
}