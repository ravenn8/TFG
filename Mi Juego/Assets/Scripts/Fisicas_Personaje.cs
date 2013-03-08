using UnityEngine;
using System.Collections;

public class Fisicas_Personaje : MonoBehaviour { // todos los scripts derivan de MonoBehaviour(script basico)
	
	// Variables
	Vector3 posicionInicial; 							// =FALTA POR HACER= posicion del personaje 
	public float acceleracionInicial			= 35;   // parametro de la aceleracion
	public float acceleracionEsprintando        = 50;	// parametro de esprintar
	public bool esta_en_el_suelo				= true; // booleano para controlar si esta en contacto con el suelo
	public Vector3 direccionSuelo				= Vector3.right; // en la direccion que nos movemos "right" = (1,0,0)
	public float fuerza_de_freno				= 0; 	// fuerza de frenado 
	public float friccion						= 0.9f; // nivel de friccion
	public float gravedad						= 3.5f; // nivel de gravedad
	public float velocidad_salto				= 12;	// capacidad de salto
	float personajeAltura;								// altura de la caja que lo contiene
	float personajeAnchura;								// anchura que lo contiene
	public float max_angulo_escalar				= 30.0f;// angulo maximo para saber si podemos mantenernos en una superficie o caer
	
	bool saltando								= false;// si esta saltando o no
	bool saltoPulsado							= false;// si aun esta pulsado el boton
	int  tiempo_de_salto						= 0;    // tiempo que podemos dejar el boton para llegar mas alto
	public int tiempo_salto_pulsado				= 15;   // para que el usuario pueda soltar antes o despues
	
	bool esprintar								= false;// si estamos esprintando o no
	
	// Inicializacion
	void Start () {
		posicionInicial = transform.position; // obtengo posicion inicial
		esta_en_el_suelo = false;
		saltando = false;
		saltoPulsado = false;
		tiempo_de_salto = 0;
	}
	
	// Update se llama una vez por frame
	void Update () {
		ControlarSuelo ();
		ControlarSalto ();
		AplicarFriccion();
		AplicarGravedad();
	}
	
    
	public void Caminar(float direccion) // se llama cuando jugador pulsa -1.0 izquierda 1.0 derecha
	{
		float acceleracion = acceleracionInicial; // asigno mi aceleracion base
		if (esprintar){
			acceleracion = acceleracionEsprintando;
		}

		rigidbody.AddForce(direccionSuelo * direccion * acceleracion, ForceMode.Acceleration); 
		// aplico la fuerza para moverlo usando ForceMode.Acceleration para no tener en cuenta la masa del futuro personaje
		fuerza_de_freno = 1 - Mathf.Abs(direccion); // nueva fuerza de frenado
	}
	
	public void Esprintar() // se llama al iniciar un esprint
	{
		esprintar = true;
	}
	
	public void noEsprintar() // se llama al acabar el esprint
	{
		esprintar = false;
	}
	
	public void Salto()
	{
		saltoPulsado = true;

		// Compruebo si puedo iniciar un salto
		if (tiempo_de_salto == 0 && !saltando)
		{
			if (esta_en_el_suelo) // si estoy en el suelo
			{
				tiempo_de_salto = tiempo_salto_pulsado;
				saltando = true;
			}
		}
		
		// Comprobamos si estamos en medio de un salto
		if(tiempo_de_salto != 0)
		{
			Vector3 velocidad = rigidbody.velocity; // obtengo velocidad
			velocidad.y = velocidad_salto; // le añado el salto
			rigidbody.velocity = velocidad; // lo aplico
		}
		
	}
	
	void ControlarSalto()
	{
		if (!saltoPulsado && saltando) // si estamos saltando y el boton de salto ya no esta pulsado
		{
			tiempo_de_salto = 0;
			saltando = false;
		}
		saltoPulsado = false;

		if (tiempo_de_salto != 0) // en el caso que aun nos quede tiempo en el aire descuento 1
			tiempo_de_salto--;
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
		if(!esta_en_el_suelo){ // si esta en el aire se aplicara la gravedad 
			rigidbody.AddForce(Physics.gravity * gravedad, ForceMode.Acceleration);
		}
	}
	
	void ControlarSuelo(){
		float sensibilidad = 0.05f; // distancia desde los pies a tocar el suelo
		float alturaExtra  = personajeAltura + 0.75f;
		float anchuraMitad = personajeAnchura + 0.5f;
		
		// para saber si tocare o no el suelo, realizare dos comprobaciones, partiendo de dos origenes diferentes, si alguna de las
		// dos se cumple, entonces pasariamos a estar en el suelo, si no, no
		Vector3 centroInferior = collider.bounds.center+collider.bounds.extents.y*Vector3.down; // obtengo el punto inferior del personaje
		Vector3 origen1 = centroInferior + Vector3.right * anchuraMitad + Vector3.up * alturaExtra;
		Vector3 origen2 = centroInferior + Vector3.left * anchuraMitad + Vector3.up * alturaExtra;
		Vector3 direccion = Vector3.down;
		RaycastHit choque; // creo una estructura para guardar un raycast(esto crea un rayo que al golpear con un collider nos devuelve informacion )
	
		if (Physics.Raycast(origen1, direccion, out choque) && (choque.distance < alturaExtra + sensibilidad))
			TocaSuelo(origen1, choque); // si un rayo partiendo desde el origen en la direccion indicada,realiza un choque(true o false) y la distancia de ese choque es la limitada por mi sensibilidad
		else if (Physics.Raycast(origen2, direccion, out choque) && (choque.distance < alturaExtra + sensibilidad))
			TocaSuelo(origen2, choque); // otra comprobacion diferente
		else 
		{ // Si no hemos golpeado nada estaremos en el aire
			esta_en_el_suelo = false;
			direccionSuelo = Vector3.right;
		}
	}
	
	void TocaSuelo(Vector3 origen, RaycastHit choque)
	{
		// Calculo el angulo en el que estaremos encima del suelo (si estamos puestos bien o con opcion a caer)
		direccionSuelo = new Vector3(choque.normal.y, -choque.normal.x, 0);
		float anguloRespectoElSuelo = Vector3.Angle(direccionSuelo, new Vector3(direccionSuelo.x, 0, 0));

		// Chequeo si puedo caminar en este angulo
		if (anguloRespectoElSuelo <= max_angulo_escalar)
		{
			esta_en_el_suelo = true;
		}	
		return;
	}
	
	void LimitesPersonaje() // Calcula los limites de la caja que contiene el personaje
	{
		personajeAltura = collider.bounds.size.y;  // cojo tamaño del limite de la y
		personajeAnchura = collider.bounds.size.x; // cojo tamaño del limite de la x
	}
	
}
