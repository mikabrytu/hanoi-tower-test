using UnityEngine;

using Mikabrytu.HanoiTower.Events;
using System;

namespace Mikabrytu.HanoiTower
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource[] sfxSources;

        private void Start()
        {
            PlayMusic();

            EventManager.AddListener<OnRingMoveEvent>(PlayRingClickSFX);
            EventManager.AddListener<OnRingFallEvent>(PlayRingFallSFX);
            EventManager.AddListener<OnRingFlyEvent>(PlayRingFlySFX);
        }

        private void PlayMusic()
        {
            musicSource.Play();
        }

        private void PlayRingClickSFX(OnRingMoveEvent e)
        {
            sfxSources[0].Play();
        }

        private void PlayRingFallSFX(OnRingFallEvent e)
        {
            sfxSources[1].Play();
        }

        private void PlayRingFlySFX(OnRingFlyEvent e)
        {
            sfxSources[2].Play();
        }
    }
}
