using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class atualizaJogadores : MonoBehaviour {

	//Images e Sprites para atualizar quando um novo jogador entrar
	public Image jogador1;
	public Image jogador2;
	public Image jogador3;
	public Image jogador4;
	public Image aguardando;
	public Sprite jog1;
	public Sprite jog2;
	public Sprite jog3;
	public Sprite jog4;
	public Sprite pronto;

	private bool acabou = false;
	private float tempo = 5.0f;
	private bool registrado = false;

	//Comunização entre clientes e servidor
	private string msg;
	public NetworkView nView;

	//Contador para atualizar sprite só uma vez
	private int cont=0, aux=0;

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		if(Network.isServer){
			cont = 1;
			PlayerPrefs.SetInt ("id", 1);
			PlayerPrefs.Save ();
		}
	}
		

	// Update is called once per frame
	void Update () {

		if (acabou) {
			tempo -= Time.deltaTime;

			if (tempo <= 0)
				SceneManager.LoadScene ("jogo", LoadSceneMode.Single);

		}

		if (cont != aux && !acabou) {
			aux = cont;
			if (cont == 1) {
				jogador1.sprite = jog1;
			} else if (cont == 2) {
				jogador1.sprite = jog1;
				jogador2.sprite = jog2;
			} else if (cont == 3) {
				jogador1.sprite = jog1;
				jogador2.sprite = jog2;
				jogador3.sprite = jog3;
			} else if (cont == 4) {
				jogador1.sprite = jog1;
				jogador2.sprite = jog2;
				jogador3.sprite = jog3;
				jogador4.sprite = jog4;
				aguardando.sprite = pronto;
				acabou=true;
			} 
				
		}

	}
		
	[RPC]
	void ReceiveInfoFromClient(string info) {
		if (info == "Cliente Conectado") {
			cont++;
			msg = cont.ToString();

			SendInfoToClient ();
		}
			
	}


	[RPC]
	void SendInfoToClient() {
		nView.RPC("ReceiveInfoFromServer", RPCMode.OthersBuffered, msg);
	}

	[RPC]
	void SendInfoToServer(){
		nView.RPC("ReceiveInfoFromClient", RPCMode.Server, msg);
	}
		

	[RPC]
	void ReceiveInfoFromServer(string info) {
		cont = int.Parse (info);
		if (!registrado) {
			registrado = true;
			PlayerPrefs.SetInt ("id", cont);
			PlayerPrefs.Save ();
		}
	}
		
}
