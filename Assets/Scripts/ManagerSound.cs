using DG.Tweening;
using UnityEngine;

public class ManagerSound : MonoBehaviour {
    public Settings Settings;
    public AudioSource CurrentMusic = null;
    public AudioSource CurrentCrowd = null;
    public float? FadeDuration = 2f;
    public float? SoundEffectMaxDistance = 3f;
    public AudioListener AudioListener;
    public EmitPrefab<AudioSource> AudioSourcePool;

    public void PlayMusic (AudioClip audioClip) {
        if (Settings.MusicMaxVolume == 0f) return;

        var previousMusic = CurrentMusic;
        CurrentMusic = AudioSourcePool.Acquire();
        CurrentMusic.loop = true;
        CurrentMusic.clip = audioClip;
        CurrentMusic.spatialBlend = 0f;
        CurrentMusic.volume = Settings.MusicMaxVolume;
        CurrentMusic.gameObject.name = audioClip.name;

        var tween = DOTween.Sequence();
        if (previousMusic != null) {
            tween.Append(previousMusic.DOFade(0f, FadeDuration.Value));
        }

        tween.OnComplete(() => {
            CurrentMusic.Play();

            if (previousMusic != null) {
                AudioSourcePool.Release(previousMusic);
            }
        });
    }

    public void PlayCrowd (AudioClip audioClip) {
        if (Settings.SoundMaxVolume == 0f) return;

        var previousCrowd = CurrentCrowd;
        CurrentCrowd = AudioSourcePool.Acquire();
        CurrentCrowd.loop = true;
        CurrentCrowd.clip = audioClip;
        CurrentCrowd.spatialBlend = 0f;
        CurrentCrowd.volume = Settings.MusicMaxVolume;
        CurrentCrowd.gameObject.name = audioClip.name;

        var tween = DOTween.Sequence();
        if (previousCrowd != null) {
            tween.Append(previousCrowd.DOFade(0f, FadeDuration.Value));
        }

        tween.OnComplete(() => {
            CurrentCrowd.Play();

            if (previousCrowd != null) {
                AudioSourcePool.Release(previousCrowd);
            }
        });
    }

    public void SetCurrentCrowdVolume (float volume) {
        if (CurrentCrowd != null) {
            CurrentCrowd.volume = volume;
        }
    }

    public (AudioSource, Tween)? PlaySound (AudioClip audioClip, Vector3? location = null, float? delaySecs = null) {
        if (audioClip == null || Settings.SoundMaxVolume == 0f) return null;

        var position = location.HasValue ? location.Value : Vector3.zero;
        var notWithinDistance =
            AudioListener != null &&
            location.HasValue &&
            SoundEffectMaxDistance.HasValue && 
            SoundEffectMaxDistance.Value < Vector3.Distance(AudioListener.transform.position, position);

        if (notWithinDistance) {
            return null;
        }

        var audioSource = AudioSourcePool.Acquire();
        audioSource.loop = false;
        audioSource.clip = audioClip;
        audioSource.transform.position = position;
        audioSource.spatialBlend = (location.HasValue && AudioListener != null) ? 1f : 0f;
        audioSource.volume = Settings.SoundMaxVolume;
        audioSource.gameObject.name = audioClip.name;

        return (audioSource, PlaySoundRoutine(audioSource, delaySecs));
    }

    private Tween PlaySoundRoutine (AudioSource audioSource, float? delaySecs) {
        if (delaySecs.HasValue) {
            audioSource.PlayDelayed(delaySecs.Value);
            return DOTween.Sequence()
                .AppendInterval(audioSource.clip.length + delaySecs.Value)
                .AppendCallback(() => AudioSourcePool.Release(audioSource));
        } else {
            audioSource.Play();
            return DOTween.Sequence()
                .AppendInterval(audioSource.clip.length)
                .AppendCallback(() => AudioSourcePool.Release(audioSource));
        }
    }
}
