using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance {  get; private set; }

    [SerializeField] private TextMeshProUGUI _winText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetWinTextAndActivate(string playerName)
    {
        _winText.gameObject.SetActive(true);
        _winText.text = playerName + " Won";
    }
}
