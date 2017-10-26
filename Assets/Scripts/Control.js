var Carta : Transform;
var posicao;
var cont = -7;
function OnGUI() {
	if (Network.peerType != NetworkPeerType.Disconnected){
		if(GUI.Button(new Rect(30,200,50,50),"CRIA"))
		{
			cont+=2;
			posicao = new Vector3 (cont,0, 0);

			Network.Instantiate(Carta, posicao , transform.rotation, 0);
			//GameObject.Find("zap(Clone)").transform.position = new Vector3(0,GameObject.Find("zap(Clone)").transform.position.y+0.5F,0);
		}
	}
}