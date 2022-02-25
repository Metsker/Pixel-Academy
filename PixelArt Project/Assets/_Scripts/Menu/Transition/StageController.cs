using System;
using _Scripts.Gameplay.Release.Shared.UI;
using _Scripts.Menu.Creating;
using _Scripts.Menu.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets._Scripts.Menu.Transition
{
    public class StageController : MonoBehaviour
    {
        public GameObject levels;
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform startPoint;
        [Space]
        [SerializeField] private GameObject[] toHideMain;
        [SerializeField] private GameObject[] toHideShop;
        [SerializeField] private GameObject[] particleLayers;
        [SerializeField] private ParticlesSpawner particlesSpawner;
        [Space]
        [SerializeField] private Ease ease;

        private PageManager _pageManager;

        private const float Duration = 0.7f;
        private static Vector3 _endPos;
        public static bool IsAnimating { get; private set; }
        private static int _index;
        
        private const int DefaultCapacity = 5;
        private const int MaxSize = 10;

        private ObjectPool<GameObject> _pool;
        private Sequence _sequence;

        public static event Action CloseUI;

        private void Awake()
        {
            _pageManager = FindObjectOfType<PageManager>();
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
            if (Camera.main != null)
            {
                var cam = Camera.main;
                var p1 = cam.WorldToScreenPoint(startPoint.position);
                var p2 = cam.WorldToScreenPoint(endPoint.position);
                var canvas = transform.root.GetComponent<Canvas>();
                ((RectTransform)levels.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Vector3.Distance(p1,p2)/canvas.scaleFactor);
            }

            _pool = new ObjectPool<GameObject>(() =>
                {
                    var g = Instantiate(CreatingData.creatingData.categoryInstance, CreatingData.creatingData.levelPanel.transform);
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
            if (BlurManager.IsBlured())
            {
                CloseUI?.Invoke();
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
            if (levels.gameObject.activeInHierarchy)
            {
                HideLevelsSilently();
            }
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
            levels.transform.DOMove(startPoint.position, Duration).SetEase(ease).OnComplete(() =>
            {
                foreach (var g in GetObjectsToHide(false))
                {
                    g.SetActive(false);
                }
                particlesSpawner.transform.SetParent(particleLayers[1].transform, false);
                IsAnimating = false;
            });
        }

        public void HideLevels()
        {
            if(IsAnimating) return;
            
            IsAnimating = true;
            foreach (var g in GetObjectsToHide(false))
            {
                g.SetActive(true);
            }
            particlesSpawner.transform.SetParent(particleLayers[0].transform, false);
            levels.transform.DOMove(endPoint.position, Duration).SetEase(ease).OnComplete(() =>
            {
                    levels.SetActive(false);
                    ClearPool();
                    IsAnimating = false;
                });
        }

        private void HideLevelsSilently()
        {
            ClearPool();
            levels.transform.DOMove(endPoint.position, 0);
            foreach (var g in GetObjectsToHide(true))
            {
                g.SetActive(true);
            }
        }
        
        public void SetParticlesLayer(int layer)
        {
            switch (layer)
            {
                case 0 when particlesSpawner.transform != particleLayers[layer].transform:
                    particlesSpawner.transform.SetParent(particleLayers[layer].transform, false);
                    break;
                case 1 when levels.activeInHierarchy:
                    particlesSpawner.transform.SetParent(particleLayers[layer].transform, false);
                    break;
            }
        }

        private void ClearPool()
        {
            foreach (Transform child in CreatingData.creatingData.levelPanel.transform)
            {
                if (child.CompareTag("Released")) continue;
                _pool.Release(child.gameObject);
            }
        }

        private GameObject[] GetObjectsToHide(bool reverse)
        {
            switch (PageManager.CurrentPage)
            {
                case PageManager.Pages.Main:
                    return !reverse ? toHideMain : toHideShop;
                case PageManager.Pages.Shop:
                    return !reverse ? toHideShop : toHideMain;
            }
            return null;
        }
    }
}
