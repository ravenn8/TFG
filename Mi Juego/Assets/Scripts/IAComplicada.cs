using UnityEngine;
using System.Collections;

public class IAComplicada : MonoBehaviour {

	public float Distancia;
	public Transform Objetivo;
	public float rangoAtaque = 4.0f;
	public float velMovimiento = 5.0f;
	private Vector3 velocidad = Vector3.zero;
	public CharacterController controlador;
	public float rangoVista = 15.0f;
	private float tiempoRepeticionAtaque = 2.5f;
	private int elDanyo = 40;
	private float tiempoAtaque;
	public bool estaMuerto = false;
	public bool primeraVez = true;
	public AudioClip sonidoGrito;
	
	
	void Start () {
	}
	// Update is called once per frame
	void Update () {
	
	if(!estaMuerto){	
		Distancia = Vector3.Distance(Objetivo.position, transform.position);
		
		if (Distancia < rangoAtaque)
		{
			ataque ();
		}else if (Distancia < rangoVista)
		{
			if(primeraVez){
				audio.PlayOneShot(sonidoGrito);
			    primeraVez = false;
			}
			siguePersonaje ();
		}
	}		
	}
	
	void ataque ()
	{
		if (Time.time > tiempoAtaque)
	{
		animation.Play("attack");
		Objetivo.SendMessage("ApplyDammage", elDanyo);
		Debug.Log("El enemigo ha atacado");
		tiempoAtaque = Time.time + tiempoRepeticionAtaque;
	}
	}
	
	void mueveIzquierda(){
		velocidad.x = -velMovimiento * Time.deltaTime;
		controlador.Move(velocidad);
		Vector3 localScale = controlador.transform.localScale;
		localScale.z = Mathf.Abs(localScale.z);
		controlador.transform.localScale = localScale;
		
		
		animation.Play("run");
	}
	
	void mueveDerecha(){
		velocidad.x = velMovimiento * Time.deltaTime;
		controlador.Move(velocidad);
		Vector3 localScale = controlador.transform.localScale;
		localScale.z = -Mathf.Abs(localScale.z);
		controlador.transform.localScale = localScale;
		
		
		animation.Play("run");
	}
	
	public void die(){
		estaMuerto = true;
	}
	
	
	void siguePersonaje(){
		
		if(transform.position.x <= Objetivo.position.x)
		{
			mueveDerecha();
		}
		
		if(transform.position.x > Objetivo.position.x)
		{
			mueveIzquierda ();
		}
	}
	
}