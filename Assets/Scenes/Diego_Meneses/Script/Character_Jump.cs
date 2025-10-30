using UnityEngine;
using UnityEngine.SceneManagement;

public class Character_Jump : MonoBehaviour
{
    [Header("Componentes")]
    public Animator animator;
    public AudioSource audioSource; // Nuevo componente de audio

    [Header("Sonidos del personaje")]
    public AudioClip jumpSound; //  Sonido al saltar
    public AudioClip deathSound; //  Sonido al morir

    [Header("Configuración de salto")]
    public float jumpHeight = 1.5f;
    public float jumpDuration = 0.5f;

    [Header("Configuración de muerte")]
    public float disappearDelay = 2f;
    public float gameOverDelay = 3f;

    [Header("Estado")]
    public bool isDead = false;

    private bool isImmune = false;
    private float immunityTimer = 0f;
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 startPosition;
    private CapsuleCollider capsuleCollider;

    void Awake()
    {
        isDead = false;
        isImmune = false;
        immunityTimer = 0f;
        isJumping = false;
        jumpTimer = 0f;
        Debug.Log(name + ": 🔵 Character_Jump inicializado - isDead resetado a False");
    }

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator != null)
                Debug.Log("Animator encontrado: " + animator.name);
            else
                Debug.LogError("❌ No se encontró ningún Animator en este objeto o sus hijos.");
        }

        capsuleCollider = GetComponent<CapsuleCollider>();

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            Debug.Log(name + ": ✅ Rigidbody configurado correctamente para detectar triggers");
        }

        startPosition = transform.position;

        // 🎵 Inicializa el AudioSource si no está asignado
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                Debug.LogWarning(name + ": ⚠️ No se encontró AudioSource, agrégalo al personaje.");
        }
    }

    void Update()
    {
        if (isDead)
            return;

        if (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float progress = jumpTimer / jumpDuration;

            if (progress >= 1f)
            {
                isJumping = false;
                transform.position = startPosition;
            }
            else
            {
                float yOffset = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
                Vector3 newPos = startPosition;
                newPos.y += yOffset;
                transform.position = newPos;
            }
        }

        if (isImmune)
        {
            immunityTimer -= Time.deltaTime;
            if (immunityTimer <= 0f)
            {
                isImmune = false;
                Debug.Log(name + ": ⏰ Inmunidad terminada");
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("🖱️ Click izquierdo o Espacio detectado: activando salto");
            PerformJump();
        }
    }

    void PerformJump()
    {
        if (isDead || isJumping)
        {
            Debug.Log(name + ": ⚠️ No puede saltar ahora");
            return;
        }

        Debug.Log(name + ": 🟢 EJECUTANDO SALTO - Activando trigger 'Jump' en el Animator");
        animator.ResetTrigger("Die");
        animator.SetTrigger("Jump");

        // 🎵 Reproduce sonido de salto
        if (audioSource != null && jumpSound != null)
            audioSource.PlayOneShot(jumpSound);

        isJumping = true;
        jumpTimer = 0f;
        startPosition = transform.position;

        isImmune = true;
        immunityTimer = jumpDuration + 0.1f;
        Debug.Log(name + ": 🛡️ Inmunidad activada por " + immunityTimer.ToString("F2") + " segundos");

        if (JumpCounter.Instance != null)
            JumpCounter.Instance.AddJump();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(name + ": 🔔 TRIGGER DETECTADO con: " + other.name + " | Tag: " + other.tag + " | isDead: " + isDead + " | isImmune: " + isImmune);

        if (isDead)
            return;

        if (other.CompareTag("Rope"))
        {
            if (isImmune)
            {
                Debug.Log(name + ": 🛡️ Inmune a la cuerda, está saltando (tiempo restante: " + immunityTimer.ToString("F2") + "s)");
                return;
            }

            Debug.Log(name + ": 💀 Fue golpeado por la cuerda - GAME OVER");
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        isImmune = false;

        animator.ResetTrigger("Jump");
        animator.SetTrigger("Die");

        // 💀 Reproduce sonido de muerte
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        Debug.Log(name + ": ☠️ Activando animación de muerte");

        Invoke("DisappearCharacter", disappearDelay);
        Invoke("GameOver", gameOverDelay);
    }

    void DisappearCharacter()
    {
        Debug.Log(name + ": 👻 Desapareciendo personaje");

        Transform rootObject = transform;
        while (rootObject.parent != null)
        {
            rootObject = rootObject.parent;
        }

        rootObject.gameObject.SetActive(false);
    }

    void GameOver()
    {
        Debug.Log("🎮 GAME OVER - Reiniciando escena en 2 segundos...");
        Invoke("RestartGame", 2f);
    }

    void RestartGame()
    {
        Debug.Log("🔄 Reiniciando escena: " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
