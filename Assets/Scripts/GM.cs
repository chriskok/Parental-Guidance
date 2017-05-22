using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {
	
	[Header("Object Declaring")]
	public GameObject player;
	public GameObject [] enemies;
	public GameObject tile;
	public GameObject baseSet;
	public GameObject[] preGameObjects;
	public Sprite [] endGameSprites;
	public Camera mainCam;


	[Header("UI Declaring")]
	public Button gameStarter;
	public Button resetButton;

	public Image enemyImg;
	public Image pillowImg;
	public Image clockImg;
	public Image balloonImg;

	public Text enemyText;
	public Text pillowText;
	public Text clockText;
	public Text balloonText;
	//public Text gameOverText;
	//public Text countdownText;

	[Header("Game End UI")]
	public Image GameEndShadow;
	public Image GameEndGlow;
	public GameObject GameEndBar;
	public Text GameEndBarText;
	public Text GameEndText;
	public Image GameEndImage;
	public Button gameEndReset;
	public Button gameEndHome;

	[Header("Level Setup")]
	public int baseWidth;
	public int baseHeight;
	public int spikesInLevel;
	public int flamesInLevel;
	public int bombsInLevel;

	public static int spikeAmount;
	public static int flameAmount;
	public static int bombAmount;

	public static int chosenValue = 0;
	private GameObject tempObj = null;

	public GameObject [,] tileSet;

	private int enemyNum = 0;
	private float timeNow;
	//private float countdownTimer = 0;
	private GameObject playerClone;
	//private Animator gameStartAnim;

	void Start () {
		SetTiles ();
		spikeAmount = spikesInLevel;
		flameAmount = flamesInLevel;
		bombAmount = bombsInLevel;
		//gameOverText.text = "";
		//gameStartAnim = gameStarter.GetComponent<Animator> ();
	}

	void SetTiles() {
		tileSet = new GameObject[baseWidth+1,baseHeight+1];

		for (int i = 0; i <= baseWidth; i++) {
			for (int j = 0; j <= baseHeight; j++) {
				tileSet [i,j] = (GameObject)Instantiate (tile, new Vector3 (i, j, 0), Quaternion.identity);
				tileSet [i,j].transform.parent = baseSet.transform;
			}
		}
	}

	void Update(){

		if (TileScript.gameStart == false) {
			if (Input.GetKeyDown ("q") && spikeAmount > 0) {
				chosenValue = 1;
				Destroy (tempObj);
				tempObj = (GameObject)Instantiate (preGameObjects [0], transform.position, Quaternion.identity);
			} else if (Input.GetKeyDown ("w") && flameAmount > 0) {
				chosenValue = 2;
				Destroy (tempObj);
				tempObj = (GameObject)Instantiate (preGameObjects [1], transform.position, Quaternion.identity);
			} else if (Input.GetKeyDown ("e") && bombAmount > 0) {
				chosenValue = 3;
				Destroy (tempObj);
				tempObj = (GameObject)Instantiate (preGameObjects [2], transform.position, Quaternion.identity);
			}

			Vector3 mousePos = mainCam.ScreenToWorldPoint (Input.mousePosition);
			mousePos.z = 10;

			if (tempObj != null) {
				tempObj.transform.position = mousePos;
			}
		} else {
			chosenValue = 0;
		}
		//Public static variable object. 
		//When chosen, show sprite
		//After that input the value into the tileScript
	}

	void FixedUpdate(){
		pillowText.text = "" + spikeAmount;
		clockText.text = "" + flameAmount;
		balloonText.text = "" + bombAmount;
		enemyText.text = "" + (enemies.Length - enemyNum);

		if ((enemies.Length - enemyNum) <= 0 && ((GameObject.FindGameObjectWithTag ("EnemyRotan") == null) && (GameObject.FindGameObjectWithTag ("EnemyBroco") == null))) {
				//Level Up conditions
				GameOver (1);
		}
	}

	public void GameStart(){
		//gameStartAnim.SetBool ("Clicked", true);
		baseSet.SetActive (false);
		TileScript.gameStart = true;
		SpawnPlayer ();
		SpawnEnemy ();

		timeNow = Time.realtimeSinceStartup;
		Invoke("TimeStopAndStart", 0.01f);
		gameStarter.gameObject.SetActive(false);
		resetButton.gameObject.SetActive (true);
	}

	public void GameOver(int waysToWin){

		Time.timeScale = 0.1f;
		DisplayGameEndUI ();
		Invoke ("DisplayGameEndButtons", 0.3f);

		if (waysToWin == 1) {
			GameEndBarText.text = "Did you know? \nOnly 1 out of every 0.5 kids grow up";
			GameEndText.text = "Congratulations! \n\n\n\n\n\n\n\n\n\nYou survived!";
			GameEndImage.sprite = endGameSprites[0];
		} else if (waysToWin == 2) {
			GameEndBarText.text = "Not so fun fact:\nThe rotan (cane) is the best 'teaching tool' among South-East Asian parents";
			GameEndText.text = "You were punished \n\n\n\n\n\n\n\n\n\nby the sneaky Rotan";
			GameEndImage.sprite = endGameSprites[1];
		} else if (waysToWin == 3) {
			GameEndBarText.text = "Did you know? \nBroccoli is so bro, it can bro you to the deepest hell of bro, bro!";
			GameEndText.text = "You've been punished \n\n\n\n\n\n\n\n\n\nby 'The Great' broccoli";
			GameEndImage.sprite = endGameSprites[2];
		}
	}

	public void Reset(){
		TileScript.gameStart = false;
		TileScript.numOfBombs = 0;
		SceneManager.LoadScene ("Main");
	}

	public void Home(){
		SceneManager.LoadScene ("MainMenu");
	}

	void SpawnPlayer(){
		int xVal = Mathf.RoundToInt (Random.Range ((baseWidth/2 - 1), (baseWidth/2 + 1)));
		int yVal = Mathf.RoundToInt (Random.Range ((baseWidth/2 - 1), (baseWidth/2 + 1)));
		GameObject currentObject = tileSet [xVal,yVal];
		while (currentObject.GetComponent<TileScript>().used == true){
			xVal = Mathf.RoundToInt (Random.Range ((baseWidth/2 - 1), (baseWidth/2 + 1)));
			yVal = Mathf.RoundToInt (Random.Range ((baseWidth/2 - 1), (baseWidth/2 + 1)));
			currentObject = tileSet [xVal,yVal];
		}
		Debug.Log ("Spawning Player at: " + xVal + "," + yVal);
		currentObject.GetComponent<TileScript> ().used = true;
		playerClone = Instantiate (player, new Vector3 (xVal, yVal, 0), Quaternion.identity) as GameObject;
		playerClone.name = "PlayerControl";
	}

	public void SpawnEnemy(){
		if (enemyNum < enemies.Length) {
			

			Vector3 currentPos = playerClone.transform.position;
			float dist;

			//Get randomTile from TileSet
			int xVal = Mathf.RoundToInt (Random.Range (0, baseWidth));
			int yVal = Mathf.RoundToInt (Random.Range (0, baseHeight));
			GameObject currentObject = tileSet [xVal, yVal];
			dist = Vector3.Distance (currentPos, new Vector3(xVal,yVal,0));
			Debug.Log ("First distance: " + dist);

			while ((currentObject.GetComponent<TileScript> ().used == true) || (dist < 5)) {
				xVal = Mathf.RoundToInt (Random.Range (0, baseWidth));
				yVal = Mathf.RoundToInt (Random.Range (0, baseHeight));
				currentObject = tileSet [xVal, yVal];
				dist = Vector3.Distance (currentPos, new Vector3(xVal,yVal,0));
				Debug.Log ("Recalculating, distance was: " + dist);
			}

			Debug.Log ("Distance from player " + dist);

			Debug.Log ("Spawning Enemy #" + (enemyNum + 1) + " at: " + xVal + "," + yVal);
			Instantiate (enemies [enemyNum], new Vector3 (xVal, yVal, 0), Quaternion.identity);
			enemyNum++;
		}
	}

	void TimeStopAndStart(){
		while ((Time.realtimeSinceStartup - timeNow) < 3) {
			Time.timeScale = 0;
		}
		Time.timeScale = 1;
	}

	void DisplayGameEndUI(){
		GameEndShadow.gameObject.SetActive (true);
		GameEndGlow.gameObject.SetActive (true);
		GameEndBar.SetActive(true);
		GameEndBarText.gameObject.SetActive (true);
		GameEndText.gameObject.SetActive (true);
		GameEndImage.gameObject.SetActive (true);
	}

	void DisplayGameEndButtons(){
		gameEndReset.gameObject.SetActive (true);
		gameEndHome.gameObject.SetActive (true);
	}

	void ChooseRandomStrings(){
		//For future use of end game sequence
	}
}
