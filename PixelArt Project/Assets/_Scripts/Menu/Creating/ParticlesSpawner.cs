using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Scripts.GeneralLogic.Menu.Creating
{
    public class ParticlesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject particle;
        [SerializeField] private AnimationClip clip;
        [SerializeField] private RectTransform rect;
        
        private float _pixelWidth;
        private float _pixelHeight;

        private const float SpawnRate = 0.5f;
        private const float SpawnExtraRange = 0.1f;
        private const int SpawnMaxCount = 3;
        private const int BoundsOffset = 10;

        private ObjectPool<GameObject> _pool;

        private void Awake()
        {
            var rRect = rect.rect;
            _pixelWidth = rRect.width/2;
            _pixelHeight = rRect.height/2;
        }

        private void Start()
        {
            _pool = new ObjectPool<GameObject>(() => 
                    Instantiate(particle, transform),
                pt =>
            {
                pt.transform.localPosition = RandomPos();
                pt.SetActive(true); 
            }, pt =>
            {
                pt.SetActive(false);
            }, 
                Destroy,false,6,10);
            
            StartCoroutine(Spawner());
        }

        private IEnumerator Spawner()
        {
            while (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0))
            {
                var delay = Random.Range(SpawnRate, SpawnRate+SpawnExtraRange);
                yield return new WaitForSeconds(delay);
                Spawn();
            }
        } 
        private async void Spawn()
        {
            var count = Random.Range(1, SpawnMaxCount+1);
            for (var i = 0; i < count; i++)
            {
                if (this == null) return;
                WaitForRelease(_pool.Get());
                if (i < count - 1) await Task.Delay(100);
            }
        }

        private Vector3 RandomPos()
        {
            return new (Random.Range(-_pixelWidth+BoundsOffset, _pixelWidth-BoundsOffset),
                Random.Range(-_pixelHeight+BoundsOffset, _pixelHeight-BoundsOffset), 0);
        }
        
        private async void WaitForRelease(GameObject g)
        {
            await Task.Delay((int)(clip.length*1000));
            if (this == null) return;
            _pool.Release(g);
        }
    }
}
