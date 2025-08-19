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
    public bool blendOneFinished = false;
    public bool blendTwoFinished = false;

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
}
