using Lean.Transition;
using UnityEngine;

namespace Crossfire.Workspace
{
    public sealed class AudioService
    {
        private readonly AudioSource _audioSource;

        public AudioService(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void SetMainThemeVolume(float volume)
        {
            _audioSource.volumeTransition(volume, 1f);
        }
    }
}