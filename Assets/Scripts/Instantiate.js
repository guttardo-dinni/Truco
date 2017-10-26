var Carta : Transform;

function OnNetworkLoadedLevel () {
	// Instantiating SpaceCraft when Network is loaded
	Network.Instantiate(Carta, transform.position, transform.rotation, 0);
	//Network.Instantiate(Carta, transform.position, transform.rotation, 0);
}

function OnPlayerDisconnected (player : NetworkPlayer) {
	Network.RemoveRPCs(player, 0);
	Network.DestroyPlayerObjects(player);
}