using UnityEngine;
using System.Collections;

public class BombBlastScript : MonoBehaviour {

	Vector3 originalCameraPosition;

	float shakeAmt = 0.1f;

	public Camera mainCamera;

	/*void OnCollisionEnter2D(Collision2D coll) 
	{

		shakeAmt = coll.relativeVelocity.magnitude * .0025f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);

	}*/

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "EnemyRotan" || other.tag == "EnemyBroco") {
			C_Rotan cr = other.gameObject.GetComponent<C_Rotan>();
			C_Broccoli cb = other.gameObject.GetComponent<C_Broccoli>();
			if (cr != null)
				cr.DeathSentence (true);
			if (cb != null)
				cb.DeathSentence (true);
			Destroy (other.gameObject, 0.5f);
			Invoke ("NextEnemy", 0.5f);
		}
	}

	void Start(){
		mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		originalCameraPosition = mainCamera.transform.position;
		shakeAmt = 0.1f;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);
		Invoke ("EndBlast", 0.5f);
	}

	void CameraShake()
	{
		if(shakeAmt>0) 
		{
			float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
			Vector3 pp = mainCamera.transform.position;
			pp.y+= quakeAmt; // can also add to x and/or z
			pp.x+= quakeAmt;
			mainCamera.transform.position = pp;
		}
	}

	void StopShaking()
	{
		CancelInvoke("CameraShake");
		mainCamera.transform.position = originalCameraPosition;
	}
		
	void EndBlast (){
		SpriteRenderer sr = GetComponentInChildren<SpriteRenderer> ();
		sr.enabled = false;
		gameObject.GetComponent<CircleCollider2D> ().enabled = false;
		Destroy (gameObject, 2.5f);
	}

	void NextEnemy(){
		GM gmScript = GameObject.Find ("GameManager").GetComponent<GM>();
		gmScript.SpawnEnemy ();
	}
}
