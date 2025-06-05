using UnityEngine;

public class MuityUI : MonoBehaviour
{
    public GameObject MultyPanel;

    void Start() {
        MultyPanel.SetActive(false);
    }

    public void ToggleMultyPanel(){
        MultyPanel.SetActive(!MultyPanel.activeSelf);
    }
}
