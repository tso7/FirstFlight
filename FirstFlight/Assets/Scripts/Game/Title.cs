using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

    private static AudioInputController kAudioCtrl;

	// Use this for initialization
	void Start () {
        kAudioCtrl = AudioInputController.Instance;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            Application.LoadLevel("intro");
	}
}
