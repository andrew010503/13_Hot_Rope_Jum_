using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Método para cambiar de escena por nombre
    public void LoadScene(string GameScene)
    {
        SceneManager.LoadScene(GameScene);
    }

    // Método opcional para salir del juego
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Juego cerrado");
    }
}
