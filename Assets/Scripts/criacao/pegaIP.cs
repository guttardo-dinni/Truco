using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class pegaIP : MonoBehaviour {

	public Text ip;
	// Use this for initialization
	void Start () {
		if (Network.peerType == NetworkPeerType.Server)
			ip.text = Network.player.ipAddress;
		else
			ip.text = Network.connections[0].ipAddress;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
