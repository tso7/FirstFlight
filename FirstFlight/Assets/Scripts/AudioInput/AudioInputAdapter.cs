using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public delegate float VolumeSelectHandler (string dev);
public delegate int SpectrumSelectHandler (string dev);
public enum InDev {
    kMic,
    kPhidgets
}

public class AudioInputAdapter : GenericSingleton<AudioInputAdapter> {

    #region vars
    public AudioMixerGroup target;
    public event VolumeSelectHandler VolumeSelect;
    public event SpectrumSelectHandler SpectrumSelect;

    private Hashtable audioin_;
    private float sensitivity_ = 1000;
    private static int devserial;
    private const int kVolumeSample = 2048;
    private const int kSpectrumSample = 64;
    #endregion

    #region props
    public static InDev InputDevice {
        get;
        private set;
    }
    #endregion

    #region public_methods
    /******************************************
    Use these two methods to get the input data
    ******************************************/
    public float GetInputVolume (string dev) {
        if (Input.GetKey(KeyCode.W))
            return 3;
        if (dev == null)
            return 0;
        float res = VolumeSelect(dev);
        return res;
    }
    public float GetInputSpectrum (string dev) {
        float res = SpectrumSelect(dev);
        return res;
    }
    public static string GetDevice () {
        if (InputDevice != InDev.kMic)
            return null;
        if (Microphone.devices.Length != 0) {
            if (Microphone.devices.Length > devserial) {
                Debug.Log(Microphone.devices[devserial]);
                Debug.Log(devserial);
                return Microphone.devices[devserial];
            }
            else
                Debug.Log(Microphone.devices.Length);
                return Microphone.devices[Microphone.devices.Length - 1];
        }
        return null;
    }
    #endregion

    #region mono_methods

    void Start () {
        devserial = Config.GetDevSerial();
        //Change the input device type here
        InputDevice = InDev.kMic;
        audioin_ = new Hashtable();
        if (InputDevice == InDev.kMic) {
            AudioSource audio;
            string dev = GetDevice();
            audio = gameObject.AddComponent<AudioSource>();
            audio.loop = true;
            audio.outputAudioMixerGroup = target;
            audio.ignoreListenerPause = true;
            if (dev != null) {
                audio.clip = Microphone.Start(dev, true, 1, 48000);
                if (!audioin_.ContainsKey(dev))
                    audioin_.Add(dev, audio);
            }
            audio.Play();
            VolumeSelect += MicInputVolume;
            SpectrumSelect += MicInputFreq;
        }
        else {
            VolumeSelect += PhidgetsInputVolume;
        }
        gameObject.AddComponent<AudioHighPassFilter>().cutoffFrequency = 65;
        gameObject.AddComponent<AudioLowPassFilter>().cutoffFrequency = 1200;
    }
    #endregion

    #region private_methods
    private float MicInputVolume (string dev) {
        if (dev == null)
            return 0;
        float[] _samples = new float[kVolumeSample];
        float sum = 0;
        if (!audioin_.ContainsKey(dev)) {
            Debug.LogError("Invalid Device");
            return 0;
        }
        AudioSource audio;
        if (audioin_.ContainsKey(dev))
            audio = audioin_[dev] as AudioSource;
        else
            return 0;
        if (audio != null)
            audio.GetOutputData(_samples, 0);
        foreach (float _sample in _samples) {
            sum += Mathf.Abs(_sample);
        }
        return sensitivity_ * sum / kVolumeSample;
    }

    private int MicInputFreq (string dev) {
        if (dev == null)
            return -1;
        float[] samples = new float[kSpectrumSample];
        if (!audioin_.ContainsKey(dev)) {
            Debug.LogError("Invalid Device");
            return 0;
        }
        AudioSource audio = audioin_[dev] as AudioSource;
        audio.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        int peak = 0;
        float max = 0;
        for (int i = 0; i < samples.Length; i++) {
            if (samples[i] > max) {
                max = samples[i];
                peak = i;
            }
        }
        return peak;
    }

    private float PhidgetsInputVolume (string dev) {
        return 1.0f;
    }
    #endregion
}
