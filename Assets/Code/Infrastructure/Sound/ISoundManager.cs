using UnityEngine;

namespace Assets.Code.Infrastructure.Sound
{
    public interface ISoundManager
    {
        void PlayOneShotSound(AudioClip clip, float volume = 1.0F);
        void PlayOneShotSoundRandom(AudioClip[] list, float volume = 1.0F);
        void StopAllOneShotSounds();
        void PlayLoopingSound(AudioClip clip, string soundID, int priority, float volume = 1.0F);
        void PlayLoopingSound(string resourceName, string soundID, int priority, float volume = 1.0F);
        void StopLoopingSound(string soundID);
        void StopAllLoopingSounds();
    }
}
