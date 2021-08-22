using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameplayMod.UI
{
    public class ParticlesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject particle;
        [SerializeField] private AnimationClip clip;
        [SerializeField] private RectTransform rect;

        private float _pixelWidth;
        private float _pixelHeight;
        private const float RepeatingRate = 0.15f;
        private const int Offset = 10;

        private void Start()
        {
            var rRect = rect.rect;
            _pixelWidth = rRect.width/2;
            _pixelHeight = rRect.height/2;
            InvokeRepeating(nameof(Spawn),0,RepeatingRate);
        }

        private async void Spawn()
        {
            var range = new Vector2(Random.Range(-_pixelWidth+Offset, _pixelWidth-Offset),
                Random.Range(-_pixelHeight+Offset, _pixelHeight-Offset));

            var g = Instantiate(particle, range, particle.transform.rotation);
            g.transform.SetParent(gameObject.transform, false);
            await Task.Delay((int)(clip.length*1000));
            if (g == null) return;
            Destroy(g);
        }
    }
}
