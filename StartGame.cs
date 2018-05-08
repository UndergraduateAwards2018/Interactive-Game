using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	public GameController gameController;





	void OnMouseDown () {

		gameController.StartGame ();

	}
}
