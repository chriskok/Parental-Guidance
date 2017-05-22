using UnityEngine;
using System.Collections;

public class FlamethrowerScript : MonoBehaviour {

	private AudioSource m_AudioSource;
	public AudioClip m_ShootSound;
	//public GameObject flame;
	public GameObject []notes;
	public float noteStartTime;
	public float noteStopTime;
	public float noteNextTime = 0;

	//private GameObject flame1;
	//private GameObject flame2;
	private GameObject note1;
	private GameObject note2;
	//private bool flameMade;
	//private bool noteMade = false;
	private Animator anim;
	
	void Start (){
		anim = GetComponentInChildren<Animator> ();
		m_AudioSource = GetComponent<AudioSource> ();
	}

	void Update () {
		if (TileScript.gameStart == true && noteNextTime <= 0) {
			if (Input.GetKeyDown ("w")) {
				Invoke ("SpawnNote", noteStartTime);
				Invoke ("SpawnNote", noteStartTime + 1);
				Invoke ("SpawnNote", noteStartTime + 2);
			}
		}

		if (noteNextTime > 0) {
			noteNextTime -= Time.deltaTime;
		}
	}

	/*void SpawnFlames(){
		anim.SetBool ("Active", true);
		flame1 = (GameObject)Instantiate (flame, transform.position, Quaternion.identity);
		flame2 = (GameObject)Instantiate (flame, transform.position, Quaternion.Euler(0,0,180));
		flameNextTime += 6;
		Invoke ("StopFlames", flameStopTime);
	}

	void StopFlames(){
		anim.SetBool ("Active", false);
		flame1.SetActive (false);
		flame2.SetActive (false);
	}*/

	void SpawnNote(){
		m_AudioSource.PlayOneShot (m_ShootSound);
		anim.SetBool ("Active", true);
		int randomNum = Mathf.RoundToInt(Random.Range (0, notes.Length -1));
		note1 = (GameObject)Instantiate (notes[randomNum], transform.position, Quaternion.identity);
		note1.GetComponent<Rigidbody2D> ().velocity = new Vector2 (5,0);
		randomNum = Mathf.RoundToInt(Random.Range (0, notes.Length));
		note2 = (GameObject)Instantiate (notes[randomNum], transform.position, Quaternion.Euler(0,0,180));
		note2.GetComponent<Rigidbody2D> ().velocity = new Vector2 (-5,0);;
		noteNextTime += 6;
		Invoke ("DestroyNote", 3);
	}

	void DestroyNote(){
		Destroy (note1, 3f);
		Destroy (note2, 3f);
		anim.SetBool ("Active", false);
	}
}
