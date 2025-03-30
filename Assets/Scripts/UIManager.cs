using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI finalTime;
    [SerializeField] private GameObject inGameUI;

    public void ToggleDeathPanel()
    {
        deathPanel.SetActive(!deathPanel.activeSelf);
        inGameUI.SetActive(false);
    }

    public void ToggleWinPanel()
    {
        winPanel.SetActive(!winPanel.activeSelf);
        inGameUI.SetActive(false);
        finalTime.text = "Final Time: " + inGameUI.GetComponentInChildren<TextMeshProUGUI>().text;
    }
}
