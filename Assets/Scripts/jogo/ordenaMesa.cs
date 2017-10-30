using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ordenaMesa : MonoBehaviour {

	public Image Principal;
	public Image Parceiro;
	public Image advDireito;
	public Image advEsquerdo;

	public Sprite[] jogador;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("id") == 1) {
			Principal.sprite = jogador [1];
			Parceiro.sprite = jogador [3];
			advDireito.sprite = jogador [2];
			advEsquerdo.sprite = jogador [4];

		}
		else if (PlayerPrefs.GetInt ("id") == 2) {
			Principal.sprite = jogador [2];
			Parceiro.sprite = jogador [4];
			advDireito.sprite = jogador [3];
			advEsquerdo.sprite = jogador [1];

		}
		else if (PlayerPrefs.GetInt ("id") == 3) {
			Principal.sprite = jogador [3];
			Parceiro.sprite = jogador [1];
			advDireito.sprite = jogador [4];
			advEsquerdo.sprite = jogador [2];

		}
		else if (PlayerPrefs.GetInt ("id") == 4) {
			Principal.sprite = jogador [4];
			Parceiro.sprite = jogador [2];
			advDireito.sprite = jogador [1];
			advEsquerdo.sprite = jogador [3];

		}
	}

}
