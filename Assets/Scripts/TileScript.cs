using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

	public Color highlightColor;
	public Color oriColor;
	public Color chosenColor;
	//public Color flameThrower;
	//public Color spikes;


	public GameObject flameTrap;
	public GameObject spikeTrap;
	public GameObject bombTrap;

	public bool used = false;
	public int usedTrap = 0;

	private SpriteRenderer sr;
	private bool chosen = false;

	public static int numOfBombs = 0;
	public static bool gameStart = false;

	void Start () {
		sr = gameObject.GetComponent<SpriteRenderer> ();
	}

	void OnMouseEnter () {
		if (!used && !gameStart) {
			sr.color = highlightColor;
		}

	}

	void OnMouseExit(){
		if (!used && !gameStart) {
			sr.color = oriColor;
			chosen = false;
		}
	}

	void OnMouseDown(){
		if (!used && !gameStart && GM.chosenValue != 0) {
			UseTile (GM.chosenValue);
			//sr.color = chosenColor;
			chosen = true;
		}
	}

	void Update(){
		/*if (chosen) {
			if (Input.GetKeyDown ("q") && GM.spikeAmount > 0) {
				UseTile(1);
			} else if (Input.GetKeyDown ("w") && GM.flameAmount > 0) {
				UseTile(2);
			} else if (Input.GetKeyDown ("e") && GM.bombAmount > 0) {
				UseTile(3);
			}
		}*/
	}

	void UseTile(int trap){
		used = true;
		chosen = false;
		sr.color = oriColor;
		usedTrap = trap;

		if (trap == 1 && GM.spikeAmount > 0) {
			Instantiate (spikeTrap, transform.position, Quaternion.identity);
			GM.spikeAmount--;
		} else if (trap == 2 && GM.flameAmount > 0) {
			Instantiate (flameTrap, transform.position, Quaternion.identity);
			GM.flameAmount--;
		} else if (trap == 3 && GM.bombAmount > 0) {
			numOfBombs += 1;
			GameObject bomb = (GameObject)Instantiate (bombTrap, transform.position, Quaternion.identity);
			BombScript bs = bomb.GetComponent<BombScript> ();
			bs.bombNum = numOfBombs;
			GM.bombAmount--;
		}
	}
		
}
