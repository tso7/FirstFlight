using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ending : MonoBehaviour
{
    public Texture[] _creditsDeck;
	public float[] _time;
	public AudioSource endSound;
	public float endSoundTime;
	public float fadeSpeed;
	public Text creditRoll;

	private bool playCredit;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(PlayEnd());
		playCredit = false;
    }

    IEnumerator PlayEnd()
    {
        for (int i = 0; i < _creditsDeck.Length; i++)
        {
            gameObject.GetComponent<RawImage>().texture = _creditsDeck[i];
            yield return new WaitForSeconds(_time[i]);
        }
		playCredit = true;
		yield return new WaitForSeconds(endSoundTime);
		StartCoroutine (fadeOutBackground ());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel("end");
        }

		
		if (playCredit && creditRoll != null) { 
            Vector3 creditTransform = creditRoll.rectTransform.position;
            creditRoll.rectTransform.position = new Vector3(creditTransform.x, creditTransform.y + 1f, creditTransform.z);
		}
    }

	IEnumerator fadeOutBackground(){
		
		for (int i = 0; i < 1000; i++) {
			endSound.volume -= 0.01f;
			GetComponent<RawImage>().color = Color.Lerp(GetComponent<RawImage>().color, Color.black, fadeSpeed * Time.deltaTime);
			yield return new WaitForSeconds(endSoundTime/100);
		}
	}
}