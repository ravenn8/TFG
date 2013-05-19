using UnityEngine;
using System.Collections;

public class Vida : MonoBehaviour {

	public int Health = 100;
	IAComplicada ia;
	public GUIStyle estilo;
	
	void Start(){
		ia = GetComponent<IAComplicada>();
	}
	void ApplyDammage (int TheDammage)
	{
		Health -= TheDammage;
		if(Health > 0){
			animation.Play ("gethit");
		}
		Debug.Log(Health);
		if(Health <= 0)
		{
			StartCoroutine(Dead ());
		}
	}

	//void Dead()
	IEnumerator Dead()
	{
		animation.Play("die");
		ia.die();
		yield return new WaitForSeconds(1.5f);
		Destroy (gameObject);
	}
	
	void OnGUI(){
		if(Health<100){
			if(Health < 0){
				GUI.Label(new Rect(400,50,Screen.height,Screen.width), "Vida enemigo "+ "0" + " de 100",estilo);
			}else{
				GUI.Label(new Rect(400,50,Screen.height,Screen.width), "Vida enemigo "+ Health+ " de 100",estilo);
			}
		}
	}
}
