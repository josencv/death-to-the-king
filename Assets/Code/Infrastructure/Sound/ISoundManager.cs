using UnityEngine;

namespace Assets.Code.Infrastructure.Sound
{
    public interface ISoundManager
    {
        void PlayOneShotSound(AudioClip clip, float volume = 1.0F);
        void PlayOneShotSoundRandom(AudioClip[] list, float volume = 1.0F);
        void StopAllOneShotSounds();
        void playLoopingSound(AudioClip clip, string soundID, int priority, float volume = 1.0F);
        void StopLoopingSound(string soundID);
        void StopAllLoopingSounds();
    }
}
