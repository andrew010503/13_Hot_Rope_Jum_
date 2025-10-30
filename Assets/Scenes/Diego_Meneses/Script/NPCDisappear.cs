using UnityEngine;

public class NPCDisappear : MonoBehaviour
{
    public Animator animator;
    private bool isDying = false;
    private float dieTimer = 0f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null) return;

        // Verifica si está en la animación "A_Die"
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("A_Die"))
        {
            if (!isDying)
            {
                isDying = true;
                dieTimer = animator.GetCurrentAnimatorStateInfo(0).length;
                Debug.Log(gameObject.name + ": muerte detectada, desaparecerá en " + dieTimer + "s");
            }

            dieTimer -= Time.deltaTime;
            if (dieTimer <= 0)
            {
                // 🔥 Destruye el objeto raíz del personaje
                Destroy(transform.root.gameObject);
            }
        }
    }
}
