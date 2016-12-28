using UnityEngine;
using System.Collections;

public class Pointer02 : MonoBehaviour {

    private BackgroundScroll kBack;
    private float rot;
    private float trot;
    private float current;

	// Use this for initialization
	void Start () {
        kBack = BackgroundScroll.Instance;
        trot = 150;
        rot = trot;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        trot = 150 - Mathf.Clamp(kBack.GetRollingSpeed() * 50000, 0, 150);
        rot = Mathf.SmoothDamp(rot, trot, ref current, 1.0f);
        transform.localRotation = Quaternion.Euler(new Vector3(0, rot, 0));
	}
}
