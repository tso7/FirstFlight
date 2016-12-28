using UnityEngine;
using System.Collections;

public class BackgroundScroll : GenericSingleton<BackgroundScroll> {

    public float _scrollSpeed = 0.05f;
    public float loopcount;
    //public AnimatorControllerParameter plane;
    private static AudioInputAdapter kAudioIn;
    private static GameController kGameCtrl;
    private int blend_hash_ = Animator.StringToHash("Blend");
    private float tspeed_;
    private float speed_;
    private float actual_speed_;
    private float current_acc_;
    private float saved_y_;
    private Vector2 saved_offset_;
    private bool ingame_;

    void Awake () {
        kGameCtrl = GameController.Instance;
        kGameCtrl.StartGame += StartGame;
        kGameCtrl.EndGame += EndGame;
    }

    void Start () {
        loopcount = 0;
        saved_offset_ = GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
        kAudioIn = AudioInputAdapter.Instance;
    }

    void Update () {
        tspeed_ = Mathf.Abs(kAudioIn.GetInputVolume(AudioInputAdapter.GetDevice()) * _scrollSpeed);
        if (!ingame_) {
            tspeed_ = 0;
        }
        speed_ = Mathf.SmoothDamp(speed_, tspeed_, ref current_acc_, 3.0f);
        actual_speed_ = Mathf.Clamp(speed_ * Time.deltaTime, 0f, 0.003f) + 0.0003f;
        if (!ingame_) {
            actual_speed_ -= 0.0003f;
        }
        saved_y_ += actual_speed_;
        if (saved_y_ >= 1) {
            loopcount++;
        }
        //plane.SetFloat(blend_hash_, actual_speed_ * 500);
        saved_y_ = Mathf.Repeat(saved_y_, 1f);
        loopcount = saved_y_ + Mathf.Floor(loopcount);
        Vector2 offset = new Vector2(saved_offset_.x, saved_y_);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    public float GetRollingSpeed () {
        return actual_speed_;
    }

    private void StartGame () {
        ingame_ = true;
    }

    private void EndGame () {
        ingame_ = false;
    }

    void OnDisable () {
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", saved_offset_);
    }
}
