using UnityEngine;
using System.Collections;

public class Animacion : MonoBehaviour
{
	public Transform animacionPersonaje; // Modelo animado donde tenemos las animaciones
	bool PersonajeMuerto = false;
    bool mIdle = false;

	public AudioClip sonidoSalto;
	public AudioClip sonidoMuerte;

	void Start () 
	{
		if (animacionPersonaje == null)
		{
			Debug.LogError("The animated player model is not set.");
			this.enabled = false;
		}
		else if (!ChequeaAnimaciones())
		{
			Debug.LogError("The animated player model does not seem to have the appropriate animations needed.");
			this.enabled = false;
		}
		else
		{
			animacionPersonaje.animation["accionIdle"].speed = 0;
		}
	}

	bool ChequeaAnimaciones()
	{
		if (!animacionPersonaje)
			return false;

		if (animacionPersonaje.animation["accionCamino"] == null ||
			animacionPersonaje.animation["accionSalta"] == null ||
			animacionPersonaje.animation["accionAgache"] == null ||
			animacionPersonaje.animation["accionMuerte"] == null ||
			animacionPersonaje.animation["accionAgarre"] == null ||
			animacionPersonaje.animation["accionCorrer"] == null ||
			animacionPersonaje.animation["accionPIzq"] == null ||
			animacionPersonaje.animation["accionPDer"] == null ||
			animacionPersonaje.animation["accionIdle"] == null) return false;

		return true;
	}

	void Update () 
	{

		float walkingSpeed = Mathf.Abs(rigidbody.velocity.x)*0.075f;
		animacionPersonaje.animation["accionCamino"].speed = walkingSpeed;


		if (walkingSpeed == 0 && animacionPersonaje.animation["accionCamino"].enabled)
		{
			animacionPersonaje.animation.Play("accionIdle");
            mIdle = true;
		}

        if (walkingSpeed > 0.01f && mIdle)
		{
            mIdle = false;
			animacionPersonaje.animation.CrossFade("accionCamino");
			
			
		}
	}

	void PlayAnim(string animName)
	{
		if (!PersonajeMuerto)
		{
			animacionPersonaje.animation.Play(animName);
		}
	}

	void MueveIzquierda()
	{
		Vector3 localScale = animacionPersonaje.transform.localScale;
		localScale.z = -Mathf.Abs(localScale.z);
		animacionPersonaje.transform.localScale = localScale;
	}

	void MueveDerecha()
	{
		Vector3 localScale = animacionPersonaje.transform.localScale;
		localScale.z = Mathf.Abs(localScale.z);
		animacionPersonaje.transform.localScale = localScale;
	}

	public void Muere()
	{
        PlayAnim("accionMuerte");
		audio.PlayOneShot(sonidoMuerte);
		PersonajeMuerto = true;
	}

	public void Vive()
	{
		MueveDerecha();
		PersonajeMuerto = false;
        PlayAnim("accionCamino");
	}



	void AnimacionSalto()
	{
        PlayAnim("accionSalta");
		audio.PlayOneShot(sonidoSalto);
	}

	void AnimacionSaltoMuro()
	{
        PlayAnim("accionSalta");
		audio.PlayOneShot(sonidoSalto);
	}

	void EmpiezaAgacharse()
	{
        PlayAnim("accionAgache");
	}

	void AcabaAgacharse()
	{
        //PlayAnim("accionAgache");

		if (GetComponent<Fisicas>().EstaEnMuro())
			EstaEnMuro();
		else
			animacionPersonaje.animation.CrossFade("accionCamino", 0.5f);
	}

	void EstaEnSuelo()
	{
		if (!GetComponent<Fisicas>().EstaAgachado())
		{
            PlayAnim("accionCamino");
		}
	}

	void EstaEnMuro()
	{
        if (!GetComponent<Fisicas>().EstaAgachado())
        {
            PlayAnim("accionAgarre");

            if (!GetComponent<Fisicas>().EstaMuroCorrecto())
            {
                MueveIzquierda();
            }
            else
            {
                MueveDerecha();
            }
        }
	}

	void DejaMuro()
	{
		print("se suelta");
		if (!animacionPersonaje.animation["accionSalta"].enabled && !GetComponent<Fisicas>().EstaAgachado())
            PlayAnim("accionCamino");
	}

	void DaSprint()
	{
		PlayAnim("accionCorrer");
	}

	void ParaSprint()
	{
		PlayAnim("accionCamino");
	}
	
}
