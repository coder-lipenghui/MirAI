using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirAI.Animation
{
    [Serializable]
    public class SpriteAnimationClip
    {
        public string Id;
        public Sprite[] Frames;
        public float FramesPerSecond = 12f;
        public bool Loop = true;
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimationPlayer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private SpriteAnimationClip[] _clips;

        private readonly Dictionary<string, SpriteAnimationClip> _clipLookup = new();
        private SpriteAnimationClip _currentClip;
        private int _frameIndex;
        private float _frameTimer;

        private void Awake()
        {
            if (_renderer == null)
            {
                _renderer = GetComponent<SpriteRenderer>();
            }

            _clipLookup.Clear();
            foreach (var clip in _clips)
            {
                if (clip != null && !string.IsNullOrWhiteSpace(clip.Id))
                {
                    _clipLookup[clip.Id] = clip;
                }
            }
        }

        private void Update()
        {
            if (_currentClip == null || _currentClip.Frames == null || _currentClip.Frames.Length == 0)
            {
                return;
            }

            _frameTimer += Time.deltaTime;
            var frameDuration = 1f / Mathf.Max(1f, _currentClip.FramesPerSecond);

            while (_frameTimer >= frameDuration)
            {
                _frameTimer -= frameDuration;
                AdvanceFrame();
            }
        }

        public void Play(string clipId)
        {
            if (string.IsNullOrWhiteSpace(clipId))
            {
                return;
            }

            if (!_clipLookup.TryGetValue(clipId, out var clip))
            {
                return;
            }

            if (_currentClip == clip)
            {
                return;
            }

            _currentClip = clip;
            _frameIndex = 0;
            _frameTimer = 0f;
            ApplyFrame();
        }

        private void AdvanceFrame()
        {
            if (_currentClip == null)
            {
                return;
            }

            _frameIndex++;
            if (_frameIndex >= _currentClip.Frames.Length)
            {
                if (_currentClip.Loop)
                {
                    _frameIndex = 0;
                }
                else
                {
                    _frameIndex = _currentClip.Frames.Length - 1;
                }
            }

            ApplyFrame();
        }

        private void ApplyFrame()
        {
            if (_currentClip == null || _currentClip.Frames == null || _currentClip.Frames.Length == 0)
            {
                return;
            }

            _renderer.sprite = _currentClip.Frames[_frameIndex];
        }
    }
}
