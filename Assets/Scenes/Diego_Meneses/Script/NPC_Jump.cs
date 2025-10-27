using UnityEngine;

public class NPCJumpRopeController : MonoBehaviour
{
    public Animator animator;
    public RopeRotation rope; // referencia al script de la cuerda
    public float jumpDelay = 0.1f; // margen de tiempo para saltar correctamente
    public float failChance = 0.2f; // probabilidad aleatoria de fallar (20%)
    public float disappearDelay = 2f;

    private bool isDead = false;

    void Update()
    {
        if (isDead) return;

        // Detectar fase del movimiento de la cuerda
        float ropePhase = Mathf.Sin(Time.time * rope.verticalSpeed);

        // Si la cuerda está abajo (cerca de 0), es momento de saltar
        if (ropePhase < -0.9f && Random.value > failChance)
        {
            animator.SetTrigger("Jump");
        }
        else if (ropePhase < -0.9f && Random.value <= failChance)
        {
            StartCoroutine(Fail());
        }
    }

    private System.Collections.IEnumerator Fail()
    {
        isDead = true;
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(disappearDelay);
        Destroy(gameObject);
    }
}
