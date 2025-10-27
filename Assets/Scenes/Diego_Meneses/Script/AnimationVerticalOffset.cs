using UnityEngine;

public class AnimationVerticalOffset : MonoBehaviour
{
    [Header("Configuración")]
    public Animator animator;
    public Transform modelTransform;
    
    [Header("Ajuste de Idle")]
    public float idleOffsetY = 0.1f;
    public float transitionSpeed = 10f;
    public bool enableCorrection = true;
    
    [Header("Nombres de estados")]
    public string idleStateName = "A_Idle_Characters";
    public string jumpStateName = "A_Jump";
    public string dieStateName = "A_Die";
    
    private Vector3 baseLocalPosition;
    private float targetOffsetY = 0f;
    private float currentOffsetY = 0f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        
        if (modelTransform == null)
        {
            modelTransform = transform.parent;
            Debug.Log(name + ": Auto-asignado modelTransform: " + modelTransform.name);
        }

        if (modelTransform != null)
        {
            baseLocalPosition = modelTransform.localPosition;
            Debug.Log(name + ": Posición base guardada: " + baseLocalPosition);
        }
    }

    void LateUpdate()
    {
        if (!enableCorrection || animator == null || modelTransform == null)
            return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (stateInfo.IsName(idleStateName))
        {
            targetOffsetY = idleOffsetY;
        }
        else
        {
            targetOffsetY = 0f;
        }
        
        currentOffsetY = Mathf.Lerp(currentOffsetY, targetOffsetY, Time.deltaTime * transitionSpeed);
        
        Vector3 newPosition = baseLocalPosition;
        newPosition.y += currentOffsetY;
        modelTransform.localPosition = newPosition;
    }
}
