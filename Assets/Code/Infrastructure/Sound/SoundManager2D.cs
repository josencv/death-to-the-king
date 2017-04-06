using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Assets.Code.Infrastructure.Sound
{
    public class SoundManager2D : ISoundManager
    {
        private const int MaxLoopingSources = 32;
        private const int OneShotPriority = 0;
        private const int LoopBasePriority = 1;
        private float masterVolume;

        /// <summary>
        /// Used to hold the audio source for one shot sound effects.
        /// </summary>
        private GameObject oneShotSoundObject;

        /// <summary>
        /// Audio source used to play one shot sound effects
        /// </summary>
        private AudioSource oneShotSoundSource;

        /// <summary>
        /// Holds the audio sources used for looping sounds
        /// </summary>
        private GameObject loopingSoundObject;

        /// <summary>
        /// A collection of AudioSources reserved for looping that are unused.
        /// </summary>
        private List<AudioSource> freeLoopingSources;

        //A map of channel identifiers with their audio source
        private Dictionary<string, AudioSource> usedLoopingSources;

        public SoundManager2D()
        {
            masterVolume = 0.3f;
            oneShotSoundObject = new GameObject();
            oneShotSoundObject.name = "One Shot SFX Object";
            UnityEngine.Object.DontDestroyOnLoad(oneShotSoundObject);
            oneShotSoundSource = AddAudioSource(oneShotSoundObject, OneShotPriority);

            loopingSoundObject = new GameObject();
            loopingSoundObject.name = "Looping Sound Object";
            UnityEngine.Object.DontDestroyOnLoad(loopingSoundObject);

            freeLoopingSources = new List<AudioSource>(MaxLoopingSources);
            for (int i = 0; i < MaxLoopingSources; ++i)
            {
                AudioSource loopSource = AddAudioSource(loopingSoundObject, LoopBasePriority);
                freeLoopingSources.Add(loopSource);
            }

            usedLoopingSources = new Dictionary<string, AudioSource>();
        }

        /// <summary>
        /// Plays a sound effect that does not loop.
        /// </summary>
        /// <param name="clip">The AudioClip to play.</param>
        /// <param name="volume">The volume scale from 0 to 1.0f.</param>
        public void PlayOneShotSound(AudioClip clip, float volume = 1.0F)
        {
            if (clip != null)
            {
                oneShotSoundSource.PlayOneShot(clip, volume * masterVolume);
            }
        }

        /// <summary>
        /// Plays a sound effect that does not loop randomly taken from an array. 
        /// </summary>
        /// <param name="clip">The AudioClip to play.</param>
        /// <param name="volume">The volume scale from 0 to 1.0f.</param>
        public void PlayOneShotSoundRandom(AudioClip[] list, float volume = 1.0F)
        {
            if (list != null && list.Length > 0)
            {
                oneShotSoundSource.PlayOneShot(list[UnityEngine.Random.Range(0, list.Length)], volume * masterVolume);
            }
        }

        /// <summary>
        /// Stops all one shot sounds.
        /// </summary>
        public void StopAllOneShotSounds()
        {
            oneShotSoundSource.Stop();
        }

        /// <summary>
        /// Plays a looping sound.  The AudioClip supplied will be assigned
        /// to its own AudioSource, identified by the soundID provided.
        /// 
        /// The priority value should be in the range 1-255.  0 is reserved for
        /// one shot sound effects.  The lower the value the less likely the
        /// sound will be mixed out.  A runtime error will be thrown if the
        /// priority value is outside the accepted range.
        /// 
        /// By default the system supports 32 looping sounds concurrently.  This
        /// value can be edited in SoundManager2D.cs - 
        /// private const int NUM_LOOPING_SOURCES
        /// </summary>
        /// <param name="clip">The AudioClip to play.</param>
        /// <param name="soundID">The ID of the looping audio.</param> 
        /// <param name="priority">The priority of the Audio Source.  Should
        /// be in the range 1-255.  0 is reserved for one shot sounds.</param>
        /// <param name="volume">The volume of the clip 0.0f to 1.0f.</param>
        public void PlayLoopingSound(AudioClip clip, string soundID, int priority, float volume = 1.0F)
        {
            if (priority < 1 || priority > 255)
            {
                throw new ArgumentException("Looping Audio Source priority should be in the range 1-255");
            }

            // If the soundID already exists, the source is restarted with the new values
            if (usedLoopingSources.ContainsKey(soundID))
            {
                AudioSource existingSource = usedLoopingSources[soundID];
                StartLoopingSource(existingSource, clip, priority, volume);
            }
            else
            {
                if (freeLoopingSources.Count < 1)
                {
                    throw new Exception("All looping Audio Sources are in use.");
                }

                // Take the first unused source and move it to the used sources
                AudioSource newSource = freeLoopingSources.First();
                freeLoopingSources.Remove(newSource);
                usedLoopingSources.Add(soundID, newSource);

                StartLoopingSource(newSource, clip, priority, volume);
            }
        }

        /// <summary>
        /// Plays a looping sound by resource path.
        /// </summary>
        /// <param name="resourceName">The path of the resource to load</param>
        /// <param name="soundID">The ID of the looping audio.</param> 
        /// <param name="priority">The priority of the Audio Source.  Should
        /// be in the range 1-255.  0 is reserved for one shot sounds.</param>
        /// <param name="volume">The volume of the clip 0.0f to 1.0f.</param>
        public void PlayLoopingSound(string resourceName, string soundID, int priority, float volume = 1.0F)
        {
            AudioClip clip = (AudioClip)Resources.Load(resourceName, typeof(AudioClip));
            PlayLoopingSound(clip, soundID, priority, volume);
        }

        /// <summary>
        /// Stops a looping sound.  Throws an exception if the sound is not
        /// playing.
        /// </summary>
        /// <param name="soundID">The ID of the sound to stop.</param>
        public void StopLoopingSound(string soundID)
        {
            if (!usedLoopingSources.ContainsKey(soundID))
            {
                throw new System.Exception("Tried to stop looping sound \"" + soundID + "\" while it was " + "not playing.");
            }

            // The audio is moved from the used sources to the unused list
            AudioSource sourceToStop = usedLoopingSources[soundID];
            sourceToStop.Stop();
            usedLoopingSources.Remove(soundID);

            freeLoopingSources.Add(sourceToStop);
        }

        /// <summary>
        /// Stops all looping sounds.
        /// </summary>
        public void StopAllLoopingSounds()
        {
            List<string> allKeys = new List<string>(usedLoopingSources.Keys);
            foreach (string soundID in allKeys)
            {
                StopLoopingSound(soundID);
            }
        }

        /// <summary>
        /// Adds an audio source to the game object supplied, sets its priority
        /// and then returns it.
        /// </summary>
        /// <param name="gameObject">The game object to add the audio source to.
        /// </param>
        /// <param name="priority">The priority to set the Audio Source 
        /// to.  Should be an int between 0 and 255.  Lower number means higher
        /// priority.</param>
        private AudioSource AddAudioSource(GameObject gameObject, int priority)
        {
            if (priority < 0 || priority > 255)
            {
                throw new ArgumentException("Audio Source priority should be in the range 0-255");
            }

            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.priority = priority;

            return audioSource;
        }

        /// <summary>
        /// Starts the looping source.
        /// </summary>
        /// <param name="source">The AudioSource to use.</param>
        /// <param name="clip">The AudioClip to play.</param>
        /// <param name="priority">The priority of the AudioSource.</param>
        /// <param name="volume">The volume to play the sound at.</param>
        private void StartLoopingSource(AudioSource source, AudioClip clip, int priority, float volume)
        {
            source.loop = true;
            source.clip = clip;
            source.priority = priority;
            source.volume = volume * masterVolume;

            source.Play();
        }

    }
}