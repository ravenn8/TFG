using UnityEngine;
using System.Collections;

public class IASimple : MonoBehaviour {

	public float Distancia;
	public Transform Target;
	public float rangoVista = 25.0f;
	public float rangoAtaque = 15.0f;
	public float velMovimiento = 5.0f;
	public float amortiguacion = 6.0f;
	
	// Update is called once per frame
	void Update () {
		Distancia = Vector3.Distance(Target.position, transform.position);
	
	if (Distancia < rangoVista)
	{
		//renderer.material.color = Color.yellow;
		mirarA();
	}
	
	if (Distancia > rangoVista)
	{
		//renderer.material.color = Color.green;
	}
	
	if (Distancia < rangoAtaque)
	{
		//renderer.material.color = Color.red;
		ataque ();
	}
		
	}
	
	void mirarA ()
	{
		var rotation = Quaternion.LookRotation(Target.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * amortiguacion);
	}
	
	void ataque ()
	{
		transform.Translate(Vector3.forward * velMovimiento * Time.deltaTime);
	}
	
	
}