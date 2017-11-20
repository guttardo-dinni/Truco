using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
//using System;
using System.Collections.Generic;
using System.Threading;

public class controlaJogo : MonoBehaviour {

	public Image[] jogador;
	public Sprite[] jogador_normal;
	public Sprite[] jogador_ativo;
	public Image[] posicao;
	public Image[] mao;
	public Sprite transp;
	public NetworkView nView;
	public Button[] carta;
	public Sprite[] sprs;
	public int[] valor;
	public struct obj{
		public Sprite img;
		public int peso;
	}
	public obj[] cards = new obj[40];

	public Text timeA, timeB, win;
	private bool manda = false, ganhou = false, espera;
	private int[] sorteio = new int[12];
	private bool[] c = new bool[3];
	private int vez = 1, id, maior, cont, ganhando, pa, pb, ga, gb, comecou;
	private string msg;
	private float tempo = 3.0f;

	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		pa = pb = ga = gb = 0;
		c [0] = c [1] = c [2] = false;
		espera = false;
		id = PlayerPrefs.GetInt("id");
		for (int i = 0; i < 40; i++) {
			cards [i].img = sprs [i];
			cards [i].peso = valor[i];
		}
		if(Network.isServer)
			comecaRodada ();
	}
		
	void Update () {
		if (espera) {
			tempo -= Time.deltaTime;
			if (tempo <= 0) {
				if (ganhando == 1 || ganhando == 3) {
					pa ++;
				} else {
					pb ++;
				}
				if (cont == 12) {
					if (pa > pb) {
						ga += 2;
						if (ga == 12) {
							win.text = "TimeA ganhou!";
							ganhou = true;
						}
						if (ga < 10) {
							timeA.text = "0" + ga.ToString ();
						} else {
							timeA.text = ga.ToString ();
						}
					} else {
						gb += 2;
						if (gb == 12) {
							win.text = "TimeB ganhou!";
							ganhou = true;
						}
						if (gb < 10) {
							timeB.text = "0" + gb.ToString ();
						} else {
							timeB.text = gb.ToString ();
						}
					}
					manda = true;
					pa = pb = ga = gb = 0;
				}
				for (int i = 0; i < 4; i++)
					posicao [i].sprite = transp;
				tempo = 3.0f;
				espera = false;
			}
		}
		if (manda && !ganhou && Network.isServer) {
			comecaRodada ();
		}
	}

	void comecaRodada(){
		maior = 0;
		embaralha ();
		for (int i = 0; i < 12; i++) {
			if (sorteio [i] < 10)
				msg = msg + sorteio [i].ToString () + " ";
			else
				msg = msg + sorteio [i].ToString ();
		}
		SendComecoToClient ();
		c [0] = c [1] = c [2] = false;
		cont = 0;
		vez = (comecou%4)+1;
		comecou = vez;
		atualizaMesa ();
		for(int i=0; i<3; i++)
			mao [i].sprite = cards [sorteio [i]].img;
	}

	void embaralha(){
		
		List<int> numeros = new List<int>();

		for (int i = 0; i < 40; i++)
			numeros.Add (i);

		for (int i = 0; i < 28; i++) {
			numeros.RemoveAt(Random.Range(0,numeros.Count));
		}
		int aux, n1, n2;
		for (int i = 0; i < 100; i++) {
			n1 = Random.Range (0, 11);
			n2 = Random.Range (0, 11);
			aux = numeros [n1];
			numeros [n1] = numeros [n2];
			numeros [n2] = aux;
		}
		for (int i = 0; i < 12; i++) {
			sorteio [i] = numeros [i];
		}
	}
		
	public void click(int i){
		if (vez == id && !c [i]) {
			c [i] = true;
			int num = (3 * (id - 1)+i);
			msg = num.ToString();
			SendJogouToOthers ();
			posicao [0].sprite = mao [i].sprite;
			if (cards [sorteio [num]].peso>maior) {
				maior=cards [sorteio [num]].peso;
				ganhando = id;
			}
			mao [i].sprite = transp;

			vez = (vez % 4)+1;
			atualizaMesa ();
		}
	}

	void atualizaMesa(){
		jogador [(4-(id-vez))%4].sprite = jogador_ativo [vez - 1];
		for(int i=1; i<=3; i++)
			jogador [(4-(id-vez)+i)%4].sprite = jogador_normal[(vez+i-1)%4];
	}

	[RPC]
	void SendJogouToOthers (){
		cont++;
		if (cont % 4 == 0)
			espera = true;
		nView.RPC("ReceiveJogou", RPCMode.OthersBuffered, msg);
	}

	[RPC]
	void ReceiveJogou (string info){
		cont++;
		if (cont % 4 == 0)
			espera = true;
		int num = int.Parse (info);
		posicao [(4 -(id - vez))%4].sprite = cards [sorteio [num]].img;
		if (cards [sorteio [num]].peso>maior) {
			maior = cards [sorteio [num]].peso;
			ganhando = (num / 3)+1;
		}
		vez = (vez % 4)+1;
		atualizaMesa ();
	}
		
	[RPC]
	void SendComecoToClient (){
		nView.RPC("ReceiveComecoFromServer", RPCMode.OthersBuffered, msg);
	}

	[RPC]
	void ReceiveComecoFromServer(string info){
		c [0] = c [1] = c [2] = false;
		cont = 0;
		maior = 0;
		vez = (comecou%4)+1;
		comecou = vez;
		atualizaMesa ();
		for (int i = 0, j=0; i < 24; i += 2,j++) {
			sorteio [j] = int.Parse (info.Substring (i, 2));
		}
		for (int i = 0; i < 3; i++) {
			mao[i].sprite = cards[sorteio[3*(id-1)+i]].img;
		}
	}
		

	[RPC]
	void SendInfoToClient() {
		nView.RPC("ReceiveInfoFromServer", RPCMode.OthersBuffered, msg);
	}

	[RPC]
	void ReceiveInfoFromClient(string info) {

	}

	[RPC]
	void SendInfoToServer(){
		nView.RPC("ReceiveInfoFromClient", RPCMode.Server, msg);
	}


	[RPC]
	void ReceiveInfoFromServer(string info) {
	}
}
