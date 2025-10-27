using UnityEngine;

public class AnimatorDebugger : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            Debug.Log("=== DIAGNÓSTICO DEL ANIMATOR ===");
            Debug.Log("GameObject: " + gameObject.name);
            Debug.Log("Controller: " + animator.runtimeAnimatorController.name);
            
            AnimatorControllerParameter[] parameters = animator.parameters;
            Debug.Log("\n--- PARÁMETROS (" + parameters.Length + ") ---");
            foreach (AnimatorControllerParameter param in parameters)
            {
                Debug.Log("  • " + param.name + " (" + param.type + ")");
            }

            Debug.Log("\n--- CLIPS DE ANIMACIÓN ---");
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                Debug.Log("  • " + clip.name + " (duración: " + clip.length + "s)");
            }

            Debug.Log("\n--- ESTADO ACTUAL ---");
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            Debug.Log("  Hash: " + currentState.fullPathHash);
            Debug.Log("  Normalized Time: " + currentState.normalizedTime);
            
            Debug.Log("\n=== FIN DIAGNÓSTICO ===\n");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("\n🧪 TEST: Activando trigger 'Jump' manualmente");
            animator.SetTrigger("Jump");
            
            Invoke(nameof(CheckAnimatorState), 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("\n🧪 TEST: Activando trigger 'Die' manualmente");
            animator.SetTrigger("Die");
            
            Invoke(nameof(CheckAnimatorState), 0.1f);
        }
    }

    void CheckAnimatorState()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Estado actual después del trigger:");
        Debug.Log("  Hash: " + state.fullPathHash);
        Debug.Log("  Normalized Time: " + state.normalizedTime);
        
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (state.IsName(clip.name))
            {
                Debug.Log("  ✅ Estado: " + clip.name);
                return;
            }
        }
        Debug.Log("  ⚠️ No se pudo identificar el estado actual");
    }
}
