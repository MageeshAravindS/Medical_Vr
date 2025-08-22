using UnityEngine;

public class StitcherFeedback : MonoBehaviour
{
    [Header("Visual Feedback")]
    public ParticleSystem stitchingEffect;
    public AudioSource stitchingSound;
    public Material stitchingMaterial;
    
    [Header("Haptic Feedback")]
    public float hapticIntensity = 0.3f;
    public float hapticDuration = 0.1f;
    
    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    private bool isStitching = false;
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalMaterial = meshRenderer.material;
        }
    }
    
    void Update()
    {
        // Check if we're currently stitching by looking for collision
        CheckStitchingStatus();
    }
    
    void CheckStitchingStatus()
    {
        // Simple collision detection to see if we're touching the forearm
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        bool touchingForearm = false;
        
        foreach (Collider col in colliders)
        {
            if (col.GetComponent<Collider1>() != null)
            {
                touchingForearm = true;
                break;
            }
        }
        
        if (touchingForearm && !isStitching)
        {
            StartStitchingFeedback();
        }
        else if (!touchingForearm && isStitching)
        {
            StopStitchingFeedback();
        }
    }
    
    void StartStitchingFeedback()
    {
        isStitching = true;
        
        // Visual feedback
        if (stitchingEffect != null)
        {
            stitchingEffect.Play();
        }
        
        if (meshRenderer != null && stitchingMaterial != null)
        {
            meshRenderer.material = stitchingMaterial;
        }
        
        // Audio feedback
        if (stitchingSound != null)
        {
            stitchingSound.Play();
        }
        
        // Haptic feedback for VR controllers
        TriggerHapticFeedback();
    }
    
    void StopStitchingFeedback()
    {
        isStitching = false;
        
        // Stop visual effects
        if (stitchingEffect != null)
        {
            stitchingEffect.Stop();
        }
        
        if (meshRenderer != null && originalMaterial != null)
        {
            meshRenderer.material = originalMaterial;
        }
        
        // Stop audio
        if (stitchingSound != null)
        {
            stitchingSound.Stop();
        }
    }
    
    void TriggerHapticFeedback()
    {
        // Basic haptic feedback for XR controllers
        // You might need to implement more specific haptics based on your XR system
        var xrGrabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (xrGrabInteractable != null && xrGrabInteractable.isSelected)
        {
            // Trigger haptics on the controller that's holding this tool
            // Implementation depends on your specific XR system
        }
    }
}