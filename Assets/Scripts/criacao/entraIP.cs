using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class entraIP : MonoBehaviour {

	// Use this for initialization
	public NetworkView nView;
	private string msg;

	public void conecta(string ip){
		Network.Connect (ip, 25000);
	}

	void OnConnectedToServer() {

		msg = "Cliente Conectado";
		SendInfoToServer ();
		SceneManager.LoadScene ("criaServidor");
	}
		

	[RPC]
	void SendInfoToServer(){
		nView.RPC("ReceiveInfoFromClient", RPCMode.Server, msg);
	}

	[RPC]
	void ReceiveInfoFromClient(string info) {
	}

	[RPC]
	void SendInfoToClient() {
	}

	[RPC]
	void ReceiveInfoFromServer(string info) {
	}
}
