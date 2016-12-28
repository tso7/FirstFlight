using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Intro : MonoBehaviour {

    public Texture[] _imageDeck;
	public float[] _time;
	public AudioSource[] _dialog;
	public GameObject soundObject;

    // Use this for initialization
	void Start () {
        StartCoroutine(PlayIntro());
	}

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.LoadLevel("main");
        }
    }
	
	IEnumerator PlayIntro()
    {
        float flt = 3;
        for (int i=0; i < _imageDeck.Length; i++)
        {
            gameObject.GetComponent<RawImage>().texture = _imageDeck[i];
            if (i < _dialog.Length) {
                if (_dialog[i] != null)
                    _dialog[i].Play();
                yield return new WaitForSeconds(_time[i]);
            }
            else
                yield return new WaitForSeconds(flt);
        }
		StartCoroutine(fadeOutBackground (soundObject.GetComponent<AudioSource> (), 2));
        yield return new WaitForSeconds(flt);
        Application.LoadLevel("main");
    }

	IEnumerator fadeOutBackground(AudioSource fadeOutMusic, float time){

		for (int i = 0; i < 100; i++) {
			fadeOutMusic.volume -= 0.02f;
			yield return new WaitForSeconds(time/100);
		}
		fadeOutMusic.Stop ();
	}
}