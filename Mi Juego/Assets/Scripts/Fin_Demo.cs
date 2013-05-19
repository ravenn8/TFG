using UnityEngine;
using System.Collections;

public class Fin_Demo : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		Application.LoadLevel("ganador");
	}
}