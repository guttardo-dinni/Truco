

function Cria () {
	Network.InitializeServer(4, 25000, false);

	for (var go : GameObject in FindObjectsOfType(GameObject)){
		go.SendMessage("OnNetworkLoadedLevel",
		SendMessageOptions.DontRequireReceiver);
	}
}

function OnConnectedToServer () {
	// Notify our objects that the level and the network are ready
	for (var go : GameObject in FindObjectsOfType(GameObject))
		go.SendMessage("OnNetworkLoadedLevel",
	SendMessageOptions.DontRequireReceiver);
}