using UnityEngine;

public class Character_Jump : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();

            if (animator != null)
            {
                Debug.Log("Animator encontrado: " + animator.name);
            }
            else
            {
                Debug.LogError("❌ No se encontró ningún Animator en este objeto o sus hijos.");
            }
        }
    }

    void Update()
    {
        // Salto con click izquierdo o barra espaciadora
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("🖱️ Click izquierdo o Espacio detectado: activando salto");
            animator.SetTrigger("Jump");
        }
    }
}
