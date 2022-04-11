using TMPro;
using UnityEngine;

public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;
    private static TextMeshProUGUI _staticTxt;
    
    private void Start()
    {
        _staticTxt = txt;
        UpdateCoinsUI();
    }

    public static void UpdateCoinsUI()
    {
        if (_staticTxt == null) return;
        _staticTxt.SetText(PlayerPrefs.GetInt("Coins", 0).ToString());
    }
}
