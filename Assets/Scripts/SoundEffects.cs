using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class SoundEffects : MonoBehaviour {
    public AudioClip[] sounds;
    private AudioSource audio;
    private bool fading = false;

    public bool isPlaying {
        get {
            return audio.isPlaying;
        }
    }

	void Start () {
        audio = GetComponent<AudioSource>();
	}

    void Update () {
        if (fading && audio.isPlaying) {
            if (audio.volume > 0.1f) {
                audio.volume -= 1 * Time.deltaTime;
            } else {
                audio.volume = 0f;
                fading = false;
            }
        }
    }

    public void PlayRandom() {
        PlayRandom(0.05f);
    }

    public void PlayRandom(float volume) {
        if (audio.isPlaying) {
            return;
        }
        audio.clip = sounds[Random.Range(0, sounds.Length)];
        audio.volume = volume;
        audio.Play();
    }

    public void FadeOut() {
        fading = true;
    }

    public void SetVolume(float volume) {
        if (!audio.isPlaying) {
            return;
        }

        volume = Mathf.Clamp(volume, 0f, 1f);
        if (volume >= audio.volume) {
            audio.volume += Mathf.Min(volume - audio.volume, 0.5f * Time.deltaTime);
        } else {
            audio.volume -= Mathf.Min(audio.volume - volume, 0.5f * Time.deltaTime);
        }
    }
}
