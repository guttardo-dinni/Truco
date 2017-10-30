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

	public Image Principal;
	public Image Parceiro;
	public Image advDireito;
	public Image advEsquerdo;

	public Sprite[] jogador_normal;
	public Sprite[] jogador_ativo;

	public Image[] posicao;
	public Image[] mao;
	public Sprite transp;


	private string msg;
	public NetworkView nView;

	public Button[] carta;
	private bool c1 = false, c2 = false, c3 = false;
	private int vez = 1;
	private int comecou = 0;
	private int id;

	public Sprite[] sprs;
	public int[] valor;

	private int p1, p2, pv;

	public struct obj{
		public Sprite img;
		public int peso;
	}

	public obj[] cards = new obj[40];


	private int[] sorteio = new int[12];

	private int cont;
	/*Server Padrão
	


	*/
	// Use this for initialization
	void Start () {
		p1 = 0;
		p2 = 0;
		pv = 2;
		id = PlayerPrefs.GetInt("id");
		for (int i = 0; i < 40; i++) {
			cards [i].img = sprs [i];
			cards [i].peso = valor[i];
		}
		if(Network.isServer)
			comecaRodada ();
	}


	// Update is called once per frame
	void Update () {
		
	}

	void comecaRodada(){
		embaralha ();
		c1 = false;
		c2 = false;
		c3 = false;
		cont = 0;
		for(int i=0; i<3; i++)
			mao [i].sprite = cards [sorteio [i]].img;
		for (int i = 0; i < 12; i++) {
			if (sorteio [i] < 10)
				msg = msg + sorteio [i].ToString () + " ";
			else
				msg = msg + sorteio [i].ToString ();
		}
		vez = (comecou)%4+1;
		comecou = vez;
		SendComecoToClient ();
		atualizaMesa ();
		print ("Rodada Iniciada com sucesso!");
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
		

	public void clicou_1(){
		if (vez == id && !c1) {
			
			posicao [3].sprite = mao [0].sprite;
			mao [0].sprite = transp;
			c1 = true;

			msg = (3 * (id - 1)).ToString ();

			SendJogouToOthers ();

			vez = (vez + 1) % 4;

			atualizaMesa ();
		} 
	}

	public void clicou_2(){
		if (vez == id && !c2) {



			posicao [3].sprite = mao [1].sprite;
			mao [1].sprite = transp;
			c2 = true;

			msg = (3 * (id - 1)+1).ToString();

			SendJogouToOthers ();

			vez = (vez + 1) % 4;

			atualizaMesa ();
		} 
	}

	public void clicou_3(){
		if (vez == id && !c3) {
			
			posicao [3].sprite = mao [2].sprite;
			mao [2].sprite = transp;
			c3 = true;

			msg = (3 * (id - 1)+2).ToString();

			SendJogouToOthers ();

			vez = (vez + 1) % 4;

			atualizaMesa ();
		} 
	}

	[RPC]
	void SendJogouToOthers (){
		nView.RPC("ReceiveJogou", RPCMode.Others, msg);
	}

	[RPC]
	void ReceiveJogou (string info){
		int num = int.Parse (info);
		posicao[3].sprite = cards [sorteio [num]].img;
		vez = (vez + 1) % 4;
		atualizaMesa ();
	}
		
	[RPC]
	void SendComecoToClient (){
		nView.RPC("ReceiveComecoFromServer", RPCMode.Others, msg);
	}

	[RPC]
	void ReceiveComecoFromServer(string info){
		for (int i = 0, j=0; i < 24; i += 2,j++) {
			sorteio [j] = int.Parse (info.Substring (i, 2));
		}
		for (int i = 0; i < 3; i++) {
			mao[i].sprite = cards[sorteio[3*(id-1)+i]].img;
		}
		c1 = false;
		c2 = false;
		c3 = false;
		cont = 0;
		vez = (comecou)%4+1;
		comecou = vez;
		atualizaMesa ();
		print ("Rodada Iniciada com sucesso!");
	}
		

	[RPC]
	void SendInfoToClient() {
		nView.RPC("ReceiveInfoFromServer", RPCMode.Others, msg);
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


	void atualizaMesa(){
		if (vez == 1) {

			if (id == 1) {
				Principal.sprite = jogador_ativo [1];
				Parceiro.sprite = jogador_normal [3];
				advDireito.sprite = jogador_normal [2];
				advEsquerdo.sprite = jogador_normal [4];
			} else if (id == 2) {
				Principal.sprite = jogador_normal [2];
				Parceiro.sprite = jogador_normal [4];
				advDireito.sprite = jogador_normal [3];
				advEsquerdo.sprite = jogador_ativo [1];
			} else if (id == 3) {
				Principal.sprite = jogador_normal [3];
				Parceiro.sprite = jogador_ativo [1];
				advDireito.sprite = jogador_normal [4];
				advEsquerdo.sprite = jogador_normal [2];
			} else if (id == 4) {
				Principal.sprite = jogador_normal [4];
				Parceiro.sprite = jogador_normal [2];
				advDireito.sprite = jogador_ativo [1];
				advEsquerdo.sprite = jogador_normal [3];
			}
		} else if (vez == 2) {
			if (id == 1) {
				Principal.sprite = jogador_normal [1];
				Parceiro.sprite = jogador_normal [3];
				advDireito.sprite = jogador_ativo [2];
				advEsquerdo.sprite = jogador_normal [4];
			} else if (id == 2) {
				Principal.sprite = jogador_ativo [2];
				Parceiro.sprite = jogador_normal [4];
				advDireito.sprite = jogador_normal [3];
				advEsquerdo.sprite = jogador_normal [1];
			} else if (id == 3) {
				Principal.sprite = jogador_normal [3];
				Parceiro.sprite = jogador_normal [1];
				advDireito.sprite = jogador_normal [4];
				advEsquerdo.sprite = jogador_ativo [2];
			} else if (id == 4) {
				Principal.sprite = jogador_normal [4];
				Parceiro.sprite = jogador_ativo [2];
				advDireito.sprite = jogador_normal [1];
				advEsquerdo.sprite = jogador_normal [3];
			}
		} else if (vez == 3) {
			if (id == 1) {
				Principal.sprite = jogador_normal [1];
				Parceiro.sprite = jogador_ativo [3];
				advDireito.sprite = jogador_normal [2];
				advEsquerdo.sprite = jogador_normal [4];
			} else if (id == 2) {
				Principal.sprite = jogador_normal [2];
				Parceiro.sprite = jogador_normal [4];
				advDireito.sprite = jogador_ativo [3];
				advEsquerdo.sprite = jogador_normal [1];
			} else if (id == 3) {
				Principal.sprite = jogador_ativo [3];
				Parceiro.sprite = jogador_normal [1];
				advDireito.sprite = jogador_normal [4];
				advEsquerdo.sprite = jogador_normal [2];
			} else if (id == 4) {
				Principal.sprite = jogador_normal [4];
				Parceiro.sprite = jogador_normal [2];
				advDireito.sprite = jogador_normal [1];
				advEsquerdo.sprite = jogador_ativo [3];
			}
		} else if (vez == 4) {
			if (id == 1) {
				Principal.sprite = jogador_normal [1];
				Parceiro.sprite = jogador_normal [3];
				advDireito.sprite = jogador_normal [2];
				advEsquerdo.sprite = jogador_ativo [4];
			} else if (id == 2) {
				Principal.sprite = jogador_normal [2];
				Parceiro.sprite = jogador_ativo [4];
				advDireito.sprite = jogador_normal [3];
				advEsquerdo.sprite = jogador_normal [1];
			} else if (id == 3) {
				Principal.sprite = jogador_normal [3];
				Parceiro.sprite = jogador_normal [1];
				advDireito.sprite = jogador_ativo [4];
				advEsquerdo.sprite = jogador_normal [2];
			} else if (id == 4) {
				Principal.sprite = jogador_ativo [4];
				Parceiro.sprite = jogador_normal [2];
				advDireito.sprite = jogador_normal [1];
				advEsquerdo.sprite = jogador_normal [3];
			}
		}

	}
}
