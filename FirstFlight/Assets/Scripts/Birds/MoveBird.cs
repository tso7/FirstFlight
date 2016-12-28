using UnityEngine;
using System.Collections;

public class MoveBird : MonoBehaviour {

    // Variables
    public float speed;

    private const float birdPanicThreshold = 3;
    private SoundManager sound_manager_;
    private GameObject airplane_;
    private Animator anim_;
    private int hash_frenzy_ = Animator.StringToHash("Frenzy");
    private BackgroundScroll kBack;
    private Rigidbody rig_;
    private Vector3 qtarget_;
    private Vector3 current_angular_;

    void Start () {
        kBack = BackgroundScroll.Instance;
        rig_ = GetComponent<Rigidbody>();
        rig_.velocity = transform.forward * speed;
        sound_manager_ = SoundManager.Instance;
        airplane_ = GameObject.FindWithTag("Airplane");
        anim_ = GetComponentInChildren<Animator>();
        qtarget_ = transform.rotation.eulerAngles;
    }

    void Update () {
        // rig.position += new Vector3(0, 0, -3 * back.GetRollingSpeed());

        if (Vector3.Distance(gameObject.transform.position, airplane_.transform.position) < birdPanicThreshold
            && sound_manager_ != null
            && airplane_.transform.position.z + 1 <= transform.position.z) {
            sound_manager_.playBirdTenseSound();
            // Trigger animation
            anim_.SetBool(hash_frenzy_, true);
            sound_manager_ = null;
            qtarget_ = Quaternion.LookRotation(gameObject.transform.position - airplane_.transform.position).eulerAngles;
            //transform.GetChild(0).localRotation = Quaternion.Euler(45f, 0f, 0f);
        }
        else {
            anim_.SetBool(hash_frenzy_, false);
            rig_.velocity = transform.forward * (speed + 500 * speed * kBack.GetRollingSpeed());
        }
        transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.rotation.eulerAngles, qtarget_, ref current_angular_, 1.0f));
    }
}
