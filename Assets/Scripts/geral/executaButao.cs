using UnityEngine;
using System.Collections;

public class executaButao : MonoBehaviour {

	public AudioSource clica;
	// Use this for initialization
	public void executa(){
		clica.Play ();
	}
}
