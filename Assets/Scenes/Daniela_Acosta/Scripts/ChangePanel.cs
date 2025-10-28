using UnityEngine;

public class ChangePanel : MonoBehaviour
{
    public GameObject panelActual;
    public GameObject panelDestino;

    public void Cambiar()
    {
        panelActual.SetActive(false);
        panelDestino.SetActive(true);
    }
}
