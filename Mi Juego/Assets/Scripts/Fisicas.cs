using UnityEngine;
using System.Collections;

public class Fisicas : MonoBehaviour
{
	
	// Variables de movimiento
	public float velocidadAndando			= 35;		// Velocidad caminando
	public float velocidadCorriendo			= 60;		// Velocidad esprintando
	public float velocidadAndandoMaxima		= 15;		// Velocidad caminando maxima
	public float velocidadCorriendoMaxima	= 20;		// Velocidad corriendo maxima
	public float movimientoFriccion			= 0.9f;		// Friccion del personaje en el suelo, si el usuario no pulsa
	public float velocidadDeParada			= 5.0f;		// Velocidad de parada
	public float friccionDelAire			= 0.98f;	// Coeficiente de friccion en el aire
	public float AnguloMaximoAndar			= 30.0f;	// Angulo maximo para caminar
	public float agacharseCollider			= 0.5f;		// Transformador de collider al agacharse
	public float aceleracionAgachado		= 0.1f;		// Velocidad maxima agachado

	// Variables de salto
	public float velocidadSalto			= 12;			// Velocidad de salto
	public int framesSalto				= 15;			// Frames para el salto
	public float gravedadAgachado		= 20;			// Gravedad si se esta agachado
	public bool dobleSalto				= false;		// Si puede dar doble salto o no el personaje
	public bool saltoMuro				= true;			// Si puede saltar en Muro
	public float saltoMuroVelocidad		= 15;			// Velocidad del salto en el Muro
	public float muroNumeroEnganche		= 0.5f;			// Tiempo que se aguanta el usuario hasta empezar a bajar en un muro
	public float multiplicadorGravedad	= 3.5f;			// Coeficiente de gravedad que se le aplica al usuario



	// Variables privadas
	bool fEnSuelo						= false;		// Estamos en el suelo
	bool fSprint						= false;		// Estamos en el esprintando
	bool fAgacharse						= false;		// Estamos en el agachado
	bool fIntentaAgacharse				= false;		// Salimos de estar agachado
	Vector3 fDireccionSuelo				= Vector3.right;// En la direccion que estamos con respecto el suelo
	bool fEnSalto						= false;		// Estamos en medio de un salto
	bool fSaltarApretado				= false;		// Sigue el boton de salto apretado
	bool fSegundoSaltoRestante			= true;			// Nos queda el segundo salto
	int fFramesSaltoRestantes			= 0;			// Frames para mantener el salto
	bool fEnMuro						= false;		// Estamos en un muro
	bool fMuroCorrecto					= false;		// Esta el muro en el sitio correcto
	float fMuroTiempoBajar				= 0;			// Tiempo para empezar a caer en el muro
	float fFuerzaFrenado				= 0;			// Fuerza de frenado
	bool fVamosBien						= true;			// Vamos hacia la derecha
	float fPersonajeAlto;								// Caja altura personaje
	float fPersonajeAncho;								// Caja anchura personaje
	Vector3 fPosicionInicial;							// Posicion para resucitar
	float colliderCentroY;								// Tamanyo de la caja de colisiones
	float colliderTamanyoY;

	public void Start () 
	{
		fPosicionInicial = transform.position;
		LimitesPersonaje();
		colliderCentroY = ((BoxCollider)collider).center.y;
		colliderTamanyoY = ((BoxCollider)collider).size.y;
	}

	public void Reset() // Resetea variables privadas
	{
		fEnSuelo = false;
		fSprint = false;
		ParaAgacharse();
		fDireccionSuelo = Vector3.right;
		fEnSalto = false;
		fSaltarApretado = false;
		fSegundoSaltoRestante = true;
		fFramesSaltoRestantes = 0;
		fEnMuro = false;
		fMuroCorrecto = false;
		fFuerzaFrenado = 0;
		transform.position = fPosicionInicial;
		rigidbody.velocity = Vector3.zero;
		fVamosBien = true;
	}



	void FixedUpdate () 
	{
		if(saltoMuro)
			ActualizaMuro();		// Mira si estamos en muro
		ActualizaSuelo();			// Mira si estamos en el suelo

		ActualizaSalto();
		ActualizaAgacharse();
		AplicaGravedad();
		AplicaFriccion();
	}


	public void Andar(float direction) 
	{
		
		if (fEnMuro && fMuroTiempoBajar > 0)
		{
			if ((fMuroCorrecto && direction < 0) ||
				(!fMuroCorrecto && direction > 0))
			{
				fMuroTiempoBajar -= Time.fixedDeltaTime;
			}

			if (fMuroTiempoBajar <= 0)
			{
				EjecutaAnimacion("DejaMuro");
			}

			return;
		}

		// cantidad de velocidad
		float accel = velocidadAndando;
		if (fSprint)
			accel = velocidadCorriendo;
		if (fAgacharse && fEnSuelo)
			accel = velocidadAndando * aceleracionAgachado;

        // aplicar fuerza actual
		rigidbody.AddForce(fDireccionSuelo * direction * accel, ForceMode.Acceleration);

		fFuerzaFrenado = 1 - Mathf.Abs(direction);

		if (direction < 0 && fVamosBien)
		{
			fVamosBien = false;
			EjecutaAnimacion("MueveIzquierda");
		}
		if (direction > 0 && !fVamosBien)
		{
			fVamosBien = true;
			EjecutaAnimacion("MueveDerecha");
		}
	}


	public void Salto() 
	{
		fSaltarApretado = true;

		// Mira si podemos iniciar un salto
		if (fFramesSaltoRestantes == 0 && !fEnSalto && !fAgacharse)
		{
			if (!fEnSuelo && fSegundoSaltoRestante && dobleSalto) // Segundo salto
			{
				fSegundoSaltoRestante = false;

				fFramesSaltoRestantes = framesSalto;
				fEnSalto = true;

                EjecutaAnimacion("AnimacionSalto");
			}

			if (fEnSuelo || fEnMuro) // Primer salto
			{
				fSegundoSaltoRestante = true;

				fFramesSaltoRestantes = framesSalto;
				fEnSalto = true;

				if (fEnMuro) // Velocidad extra en saltos de muro
				{
					if (fMuroCorrecto)
						rigidbody.velocity += saltoMuroVelocidad * Vector3.left;
					else
						rigidbody.velocity += saltoMuroVelocidad * Vector3.right;

                    EjecutaAnimacion("AnimacionSaltoMuro");
				}
				else
				{
                    EjecutaAnimacion("AnimacionSalto");
				}
			}
		}

		// Mira si estamos en medio de un salto
		if(fFramesSaltoRestantes != 0)
		{
			Vector3 vel = rigidbody.velocity;
			vel.y = velocidadSalto;
			rigidbody.velocity = vel;
		}
	}


	public void Agacharse() 
	{
		if (!fAgacharse) // Mira si no estamos agachados ya
		{
			fAgacharse = true;

			AgacharseCollider();

			LimitesPersonaje();

			EjecutaAnimacion("EmpiezaAgacharse");
		}
	}

	public void AgacharseCollider()
	{		
		BoxCollider myCollider = (BoxCollider)collider;

		Vector3 center = myCollider.center;
		Vector3 size = myCollider.size;

		// ajustar el centro y tamaño al estar arrastrandose por el suelo
		size.y = colliderTamanyoY * agacharseCollider;
		center.y = colliderCentroY - (colliderTamanyoY * (1.0f - agacharseCollider))*0.5f;

		myCollider.size = size;
		myCollider.center = center;
	}

	public void DejaAgacharse()
	{
		fIntentaAgacharse = true;
	}

	void ParaAgacharse() 
	{
		fIntentaAgacharse = false;
		fAgacharse = false;
		DejaAgacharseCollider();
		LimitesPersonaje();
		EjecutaAnimacion("AcabaAgacharse");
	}

	public void DejaAgacharseCollider()
	{
		BoxCollider myCollider = (BoxCollider)collider;

		Vector3 center = myCollider.center;
		Vector3 size = myCollider.size;

		size.y = colliderTamanyoY;
		center.y = colliderCentroY;

		myCollider.size = size;
		myCollider.center = center;
	}

	public void EmpiezaSprint() 
	{
		fSprint = true;
        EjecutaAnimacion("DaSprint");
	}

	public void AcabaSprint() 
	{
		fSprint = false;
        EjecutaAnimacion("ParaSprint");
	}


	void AplicaGravedad()
	{
		if (!fEnSuelo) 
		{
			rigidbody.AddForce(Physics.gravity * multiplicadorGravedad, ForceMode.Acceleration);
		}

		if (fAgacharse) 
		{
			rigidbody.AddForce(Vector3.down * gravedadAgachado, ForceMode.Acceleration);
		}
	}

	void ActualizaAgacharse()
	{
		if (fIntentaAgacharse && PuedeDejarAgacharse())
		{
			ParaAgacharse();
		}
	}


	void ActualizaSalto()
	{
		if (!fSaltarApretado && fEnSalto) 
		{
			fFramesSaltoRestantes = 0;
			fEnSalto = false;
		}
		fSaltarApretado = false;

		if (fFramesSaltoRestantes != 0)
			fFramesSaltoRestantes--;
	}

	void AplicaFriccion()
	{
		Vector3 velocity = rigidbody.velocity;

		// Friccion del suelo
		if (fEnSuelo && fFuerzaFrenado > 0.0f)
		{
			Vector3 velocityInGroundDir = Vector3.Dot(velocity, fDireccionSuelo) * fDireccionSuelo; //project velocity on ground direction
			Vector3 newVelocityInGroundDir = velocityInGroundDir * Mathf.Lerp(1.0f, movimientoFriccion, fFuerzaFrenado); //apply ground friction on velocity
			velocity -= (velocityInGroundDir - newVelocityInGroundDir); //apply to actual velocity
		}

		// Friccion del aire
		velocity *= friccionDelAire;

		float absSpeed = Mathf.Abs(velocity.x);

		// Velocidad maxima
		float maxSpeed = velocidadAndandoMaxima;
		if (fSprint)
			maxSpeed = velocidadCorriendoMaxima;

		if (absSpeed > maxSpeed)
			velocity.x *= maxSpeed / absSpeed;

		// Velocidad minima
		if (absSpeed < velocidadDeParada && fFuerzaFrenado == 1.0f)
			velocity.x = 0;

		// Velocidad final
		rigidbody.velocity = velocity;

		fFuerzaFrenado = 1.0f; //if no walking is done this frame, the character will start stopping next frame
	}


	void ActualizaSuelo()
	{
		
		float epsilon = 0.05f; 
		float extraHeight = fPersonajeAlto * 0.75f;
		float halfPlayerWidth = fPersonajeAncho * 0.49f;

		// Origenes de los rayos
		Vector3 origin1 = ConsigueCentroInferior() + Vector3.right * halfPlayerWidth + Vector3.up * extraHeight;
		Vector3 origin2 = ConsigueCentroInferior() + Vector3.left * halfPlayerWidth + Vector3.up * extraHeight;
		Vector3 direction = Vector3.down;
		RaycastHit hit;

		// Trazos de los rayos
		if (Physics.Raycast(origin1, direction, out hit) && (hit.distance < extraHeight + epsilon))
			TocaSuelo(origin1, hit);
		else if (Physics.Raycast(origin2, direction, out hit) && (hit.distance < extraHeight + epsilon))
			TocaSuelo(origin2, hit);
		else
		{
			fEnSuelo = false; // Si no tocamos nada estamos en el aire
			fDireccionSuelo = Vector3.right;
		}
	}

	void TocaSuelo(Vector3 origin, RaycastHit hit)
	{
		
		fDireccionSuelo = new Vector3(hit.normal.y, -hit.normal.x, 0);
		float groundAngle = Vector3.Angle(fDireccionSuelo, new Vector3(fDireccionSuelo.x, 0, 0));


		if (groundAngle <= AnguloMaximoAndar)
		{
			if(!fEnSuelo)
                EjecutaAnimacion("EstaEnSuelo");

			Debug.DrawLine(hit.point+Vector3.up, hit.point, Color.green);
			Debug.DrawLine(hit.point, hit.point + fDireccionSuelo, Color.magenta);
			fEnSuelo = true;
			fEnMuro = false;
		}
		else
		{
			Debug.DrawLine(hit.point, hit.point + fDireccionSuelo, Color.grey);
		}
	
		return;
	}


	void ActualizaMuro()
	{

		float epsilon = 0.05f;
		float halfPlayerWidth = fPersonajeAncho * 0.5f;

		Vector3 origin = ConsigueCentroInferior() + Vector3.up * fPersonajeAlto * 0.5f;
		RaycastHit hit;

		if (Physics.Raycast(origin, Vector3.right, out hit))
		{
			if (hit.distance < halfPlayerWidth + epsilon && !fEnSuelo)
			{
				transform.position += Vector3.left * (halfPlayerWidth - hit.distance);

				TocaMuro(true);
				Debug.DrawLine(origin, hit.point, Color.yellow);
				return;
			}
		}

		if (Physics.Raycast(origin, Vector3.left, out hit))
		{
			if (hit.distance < halfPlayerWidth + epsilon && !fEnSuelo)
			{
				transform.position += Vector3.right * (halfPlayerWidth - hit.distance);

				TocaMuro(false);
				Debug.DrawLine(origin, hit.point, Color.yellow);
				return;
			}
		}

		if (fEnMuro)
		{
            EjecutaAnimacion("DejaMuro");
		}

		fMuroTiempoBajar = 0;
		fEnMuro = false;
	}

	void TocaMuro(bool onRightSide)
	{
		fMuroCorrecto = onRightSide;
		fVamosBien = fMuroCorrecto;

		if (!fEnMuro)
		{
			rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0); //Remove horizontal speed
			fMuroTiempoBajar = muroNumeroEnganche;
			fEnMuro = true;
            EjecutaAnimacion("EstaEnMuro");
		}

		fEnMuro = true;
	}

	bool PuedeDejarAgacharse()
	{
		float epsilon = 0.05f; 
		float origCharHeight = colliderTamanyoY;
		float extraHeight = origCharHeight * 0.75f;
		float halfPlayerWidth = fPersonajeAncho * 0.49f;

		Vector3 origin1 = ConsigueCentroInferior() + Vector3.right * halfPlayerWidth + Vector3.up * (origCharHeight - extraHeight);
		Vector3 origin2 = ConsigueCentroInferior() + Vector3.left * halfPlayerWidth + Vector3.up * (origCharHeight - extraHeight);
		Vector3 direction = Vector3.up;
		RaycastHit hit;

		bool canUncrouch = true;

		if (Physics.Raycast(origin1, direction, out hit) && (hit.distance < extraHeight + epsilon))
			canUncrouch = false;
		else if (Physics.Raycast(origin2, direction, out hit) && (hit.distance < extraHeight + epsilon))
			canUncrouch = false;

		return canUncrouch;
	}

    void EjecutaAnimacion(string message)
    {
        SendMessage(message, SendMessageOptions.DontRequireReceiver);
    }

	void LimitesPersonaje()
	{
		fPersonajeAlto = collider.bounds.size.y;
		fPersonajeAncho = collider.bounds.size.x;
	}

	public void Checkpoint(Vector3 spawnPoint)
	{
		fPosicionInicial = spawnPoint;
	}

	public Vector3 ConsigueCentroInferior()
	{
		return collider.bounds.center+collider.bounds.extents.y*Vector3.down;
	}
	
	// funciones get
	public bool EstaMuroCorrecto() { return fMuroCorrecto; }
	public bool EstaAgachado() { return fAgacharse; }
	public bool EstaEnMuro() { return fEnMuro; }
	public bool EstaEnSuelo() { return fEnSuelo; }
	public bool EstaEnSprint() { return fSprint; }
}
