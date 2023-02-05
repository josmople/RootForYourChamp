using UnityEngine;

public class EmitAudioSource : EmitPrefab<AudioSource> {
    public override void Clean (AudioSource audioSource) {
        audioSource.Stop();
        audioSource.clip = null;
    }
}
