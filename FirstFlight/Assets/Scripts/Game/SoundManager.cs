using UnityEngine;
using System.Collections;

public class SoundManager : GenericSingleton<SoundManager> {

	public AudioSource bgWithoutBirds;
	public AudioSource bgWithBirds;
	public AudioSource endLevel;
	public AudioSource spinSound;
	public AudioSource fallSound;

	public float bgwithBirdsMaxVolume;
	public float bgwithoutBirdsMaxVolume;
	public AudioSource[] engineSounds;

	public AudioSource[] spawnSounds;
	public AudioSource[] tenseSounds;
	public AudioSource[] splatSounds;

	void Start(){
        GameObject gameCtrl = GameObject.FindGameObjectWithTag("GameController");
        if(gameCtrl!=null)
            gameCtrl.GetComponent<GameController>().StartGame += StartBackgroundMusic;
        if (bgWithoutBirds != null)
            bgWithoutBirds.volume = bgwithoutBirdsMaxVolume;
		Invoke("startSecondEngineSound", 2);
	}

	void startSecondEngineSound(){
        if (engineSounds != null)
            engineSounds[1].Play();
	}

	void StartBackgroundMusic(){
		StartCoroutine(fadeOutAndFadeIn (engineSounds [0], 1, bgWithoutBirds, bgwithBirdsMaxVolume, 1f));
		StartCoroutine(fadeOutAndFadeIn (engineSounds [1], 1, bgWithoutBirds, bgwithBirdsMaxVolume, 1f));
		Invoke ("changeBackgroundMusic", 5);
	}

	void changeBackgroundMusic(){
		StartCoroutine (fadeOutAndFadeIn (bgWithoutBirds, bgwithoutBirdsMaxVolume, bgWithBirds, bgwithBirdsMaxVolume, 2f));
	}

	public void playBirdSpawnSound(){

		if (!isSoundinListPlaying (spawnSounds) && !isSoundinListPlaying (tenseSounds) && !isSoundinListPlaying (splatSounds)) {
			spawnSounds[Random.Range(0, spawnSounds.Length)].Play();
		}
		if (bgWithBirds.isPlaying) {
			return;
		}
	}

	public void playBirdTenseSound(){

		if (!isSoundinListPlaying (tenseSounds) && !isSoundinListPlaying (splatSounds)) {
			tenseSounds[Random.Range(0, tenseSounds.Length)].Play();
		}
	}

	public void playBirdSplatSound(){

		if (!isSoundinListPlaying (splatSounds)) {
			splatSounds[Random.Range(0, splatSounds.Length)].Play();
		}
	}

	bool isSoundinListPlaying(AudioSource[] list){

		foreach (AudioSource audio in list) {
			if(audio.isPlaying){
				return true;
			}
		}
		return false;
	}

	IEnumerator fadeOutAndFadeIn(AudioSource fadeOutMusic, float fadeOutMaxVolume, AudioSource fadeInMusic, float fadeInMaxVolume, float time){

		fadeInMusic.volume = 0f;
		fadeInMusic.Play ();
		for (int i = 0; i < 100; i++) {
			fadeOutMusic.volume -= fadeOutMaxVolume/100;
			fadeInMusic.volume += fadeInMaxVolume/100;
			yield return new WaitForSeconds(time/100);
		}
		fadeOutMusic.Stop ();
	}

	//Call at the end of level a couple of seconds before spin happens
	public void endLevelSound(){
		StartCoroutine (fadeOutAndFadeIn (bgWithBirds, bgwithBirdsMaxVolume, endLevel, 1f, 2f));
	}

	//Call when flip begins
	public void initiateSpinSound(){
		spinSound.Play ();
	}

	//Call when second level begins
	public void restartLevelSound(){
		StartCoroutine (fadeOutAndFadeIn (spinSound, 1, bgWithBirds, bgwithBirdsMaxVolume, 1f));
	}

	//Call when plan is crashing
	public void fallingSound(){
		fallSound.Play ();
	}
}