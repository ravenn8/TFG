using UnityEngine;
using System.Collections;

public class Ataque : MonoBehaviour {

	int TheDammage = 50;
	public float Distance;
	public float MaxDistance = 3.0f;
	public Transform TheSystem;
	public AudioClip punch;
	private bool mano = false;
	private bool tieneControl = true;
	
	void Update ()
	{
		if(tieneControl){
			if (Input.GetButtonDown("Fire1")) 
			{
				//Attack animation
				print("estoy aqui");
				if(mano){
					animation.Play ("accionPIzq");
					mano = !mano;
				}else{
					animation.Play ("accionPDer");
					mano = !mano;
				}
				audio.PlayOneShot(punch);
				AttackDammage();
				StartCoroutine(dejaGolpear());
			}
		}
	}
	
	public void AttackDammage ()
	{
			//Attack function
			RaycastHit hit;
			Debug.Log("AQUI");

			
			if (Physics.Raycast (TheSystem.transform.position, TheSystem.transform.TransformDirection(Vector3.forward), out hit)){

				Distance = hit.distance;
				if (Distance < MaxDistance)
				{
					Debug.Log("ATACO");
					hit.transform.SendMessage("ApplyDammage", TheDammage, SendMessageOptions.DontRequireReceiver);
				}
			}
	
			if (Physics.Raycast (TheSystem.transform.position, TheSystem.transform.TransformDirection(Vector3.back), out hit)){
				Distance = hit.distance;
				if (Distance < MaxDistance)
				{
					Debug.Log("ATACO");
					hit.transform.SendMessage("ApplyDammage", TheDammage, SendMessageOptions.DontRequireReceiver);
				}
			}
	}
	
	IEnumerator dejaGolpear(){

		yield return new WaitForSeconds(0.8f);
		animation.Play ("accionCamino");
		
	}
	
	public void DaControl() { tieneControl = true; }
	public void QuitaControl() { tieneControl = false; }
	
}
