using UnityEngine;

public class RopeFireEffect : MonoBehaviour
{
    [Header("Configuración de Partículas")]
    public Sprite fireSprite;
    public Material particleMaterial;
    
    [Header("Modo de Emisión")]
    public EmissionMode emissionMode = EmissionMode.AroundCylinder;
    
    [Header("Apariencia del Fuego")]
    public int particlesPerUnit = 20;
    public float fireSize = 0.5f;
    public Color fireColorStart = new Color(1f, 0.5f, 0f, 1f);
    public Color fireColorEnd = new Color(1f, 0f, 0f, 0.5f);
    
    [Header("Animación")]
    public float particleLifetime = 0.5f;
    public float emissionRate = 50f;
    public Vector3 fireDirection = Vector3.up;
    public float fireSpeed = 1f;
    public bool radialDirection = false;
    
    [Header("Configuración Visual")]
    public bool hideOriginalMesh = true;
    
    private ParticleSystem fireParticles;
    private MeshRenderer meshRenderer;
    
    public enum EmissionMode
    {
        FromSurface,
        AroundCylinder,
        EdgeOnly
    }

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        
        if (hideOriginalMesh && meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        
        CreateFireParticleSystem();
        ConfigureParticleSystem();
    }

    void CreateFireParticleSystem()
    {
        GameObject fireObj = new GameObject("FireParticles");
        fireObj.transform.SetParent(transform);
        fireObj.transform.localPosition = Vector3.zero;
        fireObj.transform.localRotation = Quaternion.identity;
        
        fireParticles = fireObj.AddComponent<ParticleSystem>();
    }

    void ConfigureParticleSystem()
    {
        if (fireParticles == null) return;
        
        var main = fireParticles.main;
        main.startLifetime = particleLifetime;
        main.startSpeed = fireSpeed;
        main.startSize = fireSize;
        main.startColor = new ParticleSystem.MinMaxGradient(fireColorStart, fireColorEnd);
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.loop = true;
        main.playOnAwake = true;
        main.maxParticles = 1000;
        
        var emission = fireParticles.emission;
        emission.rateOverTime = emissionRate;
        
        ConfigureShape();
        
        if (radialDirection)
        {
            var shape = fireParticles.shape;
            main.startSpeed = new ParticleSystem.MinMaxCurve(fireSpeed);
        }
        else
        {
            var velocity = fireParticles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            velocity.x = new ParticleSystem.MinMaxCurve(fireDirection.x * fireSpeed);
            velocity.y = new ParticleSystem.MinMaxCurve(fireDirection.y * fireSpeed);
            velocity.z = new ParticleSystem.MinMaxCurve(fireDirection.z * fireSpeed);
        }
        
        var colorOverLifetime = fireParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] 
            { 
                new GradientColorKey(fireColorStart, 0.0f),
                new GradientColorKey(new Color(1f, 0.8f, 0f), 0.5f),
                new GradientColorKey(fireColorEnd, 1.0f)
            },
            new GradientAlphaKey[] 
            { 
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(0.8f, 0.5f),
                new GradientAlphaKey(0.0f, 1.0f)
            }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        
        var sizeOverLifetime = fireParticles.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.3f);
        curve.AddKey(0.5f, 1.0f);
        curve.AddKey(1.0f, 0.0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);
        
        var renderer = fireParticles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.alignment = ParticleSystemRenderSpace.View;
        
        if (particleMaterial != null)
        {
            renderer.material = particleMaterial;
        }
        else
        {
            Material defaultMaterial = new Material(Shader.Find("Particles/Standard Unlit"));
            defaultMaterial.SetColor("_Color", fireColorStart);
            defaultMaterial.SetInt("_BlendMode", 1);
            defaultMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            defaultMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.One);
            defaultMaterial.SetInt("_ZWrite", 0);
            defaultMaterial.DisableKeyword("_ALPHATEST_ON");
            defaultMaterial.EnableKeyword("_ALPHABLEND_ON");
            defaultMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            defaultMaterial.renderQueue = 3000;
            renderer.material = defaultMaterial;
        }
        
        if (fireSprite != null)
        {
            Texture2D spriteTexture = fireSprite.texture;
            renderer.material.SetTexture("_MainTex", spriteTexture);
        }
        
        renderer.sortingOrder = 10;
    }
    
    void ConfigureShape()
    {
        var shape = fireParticles.shape;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        
        switch (emissionMode)
        {
            case EmissionMode.FromSurface:
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    shape.shapeType = ParticleSystemShapeType.MeshRenderer;
                    shape.meshRenderer = GetComponent<MeshRenderer>();
                    shape.meshShapeType = ParticleSystemMeshShapeType.Triangle;
                }
                break;
                
            case EmissionMode.AroundCylinder:
                if (meshFilter != null && meshFilter.sharedMesh != null)
                {
                    shape.shapeType = ParticleSystemShapeType.MeshRenderer;
                    shape.meshRenderer = GetComponent<MeshRenderer>();
                    shape.meshShapeType = ParticleSystemMeshShapeType.Edge;
                }
                break;
                
            case EmissionMode.EdgeOnly:
                CapsuleCollider capsule = GetComponent<CapsuleCollider>();
                if (capsule != null)
                {
                    shape.shapeType = ParticleSystemShapeType.Circle;
                    shape.radius = capsule.radius;
                    shape.radiusThickness = 1f;
                    shape.rotation = new Vector3(0, 0, 90);
                }
                else if (meshFilter != null)
                {
                    Bounds bounds = meshFilter.sharedMesh.bounds;
                    float radius = Mathf.Max(bounds.extents.x, bounds.extents.z);
                    shape.shapeType = ParticleSystemShapeType.Circle;
                    shape.radius = radius * transform.lossyScale.x;
                    shape.radiusThickness = 1f;
                    shape.rotation = new Vector3(0, 0, 90);
                }
                break;
        }
    }
}
