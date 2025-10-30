using UnityEngine;

public class RopeRotation : MonoBehaviour
{
    [Header("Componentes")]
    public Transform rope; // Malla o modelo de la cuerda

    [Header("Movimiento de la cuerda")]
    public float rotationSpeed = 150f;
    public float verticalAmplitude = 0.3f;
    public float verticalSpeed = 2f;
    public float startDelay = 1.5f;

    private float timer = 0f;
    private bool isActive = false;
    private bool canHit = false;

    void Start()
    {
        Invoke(nameof(ActivateRope), startDelay);
    }

    void ActivateRope()
    {
        isActive = true;
        canHit = true;
    }

    void Update()
    {
        if (!isActive || rope == null)
            return;

        timer += Time.deltaTime;

        rope.localPosition = new Vector3(0, Mathf.Sin(timer * verticalSpeed) * verticalAmplitude, 1.2f);
        
        transform.localRotation = Quaternion.Euler(298.99f + (timer * rotationSpeed), 270f, 90f);
    }

    public bool CanHit()
    {
        return canHit;
    }

    // ✅ Este método devuelve la posición Y del centro de la cuerda (para sincronización)
    public float GetCenter()
    {
        return rope.localPosition.y;
    }

    public float GetRotationAngle()
    {
        return (timer * rotationSpeed) % 360f;
    }

    public Vector3 GetRopeWorldPosition()
    {
        if (rope != null)
            return rope.position;
        return Vector3.zero;
    }

    public float GetRotationPeriod()
    {
        if (rotationSpeed == 0f)
            return 1f;
        return 360f / rotationSpeed;
    }

    // ✅ Este método controla el momento en que puede golpear
    public void DisableHit()
    {
        canHit = false;
        Invoke(nameof(ReenableHit), 0.2f);
    }

    void ReenableHit()
    {
        canHit = true;
    }
}
