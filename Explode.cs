using UnityEngine;
using System.Collections;

public class Explode : MonoBehaviour {

	public GameObject explosion;
	public ParticleSystem[] effects;

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "Hat") {
			Debug.Log("bomb");
			Instantiate (explosion, transform.position, transform.rotation);
			foreach (var effect in effects) {
				effect.transform.parent = null;
				effect.Stop ();
				Destroy (effect.gameObject, 1.0f);
			}
			Debug.Log("bomb");
			Destroy (gameObject);
		}
	}
}
