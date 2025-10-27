using UnityEngine;
using System;

public class RopeRotation : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public Transform rope;
    public float verticalAmplitude = 0.3f;
    public float verticalSpeed = 2f;

    private float baseHeight;
    private bool goingDown = true;

    // 🔔 Evento que avisa cuando la cuerda pasa por el suelo
    public static event Action OnRopeSwing;

    void Start()
    {
        if (rope != null)
            baseHeight = rope.localPosition.y;
    }

    void Update()
    {
        // 🔁 Rotación continua
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        if (rope != null)
        {
            // Movimiento vertical tipo seno
            float newY = baseHeight + Mathf.Sin(Time.time * verticalSpeed) * verticalAmplitude;
            rope.localPosition = new Vector3(rope.localPosition.x, newY, rope.localPosition.z);

            // 📉 Detectar cuando pasa por el punto más bajo
            float sinValue = Mathf.Sin(Time.time * verticalSpeed);

            if (goingDown && sinValue < -0.99f)
            {
                goingDown = false;
                OnRopeSwing?.Invoke(); // ⚡ Avisar a todos que la cuerda pasó
                // Debug.Log("💥 Cuerda pasó por el suelo!");
            }
            else if (!goingDown && sinValue > 0f)
            {
                goingDown = true;
            }
        }
    }
}
