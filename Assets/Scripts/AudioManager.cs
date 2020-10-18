using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    public class AudioManager : MonoBehaviour
    {

        public Sound[] audioClips;

        public static AudioManager reference;

    public bool blockCollisions = true;


    // Use this for initialization
    void Start()
        {
        
        }
        public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
        private void Awake()
        {
            if (reference == null)
            {
                reference = this;

            }
            else
            {
                Destroy(this);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
            foreach (Sound sound in audioClips)
            {
                sound.source = this.gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.audio;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
            playSound("bgm01");
            
        }

        public void playSound(string name)
        {
            
            Sound s = Array.Find<Sound>(audioClips, Sound => Sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Missing Sound: " + name);
            }
            else
            {
                s.source.time = s.startTime;
            s.source.priority = 0;
            s.source.maxDistance = 1000f;
            s.source.Play();
           
            }
        }

        public AudioSource playSoundFromObject(string name, GameObject newSource, bool destroyPostClip)
        {
      
            Sound s = Array.Find<Sound>(audioClips, Sound => Sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Missing Sound: " + name);
                return null;
            }
            else
            {
                AudioSource source = newSource.AddComponent<AudioSource>();
                source.clip = s.audio;
                source.volume = s.volume;
                source.pitch = s.pitch;
                source.loop = s.loop;
            source.priority = 255;
                source.Play();
                if (destroyPostClip)
                {
                    Destroy(source, source.clip.length);
                }
                source.time = s.startTime;
            
                return source;
            }
        }
    public AudioSource playSoundFromObject(string name, GameObject newSource, bool destroyPostClip, float force)
    {

        Sound s = Array.Find<Sound>(audioClips, Sound => Sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Missing Sound: " + name);
            return null;
        }
        else
        {
            AudioSource source = newSource.AddComponent<AudioSource>();
            source.clip = s.audio;
            source.volume = s.volume*force;
            source.pitch = s.pitch;
            source.loop = s.loop;
            source.priority = 255;
            source.Play();
            if (destroyPostClip)
            {
                Destroy(source, source.clip.length);
            }
            source.time = s.startTime;

            return source;
        }
    }
}
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip audio;

        [HideInInspector]
        public AudioSource source;
        [Range(0f, 1f)]
        public float volume;
        [Range(-1f, 3f)]
        public float pitch;
        public bool loop;
        public float startTime = 0;

    }


