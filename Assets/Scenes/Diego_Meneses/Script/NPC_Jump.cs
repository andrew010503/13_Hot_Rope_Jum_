<<<<<<< Updated upstream
﻿using UnityEngine;
using System.Collections;
=======
using UnityEngine;
>>>>>>> Stashed changes

public class NPC_Jump : MonoBehaviour
{
<<<<<<< Updated upstream
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

=======
    [Header("Componentes")]
    public Animator animator;
    public RopeRotation rope;

    [Header("Configuración del salto")]
    public float horizontalJumpDistance = 3.0f;
    public float jumpAnticipation = 0.0f;
    [Range(0f, 1f)] public float failChance = 0.2f;

    private bool hasJumped = false;
>>>>>>> Stashed changes
    private bool isDead = false;
    private bool isImmune = false;
    private Vector3 lastRopePos;
    private float ropePassCooldown = 0f;
    private float immunityTimer = 0f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (rope == null)
            rope = FindObjectOfType<RopeRotation>();

        if (rope != null)
        {
            lastRopePos = rope.GetRopeWorldPosition();
            Debug.Log(name + ": ✅ NPC conectado a la cuerda " + rope.name);
        }
        else
        {
            Debug.LogError(name + ": ❌ No se encontró la cuerda en la escena");
        }

        if (animator != null)
        {
            Debug.Log(name + ": === PARÁMETROS DEL ANIMATOR ===");
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                Debug.Log(name + ": Parámetro: " + param.name + " | Tipo: " + param.type);
            }
        }
    }

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
<<<<<<< Updated upstream
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
=======
        if (isDead || rope == null)
            return;

        if (immunityTimer > 0f)
        {
            immunityTimer -= Time.deltaTime;
            if (immunityTimer <= 0f)
            {
                isImmune = false;
                Debug.Log(name + ": ⏰ Inmunidad terminada - Listo para siguiente salto");
                hasJumped = false;
            }
        }

        if (ropePassCooldown > 0f)
        {
            ropePassCooldown -= Time.deltaTime;
            return;
        }

        Vector3 ropePos = rope.GetRopeWorldPosition();
        Vector3 npcPos = transform.position;

        Vector2 ropePos2D = new Vector2(ropePos.x, ropePos.z);
        Vector2 npcPos2D = new Vector2(npcPos.x, npcPos.z);
        Vector2 lastRopePos2D = new Vector2(lastRopePos.x, lastRopePos.z);

        float currentDist = Vector2.Distance(npcPos2D, ropePos2D);
        float lastDist = Vector2.Distance(npcPos2D, lastRopePos2D);

        bool isApproaching = currentDist < lastDist;

        if (isApproaching && currentDist < horizontalJumpDistance && !hasJumped)
        {
            hasJumped = true;
            
            bool willFail = Random.value < failChance;

            if (!willFail)
            {
                Debug.Log(name + ": 🟡 Preparando salto #" + (isImmune ? "X" : "OK") + ", distancia: " + currentDist.ToString("F2"));
                
                if (jumpAnticipation <= 0f)
                {
                    PerformJump();
                }
                else
                {
                    Invoke(nameof(PerformJump), jumpAnticipation);
                }
            }
            else
            {
                Debug.Log(name + ": ❌ Falló el salto a propósito");
            }
        }

        lastRopePos = ropePos;
    }

    void PerformJump()
    {
        if (isDead)
        {
            Debug.Log(name + ": ⚠️ No puede saltar, está muerto");
            return;
        }

        Debug.Log(name + ": 🟢 EJECUTANDO SALTO - Activando trigger 'Jump' en el Animator");
        animator.ResetTrigger("Die");
        animator.SetTrigger("Jump");
        
        float ropePeriod = rope.GetRotationPeriod();
        
        isImmune = true;
        immunityTimer = ropePeriod * 0.9f;
        Debug.Log(name + ": 🛡️ Inmunidad activada por " + immunityTimer.ToString("F2") + " segundos (período de cuerda: " + ropePeriod.ToString("F2") + "s)");
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Rope"))
        {
            if (isImmune)
            {
                Debug.Log(name + ": 🛡️ Inmune a la cuerda, está saltando");
                return;
            }

            RopeRotation ropeScript = other.GetComponentInParent<RopeRotation>();
            if (ropeScript != null && ropeScript.CanHit())
            {
                bool isInJumpAnimation = false;
                
                AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
                
                if (currentState.IsName("A_Jump") ||
                    currentState.IsName("Jump") ||
                    currentState.IsName("Jumping") ||
                    currentState.IsName("jump"))
                {
                    isInJumpAnimation = true;
                    Debug.Log(name + ": ✅ En animación de salto: " + currentState.ToString());
                }
                
                if (!isInJumpAnimation)
                {
                    isDead = true;
                    animator.ResetTrigger("Jump");
                    animator.SetTrigger("Die");
                    ropeScript.DisableHit();
                    Debug.Log(name + ": 💀 Fue golpeado por la cuerda");
                }
                else
                {
                    Debug.Log(name + ": ✅ La cuerda pasó mientras saltaba");
                }
            }
        }
>>>>>>> Stashed changes
    }
}
