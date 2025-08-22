using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapes : MonoBehaviour
{
    //public int blendShapeCount;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Mesh skinnedMesh;
    public float blendOne = 0f;
    public float blendTwo = 0f;
    public float blendSpeed = 1f;
    public float stitchingSpeed = 2f;
    public bool blendOneFinished = false;
    public bool blendTwoFinished = false;
    public bool isStitching = false;
    
    [Header("Stapler Pin Integration")]
    public StaplerPinManager staplerPinManager;

    void Awake()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
    }

    void Start()
    {
        //blendShapeCount = skinnedMesh.blendShapeCount;
    }

    void Update()
    {
        
        /*if (blendShapeCount > 2)
        {
            if (blendOne < 100f)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, blendOne);
                blendOne += blendSpeed;
            }
            else
            {
                blendOneFinished = true;
            }

            if (blendOneFinished == true && blendTwo < 100f)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(1, blendTwo);
                blendTwo += blendSpeed;
            }
        }*/
    }


    public void Incision()
    {
        if (blendOne < 100f)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendOne);
            blendOne += blendSpeed;
        }
        else
        {
            blendOneFinished = true;
        }
    }

    public void Dissection()
    {
        if(blendTwo < 100f)
        {
            skinnedMeshRenderer.SetBlendShapeWeight(1, blendTwo);
            blendTwo += 99f;
        }
        else
        {
            blendTwoFinished = true;
        }
    }

    public void StartStitching()
    {
        // First close the dissection (blend shape 1), then close the incision (blend shape 0)
        if (blendTwoFinished && blendTwo > 0f)
        {
            // Close the dissection first
            blendTwo -= stitchingSpeed;
            blendTwo = Mathf.Max(blendTwo, 0f);
            skinnedMeshRenderer.SetBlendShapeWeight(1, blendTwo);
            
            if (blendTwo <= 0f)
            {
                blendTwoFinished = false;
            }
        }
        else if (blendOneFinished && blendOne > 0f)
        {
            // Then close the incision
            blendOne -= stitchingSpeed;
            blendOne = Mathf.Max(blendOne, 0f);
            skinnedMeshRenderer.SetBlendShapeWeight(0, blendOne);
            
            if (blendOne <= 0f)
            {
                blendOneFinished = false;
                isStitching = false;
            }
        }
    }

    public bool IsFullyHealed()
    {
        return blendOne <= 0f && blendTwo <= 0f && !blendOneFinished && !blendTwoFinished;
    }

    public bool CanStitch()
    {
        return blendOneFinished || blendTwoFinished;
    }
}
