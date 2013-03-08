using UnityEngine;
using System.Collections;

public class Control_Personaje : MonoBehaviour {
	
	Fisicas_Personaje fisicasPJ;
	bool controlActivo;
	
	// Use this for initialization
	void Start () {
		fisicasPJ = GetComponent<Fisicas_Personaje>(); // se inicializan las fisicas
		controlActivo = true; // variable que nos indica si podemos movernos o no
	}
	
	// Update is called once per frame
	void Update () {
		if(controlActivo){
			fisicasPJ.Caminar(Input.GetAxisRaw("Horizontal")); // envio datos del eje horizontal enviado por el teclado
			
			if (Input.GetButton("Jump")){ // si se pulsa la tecla Jump asignada a la barra espaciadora
					fisicasPJ.Salto(); // ejecuto el salto
			}
			if (Input.GetKeyDown(KeyCode.RightShift)){ // si se pulsa se inicia esprint
				fisicasPJ.Esprintar();
			}

			if (Input.GetKeyUp(KeyCode.RightShift)){
				fisicasPJ.noEsprintar();
			}
		}
	}
	// Metodos de estar en control o no
	public void DaControl() { controlActivo = true; }
	public void QuitaControl() { controlActivo = false; }
	public bool TieneControl() { return controlActivo; }
}
