using UnityEngine;
using TMPro;

public class JumpCounter : MonoBehaviour
{
    public static JumpCounter Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI jumpText;

    [Header("Configuración")]
    public string textPrefix = "Saltos: ";

    private int jumpCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateDisplay();
    }

    public void AddJump()
    {
        jumpCount++;
        UpdateDisplay();
        Debug.Log("🎯 Salto registrado! Total: " + jumpCount);
    }

    public void ResetCounter()
    {
        jumpCount = 0;
        UpdateDisplay();
        Debug.Log("🔄 Contador de saltos reiniciado");
    }

    public int GetJumpCount()
    {
        return jumpCount;
    }

    void UpdateDisplay()
    {
        if (jumpText != null)
        {
            jumpText.text = textPrefix + jumpCount;
        }
    }
}
