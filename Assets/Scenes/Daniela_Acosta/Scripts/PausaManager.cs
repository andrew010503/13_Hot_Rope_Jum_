using UnityEngine;
using UnityEngine.SceneManagement;

public class PausaManager : MonoBehaviour
{
    public GameObject panelPausa;
    private bool juegoPausado = false;

    public void PausarJuego()
    {
        juegoPausado = !juegoPausado;
        panelPausa.SetActive(juegoPausado);



        // Detener o reanudar el tiempo
        Time.timeScale = juegoPausado ? 0 : 1;
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
