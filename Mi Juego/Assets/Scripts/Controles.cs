using UnityEngine;
using System.Collections;

public class Controles : MonoBehaviour
{
	Fisicas fisicasPersonaje;
	bool tieneControl;
	bool pausa;
	public GUIStyle estilo;

	void Start () 
	{
		tieneControl = true;
		fisicasPersonaje = GetComponent<Fisicas>();
		if (fisicasPersonaje == null)
			Debug.LogError("This object also needs a PlatformerPhysics component attached for the controller to function properly");
	}

	void Update () 
	{
		if (fisicasPersonaje && tieneControl)
		{
			if (Input.GetKey(KeyCode.RightShift))
				fisicasPersonaje.EmpiezaSprint();

			if (Input.GetKeyUp(KeyCode.RightShift))
				fisicasPersonaje.AcabaSprint();

			if (Input.GetKeyDown(KeyCode.DownArrow))
				fisicasPersonaje.Agacharse();

			if (Input.GetKeyUp(KeyCode.DownArrow))
				fisicasPersonaje.DejaAgacharse();
			
			if (Input.GetKeyDown(KeyCode.P))
				pause();
			
			if (Input.GetKeyUp(KeyCode.R))
				Application.LoadLevel("prueba");
        
			
		}
	}

	void FixedUpdate()
	{
		if (fisicasPersonaje && tieneControl)
		{
			if (Input.GetButton("Fire2"))
				fisicasPersonaje.Salto();

			fisicasPersonaje.Andar(Input.GetAxisRaw("Horizontal"));
		}
	}
	
	void pause(){
		if(!pausa){
			Time.timeScale = 0;
			pausa = true;
		}else{
			Time.timeScale = 1;
			pausa = false;
		}
	}
	
	void OnGUI(){
		if(pausa){
				GUI.Label(new Rect(Screen.height + 250,0,100,30),"MENU PAUSA",estilo);
				if(GUI.Button(new Rect(Screen.height + 300,20,100,30),"Continuar",estilo)){
					Time.timeScale = 1.0f;
					pausa = false;
				}
				if(GUI.Button(new Rect(Screen.height + 300,40,100,30),"Salir",estilo)){
					Application.LoadLevel("MainMenu");
				}
		}
	}			

	public void DaControl() { tieneControl = true; }
	public void QuitaControl() { tieneControl = false; }
	public bool EnControl() { return tieneControl; }
}

