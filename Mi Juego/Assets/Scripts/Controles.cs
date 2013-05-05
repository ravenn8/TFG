using UnityEngine;
using System.Collections;

public class Controles : MonoBehaviour
{
	Fisicas fisicasPersonaje;
	bool tieneControl;

	void Start () 
	{
		tieneControl = true;
		fisicasPersonaje = GetComponent<Fisicas>();
		if (fisicasPersonaje == null)
			Debug.LogError("This object also needs a PlatformerPhysics component attached for the controller to function properly");
	}

	void Update () 
	{
		if (fisicasPersonaje && tieneControl)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
				fisicasPersonaje.EmpiezaSprint();

			if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
				fisicasPersonaje.AcabaSprint();

			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
				fisicasPersonaje.Agacharse();

			if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
				fisicasPersonaje.DejaAgacharse();
		}
	}

	void FixedUpdate()
	{
		if (fisicasPersonaje && tieneControl)
		{
			if (Input.GetButton("Jump"))
				fisicasPersonaje.Salto();

			fisicasPersonaje.Andar(Input.GetAxisRaw("Horizontal"));
		}
	}

	public void DaControl() { tieneControl = true; }
	public void QuitaControl() { tieneControl = false; }
	public bool EnControl() { return tieneControl; }
}

