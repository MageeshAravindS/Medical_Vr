using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider1 : MonoBehaviour
{
   public BlendShapes blendShapes;

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "blade")
        {
            if (blendShapes.blendOneFinished == false)
            {
                blendShapes.Incision();
            }
        }

        else if(other.gameObject.name == "spreader")
        {
            if (blendShapes.blendOneFinished == true && blendShapes.blendTwoFinished == false)
            {
                blendShapes.Dissection();
            }
        }

        else if(other.gameObject.name == "stitcher")
        {
            if (blendShapes.CanStitch())
            {
                blendShapes.isStitching = true;
                blendShapes.StartStitching();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "stitcher")
        {
            blendShapes.isStitching = false;
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (blendShapes.blendOneFinished == false)
        {
            blendShapes.Incision();
        }
    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
}
