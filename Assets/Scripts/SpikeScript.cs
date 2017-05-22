using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {

	private AudioSource m_AudioSource;
	public AudioClip m_PillowSound;

	public float spikeTime = 0;
	public float spikeWait;

	private bool spikeUp = false;
	private Animator anim;

	void Start(){
		anim = GetComponentInChildren<Animator> ();
		m_AudioSource = GetComponent<AudioSource> ();
	}

	void Update () {
		//Change with on trigger enter
		/*if (TileScript.gameStart == true) {
			if (Input.GetKeyDown ("q") && spikeTime <= 0) {
				SpikeUp ();
			}
		}*/

		if (spikeTime > 0) {
			spikeTime -= Time.deltaTime;
		}
	}

	void SpikeUp(){
		m_AudioSource.PlayOneShot (m_PillowSound);
		anim.SetBool ("Snap", true);
		spikeUp = true;
		spikeTime += spikeWait;
		Invoke ("SpikeDown", 0.5f);
	}

	void SpikeDown(){
		anim.SetBool ("Snap", false);
		spikeUp = false;
	}

	void OnTriggerEnter2D (Collider2D other) {

		if ((other.tag == "Player") && !spikeUp && spikeTime <= 0) {
			anim.SetTrigger ("Set");
			Invoke("SpikeUp", 1f);
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if ((other.tag == "EnemyRotan" || other.tag == "EnemyBroco") && spikeUp) {
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
