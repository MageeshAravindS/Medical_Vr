using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    public Transform R_hand;
    public Transform L_hand;

    public Transform XR_Right;
    public Transform XR_Left;

    public Vector3 handRotationOffset;

    // Update is called once per frame
    void Update()
    {
        R_hand.position = Vector3.Lerp(R_hand.position, XR_Right.position, 0.5f);
        L_hand.position = Vector3.Lerp(L_hand.position, XR_Left.position, 0.5f);

        R_hand.rotation = Quaternion.Lerp(R_hand.rotation, XR_Right.rotation, 0.5f) * Quaternion.Euler(handRotationOffset);
        L_hand.rotation = Quaternion.Lerp(L_hand.rotation, XR_Left.rotation, 0.5f) * Quaternion.Euler(handRotationOffset);
    }
}
