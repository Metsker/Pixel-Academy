using _Scripts.GameplayMod.UI;
using _Scripts.GeneralLogic.Menu.Creating;
using _Scripts.GeneralLogic.Menu.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts.GeneralLogic.Menu.Transition
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] private GameObject levels;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform startPoint;
        [Space]
        [SerializeField] private GameObject[] toHide;
        [SerializeField] private GameObject[] particleLayers;
        [SerializeField] private ParticlesSpawner particlesSpawner;
        [Space]
        [SerializeField] private Ease ease;

        private PageManager _pageManager;
        private WarningUI _warningUI;
        
        private CreatingData _creatingData;
        
        private const float Duration = 0.7f;
        private static Vector3 _endPos;
        public static bool IsAnimating { get; private set; }
        private static int _index;
        
        private const int DefaultCapacity = 5;
        private const int MaxSize = 10;

        private ObjectPool<GameObject> _pool;
        private Sequence _sequence;

        private void Awake()
        {
            _creatingData = FindObjectOfType<CreatingData>();
            _pageManager = FindObjectOfType<PageManager>();
            _warningUI = FindObjectOfType<WarningUI>();
        }

        private void OnEnable()
        {
            CategoryBuilder.FillPool += FillPool;
        }
        
        private void OnDisable()
        {
            CategoryBuilder.FillPool -= FillPool;
        }

        private void Start()
        {
            _index = 0;
            IsAnimating = false;
            startPoint.SetParent(levels.transform.parent, true);
            levels.transform.position = endPoint.position;
            
            _pool = new ObjectPool<GameObject>(() =>
                {
                    var g = Instantiate(_creatingData.categoryInstance, _creatingData.levelPanel.transform);
                    g.name = "Level " + _index;
                    _index++;
                    return g;
                },
                ct =>
                {
                    ct.tag = "Pooled";
                    ct.SetActive(true); 
                }, ct =>
                {
                    ct.tag = "Released";
                    ct.SetActive(false);
                }, Destroy,false,DefaultCapacity, MaxSize);
        }
        
#if UNITY_ANDROID
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (_warningUI.IsWarningActive())
            {
                _warningUI.CloseWarning();
            }
            else if (levels.activeSelf)
            {
                HideLevels();
            }
            else
            {
                _pageManager.GotoPreviousPage();
            }
        }
#endif
        
        private void FillPool(int count)
        {
            for (var i = 0; i < count; i++)
            {
                _pool.Get();
            }
            ShowLevels();
        }
        
        private void ShowLevels()
        {
            if(IsAnimating) return;
            IsAnimating = true;
            levels.SetActive(true);
            particlesSpawner.transform.SetParent(particleLayers[1].transform, false);
            levels.transform.DOMove(startPoint.position, Duration).SetEase(ease).OnComplete(() =>
            {
                foreach (var g in toHide)
                {
                    g.SetActive(false);
                }
                particlesSpawner.transform.SetParent(particleLayers[0].transform, false);
                IsAnimating = false;
            });
        }

        public void HideLevels()
        {
            if(IsAnimating) return;
            IsAnimating = true;
            foreach (var g in toHide)
            {
                g.SetActive(true);
            }
            particlesSpawner.transform.SetParent(particleLayers[1].transform, false);
            levels.transform.DOMove(endPoint.position, Duration).SetEase(ease).OnComplete(() =>
                {
                    levels.SetActive(false);
                    particlesSpawner.transform.SetParent(particleLayers[0].transform, false);
                    ClearPool();
                    IsAnimating = false;
                });
        }

        private void ClearPool()
        {
            foreach (Transform child in _creatingData.levelPanel.transform)
            {
                if (child.CompareTag("Released")) continue;
                _pool.Release(child.gameObject);
            }
        }
    }
}
