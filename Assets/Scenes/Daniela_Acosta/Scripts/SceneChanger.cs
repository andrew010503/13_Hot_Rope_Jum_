using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Metodo para cambiar de escena por nombre
    public void LoadScene(string GameScene)
    {
        SceneManager.LoadScene(GameScene);
    }

    // Metodo opcional para salir del juego
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Juego cerrado");
    }
}
