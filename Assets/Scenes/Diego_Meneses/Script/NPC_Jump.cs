using UnityEngine;
using System.Collections;

public class NPCJumpRopeController : MonoBehaviour
{
    [Header("Referencias")]
    public Animator animator;
    public RopeRotation rope; // referencia al script de la cuerda

    [Header("Parámetros de salto")]
    public float jumpDelay = 0.1f; // margen de tiempo para saltar correctamente
    public float failChance = 0.2f; // probabilidad aleatoria de fallar (20%)
    public float disappearDelay = 2f;

    [Header("Audio")]
    public AudioSource audioSource;  // Para reproducir el sonido
    public AudioClip saltoClip;      // Tu clip SALTO

    private bool isDead = false;

    void Awake()
    {
        // Asegurar que el Animator esté asignado correctamente
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Verificar que la cuerda esté asignada
        if (rope == null)
        {
            rope = FindObjectOfType<RopeRotation>();
            if (rope == null)
            {
                Debug.LogError("No se encontró el script RopeRotation en la escena.");
            }
        }
    }

    void Update()
    {
        if (isDead || rope == null) return;

        // Asegurarse de que el Animator está configurado y activo antes de usarlo
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning($"{name}: El Animator no tiene un AnimatorController asignado.");
            return;
        }

        // Detectar fase del movimiento de la cuerda
        float ropePhase = Mathf.Sin(Time.time * rope.verticalSpeed);

        // Si la cuerda está abajo (cerca de 0), decidir si salta o falla
        if (ropePhase < -0.9f)
        {
            if (Random.value > failChance)
            {
                // Ejecutar animación de salto
                animator.SetTrigger("Jump");

                // Reproducir sonido de salto
                if (audioSource != null && saltoClip != null)
                {
                    audioSource.PlayOneShot(saltoClip);
                }
            }
            else
            {
                // Fallo al saltar
                StartCoroutine(Fail());
            }
        }
    }

    private IEnumerator Fail()
    {
        isDead = true;

        // Validar que el Animator esté listo antes de usarlo
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger("Die");
        }
        else
        {
            Debug.LogWarning($"{name}: No se pudo ejecutar animación 'Die' porque el Animator no está configurado.");
        }

        yield return new WaitForSeconds(disappearDelay);

        // Destruir el NPC
        Destroy(gameObject);
    }
}
