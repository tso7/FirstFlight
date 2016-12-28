using UnityEngine;
using System.Collections;

public class DestroyOnCollision : MonoBehaviour
{

    public GameObject BirdKillEffect;
	public float damageAmount;

    // Shake variables
    public float _duration = 0.25f;
    public float _magnitude = 0.75f;

    private SoundManager sound_manager_;
    //private GameObject prefeb_;

    void Start()
    {
        sound_manager_ = SoundManager.Instance;
        //prefeb_ = transform.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Airplane")
        {
            GameController.birdHitCounter++;
			//GameObject.FindGameObjectWithTag("HealthBar").GetComponent<awakeBar>().decrementHealth(damageAmount);
            Instantiate(BirdKillEffect, transform.position, transform.rotation);
            sound_manager_.playBirdSplatSound();
            StartCoroutine(BirdDestruct());
            StartCoroutine(Shake());
        }
    }

    IEnumerator BirdDestruct()
    {
        int count = 50;
        float step = 1 / (float)count;
        while (count > 0)
        {
            transform.localScale = new Vector3(count * step, count * step, count * step);
            transform.localRotation = Quaternion.Euler(0, count * 10, 0);
            transform.localPosition += new Vector3(0, -step, 0);
            count--;
            yield return null;
        }
        Destroy(gameObject);
    }

    // Shake the camera to provide feedback
    IEnumerator Shake()
    {

        float _elapsed = 0.0f;

        Vector3 _originalCamPos = Camera.main.transform.position;

        while (_elapsed < _duration)
        {

            _elapsed += Time.deltaTime;

            float percentComplete = _elapsed / _duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            //float y = Random.value * 2.0f - 1.0f;
            x *= _magnitude * damper;
           // y *= _magnitude * damper;

            Camera.main.transform.position = new Vector3(x, _originalCamPos.y, _originalCamPos.z);

            yield return null;
        }

        Camera.main.transform.position = _originalCamPos;
    }
}
