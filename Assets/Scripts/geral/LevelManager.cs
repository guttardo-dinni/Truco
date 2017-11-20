using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public void LoadScene(string name){
		SceneManager.LoadScene (name,LoadSceneMode.Additive);
	}
	public void LoadSceneUnica(string name){
		SceneManager.LoadScene (name,LoadSceneMode.Single);
	}
	public void Destruir(string name){
		SceneManager.UnloadScene (name);
	}
}
