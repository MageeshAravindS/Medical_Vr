using UnityEngine;

[System.Serializable]
public class SurgicalSystemSetup : MonoBehaviour
{
    [Header("Auto-Setup Components")]
    public bool autoSetupOnStart = true;
    
    [Header("Manual Setup")]
    [Space(10)]
    public bool setupSurgicalSystemButton;
    
    [Header("References")]
    public GameObject forearmObject;
    public GameObject staplerSetsObject;
    public GameObject stitcherTool;
    
    void Update()
    {
        // Manual setup trigger via Inspector button
        if (setupSurgicalSystemButton)
        {
            setupSurgicalSystemButton = false;
            SetupSurgicalSystem();
        }
    }
    
    void Start()
    {
        if (autoSetupOnStart)
        {
            SetupSurgicalSystem();
        }
    }
    
    [ContextMenu("Setup Surgical System")]
    public void SetupSurgicalSystem()
    {
        Debug.Log("Setting up surgical system...");
        
        // Find components if not assigned
        FindMissingComponents();
        
        // Setup BlendShapes and StaplerPinManager connection
        SetupStaplerPinIntegration();
        
        // Enable the Straple Sets object
        EnableStraplerSets();
        
        Debug.Log("Surgical system setup complete!");
    }
    
    void FindMissingComponents()
    {
        if (forearmObject == null)
        {
            forearmObject = GameObject.Find("Forearm");
        }
        
        if (staplerSetsObject == null)
        {
            staplerSetsObject = GameObject.Find("Straple Sets");
        }
        
        if (stitcherTool == null)
        {
            stitcherTool = GameObject.Find("stitcher");
        }
    }
    
    void SetupStaplerPinIntegration()
    {
        if (forearmObject == null || staplerSetsObject == null) return;
        
        // Find or add StaplerPinManager
        StaplerPinManager pinManager = FindObjectOfType<StaplerPinManager>();
        if (pinManager == null)
        {
            // Add StaplerPinManager to the stitcher tool if available
            if (stitcherTool != null)
            {
                pinManager = stitcherTool.AddComponent<StaplerPinManager>();
            }
            else
            {
                // Create a new GameObject for the manager
                GameObject managerObj = new GameObject("Stapler Pin Manager");
                pinManager = managerObj.AddComponent<StaplerPinManager>();
            }
        }
        
        // Find BlendShapes component
        BlendShapes blendShapes = FindObjectOfType<BlendShapes>();
        
        if (pinManager != null && blendShapes != null)
        {
            // Configure the connections
            pinManager.staplerPinsParent = staplerSetsObject;
            pinManager.blendShapes = blendShapes;
            blendShapes.staplerPinManager = pinManager;
            
            Debug.Log("Connected BlendShapes and StaplerPinManager");
        }
    }
    
    void EnableStraplerSets()
    {
        if (staplerSetsObject != null && !staplerSetsObject.activeInHierarchy)
        {
            staplerSetsObject.SetActive(true);
            Debug.Log("Enabled Straple Sets object");
        }
    }
    
    [ContextMenu("Reset All Pins")]
    public void ResetAllPins()
    {
        StaplerPinManager pinManager = FindObjectOfType<StaplerPinManager>();
        if (pinManager != null)
        {
            pinManager.ResetAllPins();
            Debug.Log("Reset all stapler pins");
        }
    }
    
    [ContextMenu("Test Activate All Pins")]
    public void TestActivateAllPins()
    {
        StaplerPinManager pinManager = FindObjectOfType<StaplerPinManager>();
        if (pinManager != null)
        {
            pinManager.ForceActivateAllPins();
            Debug.Log("Activated all stapler pins for testing");
        }
    }
}