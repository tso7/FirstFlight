using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : GenericSingleton<GameController> {
    //Delegates
    public delegate void StartGameHandler ();
    public delegate void EndGameHandler ();
    public event StartGameHandler StartGame;
    public event EndGameHandler EndGame;

    // Vars
    public float startWaitVer, startWaitHor;
    public float waveWaitVer, waveWaitHor;
    public Text hudText, hudGameOver;
    public GameObject landscape;
    public GameObject cloud;
	public GameObject airplane;

	public int spawnDelay;
    // Counter for GUI text
    public static int birdHitCounter = 0;
    public const int planeLife = 20;

    // SpawnController
    private static SpawnController _spawn;
    private static bool ingame_ = false;
    private const float kLandSpeed = 0.4f;
    private const float kStartPos = 38;
    private const float kEndPos = -40;

    private static BackgroundScroll kBack;
    private static AudioInputController kAudioCtrl;
    private static SoundManager kSound;

    #region publicmethods
    public void OnGameOver (bool win) {
        if (win) {
            if (EndGame != null)
                EndGame();
            Application.LoadLevel("goodEnd");
        }
        else {
            if (EndGame != null)
                EndGame();
            Application.LoadLevel("badEnd");
        }
        
    }

    public void OnGameStart () {
        if (StartGame != null) {
            StartGame();
        }
    }
    #endregion

    void Awake () {
        if (Application.loadedLevel != 2) {
            Destroy(this);
        }
        kAudioCtrl = AudioInputController.Instance;
        kSound = SoundManager.Instance;
        StartGame += GameCtrlStart;
        EndGame += GameCtrlEnd;
        kAudioCtrl.StartDetect();
    }

    void Start () {
		//Time.timeScale = 5;
        //StartCoroutine(EndGameTimer());
        _spawn = SpawnController.Instance;
        birdHitCounter = 0;
    }

    void FixedUpdate () {
        UIDisplay();
        if (ingame_) {
            if (landscape.transform.position.z > kStartPos - 5) { 
                landscape.transform.position += new Vector3(0, 0, -kLandSpeed * Mathf.Pow(Mathf.Abs(landscape.transform.position.z - 32), 3) * Time.deltaTime);
            }
            else
                landscape.transform.position += new Vector3(0, 0, -kLandSpeed * Time.deltaTime);
        }
        if (ingame_ && cloud.transform.localScale.x > 1) {
            cloud.transform.position += new Vector3(0, -0.003f, 0);
            cloud.transform.localScale += new Vector3(-0.003f, -0.003f, -0.003f);
        }
        if (landscape.transform.position.z <= kEndPos + 5) {
            kSound.restartLevelSound();
        }
        if (landscape.transform.position.z <= kEndPos) {
            OnGameOver(true);
        }
    }

    // End game after 1.5 minutes (temp)
    IEnumerator EndGameTimer () {
        yield return new WaitForSeconds(80);
        yield return new WaitForSeconds(10);
        hudGameOver.GetComponent<Text>().enabled = true;
        hudGameOver.text = hudGameOver.text + (birdHitCounter * 100).ToString();
    }

    // Display UI Text
    void UIDisplay () {
        // hudText.text = birdHitCounter.ToString() + " BIRDS OBLITERATED";
    }

    void GameCtrlStart () {
        ingame_ = true;
        StartCoroutine(LoadLevel1());
    }

    void GameCtrlEnd () {
        ingame_ = false;
        StopAllCoroutines();
        Application.LoadLevel(Application.loadedLevel + 1);
    }

	IEnumerator LoadLevel2(){
		yield return new WaitForSeconds(spawnDelay);
        kSound.restartLevelSound();
		Vector3 tempPosition = _spawn.spawnValuesVer;
		_spawn.spawnValuesVer = new Vector3 (1, tempPosition.y, tempPosition.z);
        _spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 1);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.spawnValuesVer = tempPosition;
		_spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 3);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 1);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 1);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 1);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 2);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 2);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Big, Direction.Diagonal, 1);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 1);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 2);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 1);
		_spawn.SpawnBird(BirdType.Big, Direction.Diagonal, 1);
    }

	IEnumerator LoadLevel1(){
		yield return new WaitForSeconds(1);
		Vector3 tempPosition = _spawn.spawnValuesVer;
		_spawn.spawnValuesVer = new Vector3 (1, tempPosition.y, tempPosition.z);
        _spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 1);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.spawnValuesVer = tempPosition;
		_spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 3);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 1);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 1);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 1);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 2);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 2);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Big, Direction.Diagonal, 1);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 1);
		yield return new WaitForSeconds(spawnDelay);
		_spawn.SpawnBird(BirdType.Regular, Direction.Vertical, 2);
		_spawn.SpawnBird(BirdType.Fast, Direction.Horizontal, 1);
		_spawn.SpawnBird(BirdType.Big, Direction.Diagonal, 1);
		yield return new WaitForSeconds(spawnDelay);
        kSound.endLevelSound();
        yield return new WaitForSeconds(spawnDelay);
        kSound.initiateSpinSound();
		airplane.GetComponent<PlaneFlip> ().enabled = true;
		StartCoroutine(LoadLevel2 ());
	}
}