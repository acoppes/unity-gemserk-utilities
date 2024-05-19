using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Models
{
    public class Model : MonoBehaviour
    {
        public Transform model;
        
        public SpriteRenderer spriteRenderer;
        public SpriteColorRemapShader remapShader;

        public SortingGroup sortingGroup;

        public ParticleSystem particleSystem;

        [NonSerialized] 
        public Transform cachedTransform;

        public bool hasSpriteRenderer;

        private void Awake()
        {
            cachedTransform = this.GetComponent<Transform>();
            hasSpriteRenderer = spriteRenderer != null;
        }

        private void OnEnable()
        {
            if (particleSystem != null)
            {
                particleSystem.Play(true);
            }
        }

        private void OnDisable()
        {
            if (particleSystem != null)
            {
                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        public void ResetModel()
        {
            transform.localScale = Vector3.one;
            
            if (model != null)
            {
                model.localScale = Vector3.one;
                model.localEulerAngles = Vector3.zero;
            }
        }
    }
}
