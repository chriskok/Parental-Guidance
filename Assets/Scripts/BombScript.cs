using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

	private AudioSource m_AudioSource;
	public AudioClip m_BombSound;

	public GameObject bomb;
	public int bombNum = 0;

	public Animator anim;
	private KeyCode[] keyCodes = {
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
		KeyCode.Alpha5,
	};

	void Awake(){
		anim = GetComponentInChildren<Animator> ();
		m_AudioSource = GetComponent<AudioSource> ();
	}

	void Update () {
		if (TileScript.gameStart == true) {
			if (Input.GetKeyDown (keyCodes[bombNum - 1])) {
				m_AudioSource.PlayOneShot (m_BombSound);
				anim.SetTrigger ("Blast");
				Invoke ("SpawnBlast", 0.41f);
			}
		}
	}

	void SpawnBlast(){
		Instantiate (bomb, transform.position, Quaternion.identity);
		Destroy (gameObject);
	}
}
