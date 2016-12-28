using UnityEngine;
using System.Collections;

public class PlaneFlip : MonoBehaviour {

    // Vars
    public GameObject kCamera;
    public GameObject kPlane;
    /*
    private float _travelSpeed = 2f;
    private float _flipSpeed = 49.5f;
    private float _maxLimitZ = 6.5f;

    // Boooooooooool
    private bool _isTurning = false;
    private bool _turnCompleted = false;

    private float _posY = 0;
    private Vector3 _startPos;
    private float _startTime;
    private float _totalLength, _distanceCovered;*/

    private const float kEaseL = 5.0f;
    private float camrot_;
    private float planerot_;
    private float start_;
    private bool isturn_;
    private bool completed_;

    void Start () {
        completed_ = false;
    }
    
	void Update(){
        if (completed_) {
            kPlane.transform.rotation = Quaternion.identity;
            Destroy(this);
        }
        Flip();
	}

    public void Flip () {
        if (!isturn_ && !completed_) { 
            isturn_ = true;
            start_ = Time.time;
        }
        if (isturn_) {
            camrot_ = Mathf.Clamp01((Time.time - start_) / kEaseL) * 180;
            planerot_ = Mathf.Clamp01((Time.time - start_) / kEaseL) * 360;
            kCamera.transform.rotation = Quaternion.Euler(90, camrot_, 0);
            kPlane.transform.rotation = Quaternion.Euler(0, 0, planerot_);
            if (Time.time - start_ > kEaseL) {
                completed_ = true;
            }
        }
    }

    /*
    public void BeginFlip()
    {
        if (!_isTurning)
        {
            _isTurning = true;
            _startPos = kPlane.transform.position;
            _startTime = Time.time;
            _totalLength = Vector3.Distance(_startPos, Vector3.zero);

        }
        else if (kCamera.transform.rotation.eulerAngles.y > 180 && kCamera.transform.rotation.eulerAngles.y != 0)
        {
            _isTurning = false;
            _turnCompleted = true;
        }

        if (_isTurning && !_turnCompleted)
        {
            _distanceCovered = (Time.time - _startTime) * _travelSpeed / 1.8f;
            float _frac = Mathf.Clamp01(_distanceCovered / _totalLength);
            kPlane.transform.position = Vector3.Lerp(_startPos, Vector3.zero, _frac);
            kPlane.transform.Rotate(kPlane.transform.right * Time.deltaTime * _flipSpeed * - 1.2f);

            _posY += 0.5f;
            kCamera.transform.rotation = Quaternion.Euler(90, _posY, 0);
        }
        else if (!_turnCompleted)
        {
            transform.position += transform.forward * Time.deltaTime * _travelSpeed;
        }
    }*/

}
