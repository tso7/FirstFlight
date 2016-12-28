using UnityEngine;
using System.Collections;

public static class Extensions {

    #region AudioSource
    //AudioSource Extension
    private static AudioController store_;

    public static void PlayTrack (this AudioSource source, string track) {
        if (!source.isPlaying) {
            store_ = AudioController.Instance;
            source.clip = store_.TrackName(track);
            source.Play();
        }
        else
            Debug.LogError("This audiosource is already occupied");
    }
    #endregion
}
