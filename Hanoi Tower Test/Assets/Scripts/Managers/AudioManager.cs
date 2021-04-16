using UnityEngine;

using Mikabrytu.HanoiTower.Events;
using System;

namespace Mikabrytu.HanoiTower
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource[] _sfxSources;

        private void Start()
        {
            PlayMusic();

            EventManager.AddListener<OnRingMoveEvent>(PlayRingClickSFX);
            //EventManager.AddListener<OnRingDropEvent>(PlayRingDropSFX);
            EventManager.AddListener<OnRingFallEvent>(PlayRingFallSFX);
            EventManager.AddListener<OnRingFlyEvent>(PlayRingFlySFX);
        }

        private void PlayMusic()
        {
            _musicSource.Play();
        }

        private void PlayRingClickSFX(OnRingMoveEvent e)
        {
            _sfxSources[0].Play();
        }

        private void PlayRingDropSFX(OnRingDropEvent e)
        {
            _sfxSources[1].Play();
        }

        private void PlayRingFallSFX(OnRingFallEvent e)
        {
            _sfxSources[2].Play();
        }

        private void PlayRingFlySFX(OnRingFlyEvent e)
        {
            _sfxSources[3].Play();
        }
    }
}
