using UnityEngine;
using System.Collections;

public class NoteScript : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "EnemyRotan" || other.tag == "EnemyBroco") {
			C_Rotan cr = other.gameObject.GetComponent<C_Rotan>();
			C_Broccoli cb = other.gameObject.GetComponent<C_Broccoli>();
			if (cr != null)
				cr.DeathSentence (true);
			if (cb != null)
				cb.DeathSentence (true);
			Destroy (other.gameObject, 0.5f);
			Invoke ("NextEnemy", 1f);
		}
	}

	void NextEnemy(){
		GM gmScript = GameObject.Find ("GameManager").GetComponent<GM>();
		gmScript.SpawnEnemy ();
	}
		
}
