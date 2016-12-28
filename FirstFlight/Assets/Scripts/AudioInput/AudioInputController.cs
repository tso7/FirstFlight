using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum Edge {
    kFlat,
    kRising,
    kFalling
}

public class AudioInputController : GenericSingleton<AudioInputController> {

    #region vars
    private const float kThreshold = 1;

    private const int kSampleLength = 6;
    private static AudioInputAdapter kAudioIn;
    private static GameController kGameCtrl;
    private int riscount_;
    private Queue<float> buffer_;
    #endregion

    #region props
    public bool IsActive {
        get; private set;
    }

    public bool IsInput {
        get; private set;
    }
    #endregion

    #region publicmethods
    public void StartDetect () {
        IsActive = true;
        IsInput = false;
        StartCoroutine(BufferSetup());
    }

    public void StopDetect () {
        IsActive = false;
        buffer_.Clear();
    }

    public bool IsRising () {
        return EdgeDetect() == Edge.kRising;
    }
    #endregion

    #region monomethods
    void Awake () {
        buffer_ = new Queue<float>();
        kAudioIn = AudioInputAdapter.Instance;
        kGameCtrl = GameController.Instance;
        IsInput = false;
    }

    void Update () {
        if (buffer_.Count <= kSampleLength) {
            IsActive = false;
            StartCoroutine(BufferSetup());
        }
        if (IsActive) {
            StreamIn(kAudioIn.GetInputVolume(AudioInputAdapter.GetDevice()));
            if (!IsInput) {
                if (EdgeDetect() == Edge.kRising|| Input.GetKey(KeyCode.P)) {
                    riscount_++;
                    Debug.Log("Rising Count: " + riscount_);
                    if (riscount_ >= 3) {
                        IsInput = true;
                        StopDetect();
                        kGameCtrl.OnGameStart();
                    }
                }
            }
        }
    }
    #endregion

    #region privatemethods
    float StreamIn (float _in) {
        buffer_.Enqueue(_in);
        return buffer_.Dequeue();
    }

    IEnumerator BufferSetup () {
        while (buffer_.Count <= kSampleLength) {
            buffer_.Enqueue(kAudioIn.GetInputVolume(AudioInputAdapter.GetDevice()));
            yield return null;
            IsActive = true;
        }
    }

    Edge EdgeDetect () {
        float _temp = buffer_.Peek();
        int _count = 0;
        foreach (float _sample in buffer_) {
            if (Mathf.Abs(_sample - _temp) > kThreshold) {
                _count++;
            }
            _temp = _sample;
        }
        if (_count != 1) {
            return Edge.kFlat;
        }
        else if (buffer_.Peek() > kThreshold && IsInput) {
            return Edge.kFalling;
        }
        else if (buffer_.Peek() <= kThreshold && !IsInput) {
            return Edge.kRising;
        }
        else {
            return Edge.kFlat;
        }
    }

    #endregion

}
