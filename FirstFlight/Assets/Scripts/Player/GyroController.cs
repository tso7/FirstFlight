using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour {

    private static GameController kGameCtrl;
    private float trot;
    private float rot;
    private float angular;

    void Start () {
        kGameCtrl = GameController.Instance;
    }

    void Update () {
        trot = Mathf.Clamp((float)GameController.birdHitCounter, 0, GameController.planeLife) / (float)GameController.planeLife * 240f;
        rot = Mathf.SmoothDamp(rot, trot, ref angular, 1.0f);
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, rot));
    }
}
