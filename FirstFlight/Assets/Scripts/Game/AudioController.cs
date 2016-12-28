using UnityEngine;
using System.Collections;

//Audio Controller
public class AudioController : GenericSingleton<AudioController> {
    public AudioClip[] clips;
    private static Hashtable clips_;

    public AudioClip TrackName(string track) {
        return (AudioClip)clips_[track];
    }

    void Awake () {
        clips_ = new Hashtable();
        foreach (AudioClip clip in clips) {
            Debug.Log(clip.name);
            clips_.Add(clip.name, clip);
        }
    }
}
