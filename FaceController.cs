using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading;
using System.IO;

public class FaceController : MonoBehaviour {

	public Camera cam;
	public static FaceController Instance;
	public SerialPort serial = new SerialPort ("\\\\.\\COM5",9600);

    private Rigidbody rigidbodyComponent;
	private Vector3 updateVector;
	public Vector3 updateMag;
	public Vector3 referenceMag=new Vector3(0,0,0);
	private Vector3 referenceVector=new Vector3(0,0,0);

	public Vector3 resultantMag;
	private StreamWriter outputFile;
	public bool rightHand=true;
	public Vector3 newpos = new Vector3 (0, 0, 0);

	public GameObject collectOrangeFace;
	public GameObject collectPurpleFace;
	public GameObject collectGreenFace;


	void Startcontrol () {
		serial.Open ();
		serial.ReadTimeout = 22;
		if (cam == null) {
			cam = Camera.main;
		}
	
	}
	public Vector3 getInstantaniousVec3()
	{
		return updateVector;

	}

	public void applyReferenceGravity(Vector3 grav)
	{
		referenceVector = grav;

	}

	public Vector3 getInstantaniousMagVec3()
	{
		return updateMag;

	}
	public void applyReferenceMag(Vector3 dir)
	{
		referenceMag = dir;

	}




	void Update()
	{
		string value=""; 
		string[] vec=new string[6];
		vec[0]=" ";
		vec[1]=" ";
		vec[2]=" ";
		vec[3]=" ";
		vec[4]=" ";
		vec[5]=" ";



		if (!serial.IsOpen) {
			serial.Open ();
		} else 
		{
			value = serial.ReadLine();
			vec = value.Split (',');


			
			updateVector=new Vector3 ((float)(decimal.Parse(vec[0]))/16600.0f,(float)(decimal.Parse(vec[1]))/16600.0f,(float)(decimal.Parse(vec[2]))/16600.0f);
		
			updateVector = new Vector3(Mathf.Floor(updateVector.x*10.0f),Mathf.Floor(updateVector.y*10.0f),Mathf.Floor(updateVector.z*10.0f))/10.0f;
			updateMag = new Vector3 ((float)(decimal.Parse (vec [3])), (float)(decimal.Parse (vec [4])), (float)(decimal.Parse (vec [5])));

		}
	}
	

	void FixedUpdate () {

		if (rigidbodyComponent == null) {
			rigidbodyComponent = GetComponent<Rigidbody> ();
		}



		resultantMag = updateMag - referenceMag;

		if (rightHand == true) {
			if (referenceMag != Vector3.zero) {
			
				collectOrangeFace.SetActive (true);


				if (Math.Abs (resultantMag.x) > 1.0f || Math.Abs (resultantMag.y) > 1.0f || Math.Abs (resultantMag.z) > 1.0f) {



					Vector3 newpos = transform.position;
					newpos.x = (resultantMag.magnitude) * 15.0f / 360.0f - 7.5f; 
					if (newpos.x > 7.5f) {
						newpos.x = 7.5f;
					}
					transform.position = newpos;
				
				}
			}
		} else {
			if (referenceMag != Vector3.zero) {

				collectOrangeFace.SetActive (true);


				if (Math.Abs (resultantMag.x) > 1.0f || Math.Abs (resultantMag.y) > 1.0f || Math.Abs (resultantMag.z) > 1.0f) {


					Vector3 newpos = transform.position;
					newpos.x = -(resultantMag.magnitude) * 15.0f / 360.0f + 7.5f; 
					if (newpos.x > 7.5f) {
						newpos.x = 7.5f;
					}
					transform.position = newpos;

				}
			}
		}
			

		}
}




	
		

	

