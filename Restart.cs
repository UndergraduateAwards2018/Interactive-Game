using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {

	public void OnMouseDown () {

		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
