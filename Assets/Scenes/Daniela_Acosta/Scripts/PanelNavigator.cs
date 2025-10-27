using UnityEngine;

public class PanelNavigator : MonoBehaviour
{
    public GameObject[] panels; // index 0 Controls, 1 Tips, 2 Rules
    private int current = 0;

    void Start() { ShowPanel(current); }

    public void Next() { current = (current + 1) % panels.Length; ShowPanel(current); }
    public void Prev() { current = (current - 1 + panels.Length) % panels.Length; ShowPanel(current); }

    void ShowPanel(int idx)
    {
        for (int i = 0; i < panels.Length; i++) panels[i].SetActive(i == idx);
    }
}
