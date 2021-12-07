using System;
using TMPro;
using UnityEngine;

namespace GameplayMod.Creating
{
    public class LevelCreationData : MonoBehaviour
    {
        [Header("Dependencies")]
        public GameObject categoryInstance;
        public GameObject levelPanel;
        public Sprite filledStar;
        public TextMeshProUGUI label;
    }
}
