using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public int timeLeft = 5;
	public Text countdownText;
	public Vector3 update1g,update2g,update3g,update4g;
	public Vector3 update1m,update2m,update3m,update4m;




	public GameController stateMachine;

public	void Startcal()
	{
		
		StartCoroutine("timer");
	

		

	}


	void Update()
	{
		if (stateMachine.currentState == GameController.GameState.Calibrate) {

			if (timeLeft > 0) {
				FaceController face = GetComponent<FaceController > ();
				if (timeLeft == 4) {
					update1g = face.getInstantaniousVec3 (); 
					update1m = face.getInstantaniousMagVec3 ();
				}
				if (timeLeft == 3) {
					update2g = face.getInstantaniousVec3 ();
					update2m = face.getInstantaniousMagVec3 ();
				}
				if (timeLeft == 2) {
					update3g = face.getInstantaniousVec3 ();
					update3m = face.getInstantaniousMagVec3 ();
				}
				if (timeLeft == 1) {
					update4g = face.getInstantaniousVec3 ();
					update4m = face.getInstantaniousMagVec3 ();
				}
			}
			if (timeLeft <= 0) {

				FaceController face = GetComponent<FaceController > ();
				Vector3 averageRefGrav = (update1g + update2g + update3g + update4g) / 4.0f;
				face.applyReferenceGravity (averageRefGrav);
				Vector3 averageRefMag = (update1m + update2m + update3m + update4m) / 4.0f;
				face.applyReferenceMag (averageRefMag);

				StopCoroutine ("timer");
				countdownText.text = "Ready!";
				countdownText.enabled = false;
			
				stateMachine.SetCurrentState (GameController.GameState.Orange);
			}


		}

	}

	IEnumerator timer()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			timeLeft--;
		}
	}
}