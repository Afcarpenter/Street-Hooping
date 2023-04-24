using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    private void Start()
    {

    }

    public void PlaySound(AudioSource source, string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        source.pitch = s.pitch;
        source.PlayOneShot(s.clip, s.volumeScale);
    }

    public void PlaySoundRandomPitch(AudioSource source, string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        source.pitch = UnityEngine.Random.Range(1 - s.pitchVariationLimit, 1 + s.pitchVariationLimit);
        source.PlayOneShot(s.clip, s.volumeScale);
    }
}
