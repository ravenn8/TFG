using UnityEngine;
using System.Collections;

public class Fisicas_Personaje : MonoBehaviour { // todos los scripts derivan de MonoBehaviour(script basico)
	
	// Variables
	Vector3 posicionInicial; 					// =FALTA POR HACER= posicion del personaje 
	float acceleracionInicial			= 35;   // parametro de la aceleracion
	bool esta_en_el_suelo				= true; // booleano para controlar si esta en contacto con el suelo
	Vector3 direccionSuelo				= Vector3.right; // en la direccion que nos movemos "right" = (1,0,0)
	float fuerza_de_freno				= 0; 	// fuerza de frenado 
	float friccion						= 0.9f; // nivel de friccion
	float gravedad						= 3.5f; // nivel de gravedad
	float velocidad_salto				= 12;	// capacidad de salto
	
	// Use this for initialization
	void Start () {
		posicionInicial = transform.position; // obtengo posicion inicial
	}
	
	// Update is called once per frame
	void Update () {
		AplicarFriccion();
		AplicarGravedad();
	}
	
    
	public void Caminar(float direccion) // se llama cuando jugador pulsa -1.0 izquierda 1.0 derecha
	{
		float acceleracion = acceleracionInicial; // asigno mi aceleracion base

		rigidbody.AddForce(direccionSuelo * direccion * acceleracion, ForceMode.Acceleration); 
		// aplico la fuerza para moverlo usando ForceMode.Acceleration para no tener en cuenta la masa del futuro personaje
		fuerza_de_freno = 1 - Mathf.Abs(direccion); // nueva fuerza de frenado
	}
	
	public void Salto()
	{
		Vector3 velocidad = rigidbody.velocity; // obtengo velocidad
		velocidad.y = velocidad_salto; // le añado el salto
		rigidbody.velocity = velocidad; // lo aplico
	}
	
	void AplicarFriccion()
	{
		Vector3 velocidad = rigidbody.velocity; // obtengo la velocidad de mi rigidbody

		// Aplico la friccion del suelo
		if (esta_en_el_suelo && fuerza_de_freno > 0.0f)
		{
			Vector3 velocidadEnTierra = Vector3.Dot(velocidad, direccionSuelo) * direccionSuelo; // proyecto la velocidad en el suelo
			// Info: Vector3.Dot me devuelve 1 si van en la misma direccion -1 opuesta y 0 perpendicular
			Vector3 nuevaVelocidadEnTierra = velocidadEnTierra * Mathf.Lerp(1.0f, friccion, fuerza_de_freno); // se aplica la friccion del suelo
			// Info: Mathf.Lerp me interpola los puntos buscandome valor intermedio
			velocidad -= (velocidadEnTierra - nuevaVelocidadEnTierra); // aplico a la velocidad actual
		}
		
		rigidbody.velocity = velocidad; // Aplico la velocidad final a mi rigidbody

		fuerza_de_freno = 1.0f; // si aun no ha frenado seguira frenando
	}
	
	void AplicarGravedad()
	{
		if(esta_en_el_suelo){ // NOTA IMPORTANTE: falta poner en negativo para aplicar solo la gravedad en el aire  
			rigidbody.AddForce(Physics.gravity * gravedad, ForceMode.Acceleration);
		}
	}
}
