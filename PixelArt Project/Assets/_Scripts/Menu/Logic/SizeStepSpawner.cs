using UnityEngine;

namespace _Scripts.GeneralLogic.Menu.Logic
{
    public class SizeStepSpawner : MonoBehaviour
    {
        [SerializeField] private Side side;
        [SerializeField] private GameObject stepInstance;
        private const int StepCount = 32;
        public enum Side: byte
        {
            X, Y
        }
        private void Start()
        {
            for (var i = 1; i <= StepCount; i++)
            {
                var s = Instantiate(stepInstance, gameObject.transform);
                var num = i.ToString();
                s.name = num;
                
                var sizeStep = s.GetComponent<SizeStep>();
                sizeStep.text.SetText(num);
                sizeStep.side = side;
            }
        }
    }
}
