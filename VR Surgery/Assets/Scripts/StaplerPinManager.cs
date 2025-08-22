using UnityEngine;

public class StaplerPinManager : MonoBehaviour
{
    [Header("Stapler Pin Configuration")]
    public GameObject staplerPinsParent; // Reference to "Straple Sets"
    public BlendShapes blendShapes; // Reference to the BlendShapes script
    
    [Header("Progressive Pin Settings")]
    [Range(1, 20)]
    public int totalPins = 15; // Total number of stapler pins available
    
    [Range(0.1f, 5f)]
    public float pinsPerStitchProgress = 1f; // How many pins to activate per stitching progress
    
    [Header("Pin Activation")]
    public bool activatePinsFromCenter = true; // Start from center and work outward
    public AudioClip staplerPinSound; // Sound effect for each pin placement
    
    private Transform[] staplerPins;
    private AudioSource audioSource;
    private int currentActivePins = 0;
    private float lastStitchProgress = 100f; // Track stitching progress
    
    void Start()
    {
        SetupStaplerPins();
        SetupAudioSource();
    }
    
    void Update()
    {
        if (blendShapes != null && blendShapes.isStitching)
        {
            UpdateStaplerPins();
        }
    }
    
    void SetupStaplerPins()
    {
        if (staplerPinsParent == null)
        {
            // Try to find "Straple Sets" if not assigned
            GameObject found = GameObject.Find("Straple Sets");
            if (found != null)
            {
                staplerPinsParent = found;
            }
        }
        
        if (staplerPinsParent != null)
        {
            // Get all child transforms (stapler pins)
            int childCount = staplerPinsParent.transform.childCount;
            staplerPins = new Transform[childCount];
            
            for (int i = 0; i < childCount; i++)
            {
                staplerPins[i] = staplerPinsParent.transform.GetChild(i);
                // Initially disable all pins
                staplerPins[i].gameObject.SetActive(false);
            }
            
            totalPins = childCount;
            Debug.Log($"Found {totalPins} stapler pins");
        }
    }
    
    void SetupAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f;
    }
    
    void UpdateStaplerPins()
    {
        if (staplerPins == null || blendShapes == null) return;
        
        // Calculate current stitching progress (how much is healed)
        float currentStitchProgress = CalculateStitchingProgress();
        
        // Only update if progress has changed significantly
        if (Mathf.Abs(currentStitchProgress - lastStitchProgress) > 0.5f)
        {
            UpdatePinActivation(currentStitchProgress);
            lastStitchProgress = currentStitchProgress;
        }
    }
    
    float CalculateStitchingProgress()
    {
        // Calculate how much the wound has been stitched (0 = fully open, 100 = fully closed)
        float blendOneProgress = (100f - blendShapes.blendOne) / 100f; // How much incision is closed
        float blendTwoProgress = (100f - blendShapes.blendTwo) / 100f; // How much dissection is closed
        
        // Combined progress (prioritize closing dissection first, then incision)
        if (blendShapes.blendTwo > 0f)
        {
            return blendTwoProgress * 50f; // First 50% is closing dissection
        }
        else
        {
            return 50f + (blendOneProgress * 50f); // Last 50% is closing incision
        }
    }
    
    void UpdatePinActivation(float stitchProgress)
    {
        // Calculate how many pins should be active based on progress
        int targetActivePins = Mathf.RoundToInt((stitchProgress / 100f) * totalPins);
        targetActivePins = Mathf.Clamp(targetActivePins, 0, totalPins);
        
        // Activate new pins if we need more
        while (currentActivePins < targetActivePins)
        {
            ActivateNextPin();
        }
        
        // Deactivate pins if we need fewer (for reverse stitching/cutting)
        while (currentActivePins > targetActivePins)
        {
            DeactivateLastPin();
        }
    }
    
    void ActivateNextPin()
    {
        if (currentActivePins >= totalPins) return;
        
        int pinIndex = GetNextPinIndex();
        if (pinIndex >= 0 && pinIndex < staplerPins.Length)
        {
            staplerPins[pinIndex].gameObject.SetActive(true);
            currentActivePins++;
            
            // Play sound effect
            PlayStaplerSound();
            
            Debug.Log($"Activated stapler pin {pinIndex + 1}/{totalPins}");
        }
    }
    
    void DeactivateLastPin()
    {
        if (currentActivePins <= 0) return;
        
        int pinIndex = GetLastActivePinIndex();
        if (pinIndex >= 0 && pinIndex < staplerPins.Length)
        {
            staplerPins[pinIndex].gameObject.SetActive(false);
            currentActivePins--;
            
            Debug.Log($"Deactivated stapler pin {pinIndex + 1}/{totalPins}");
        }
    }
    
    int GetNextPinIndex()
    {
        if (activatePinsFromCenter)
        {
            // Activate from center outward (alternating sides)
            int centerIndex = totalPins / 2;
            
            if (currentActivePins == 0)
                return centerIndex;
            
            int offset = (currentActivePins + 1) / 2;
            bool goLeft = (currentActivePins % 2 == 1);
            
            if (goLeft)
                return centerIndex - offset;
            else
                return centerIndex + offset;
        }
        else
        {
            // Activate sequentially from first to last
            return currentActivePins;
        }
    }
    
    int GetLastActivePinIndex()
    {
        // Find the last active pin to deactivate
        for (int i = staplerPins.Length - 1; i >= 0; i--)
        {
            if (staplerPins[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }
        return -1;
    }
    
    void PlayStaplerSound()
    {
        if (audioSource != null && staplerPinSound != null)
        {
            audioSource.clip = staplerPinSound;
            audioSource.pitch = Random.Range(0.9f, 1.1f); // Slight pitch variation
            audioSource.Play();
        }
    }
    
    public void ResetAllPins()
    {
        // Reset all pins (useful for starting a new procedure)
        if (staplerPins != null)
        {
            foreach (Transform pin in staplerPins)
            {
                pin.gameObject.SetActive(false);
            }
        }
        currentActivePins = 0;
        lastStitchProgress = 100f;
    }
    
    public void ForceActivateAllPins()
    {
        // Activate all pins instantly (for testing)
        if (staplerPins != null)
        {
            foreach (Transform pin in staplerPins)
            {
                pin.gameObject.SetActive(true);
            }
            currentActivePins = totalPins;
        }
    }
}