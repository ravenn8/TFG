using UnityEngine;
using System.Collections;

public class Control_Personaje : MonoBehaviour {
	
	Fisicas_Personaje fisicasPJ;
	
	// Use this for initialization
	void Start () {
		fisicasPJ = GetComponent<Fisicas_Personaje>(); // se inicializan las fisicas
	}
	
	// Update is called once per frame
	void Update () {
		fisicasPJ.Caminar(Input.GetAxisRaw("Horizontal")); // envio datos del eje horizontal enviado por el teclado
		
		if (Input.GetButton("Jump")){ // si se pulsa la tecla Jump asignada a la barra espaciadora
				fisicasPJ.Salto(); // ejecuto el salto
		}
	
	}
}
