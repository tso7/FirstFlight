using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioInputController.Instance.StartDetect();
	}
    void Update () {
        if (AudioInputController.Instance.IsRising()) {
            Application.LoadLevel("intro");
        }
    }
}
