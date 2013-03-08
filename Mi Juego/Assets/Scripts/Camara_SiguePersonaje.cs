using UnityEngine;
using System.Collections;

public class Camara_SiguePersonaje : MonoBehaviour {
	
	public Transform player;
	float z;
	float y;
	float x;
	
	// Use this for initialization
	void Start () {
		z=10;
		y=7;
		x=4;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 playerPos = player.position; // cojo la posicion del player
		playerPos.z = -z;
		playerPos.y = playerPos.y + y;
		playerPos.x = playerPos.x + x; // a partir de esta modifico la posicion del vector
		transform.position -= (transform.position - playerPos); // actualizo posicion
	}
}
