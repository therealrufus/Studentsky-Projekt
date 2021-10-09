using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsMaster : MonoBehaviour
{
    [Header("MASTER")]

    public Transform player;
    public PlayerMovement playerMovement;
    public Camera cam;
    public Transform handLeft;
    public Transform handRight;

    [SerializeField] float easingSpeed;

    public void RotateWithEase(Transform hand, Quaternion newRotation, bool local = false)
    {
        RotateWithEase(hand, newRotation, local ? hand.localRotation : hand.rotation, local);
    }

    public void RotateWithEase(Transform hand, Quaternion newRotation, Quaternion lastRotation, bool local = false)
    {
        if (local)
            hand.localRotation = Quaternion.Lerp(lastRotation, newRotation, easingSpeed * 100 * Time.deltaTime);
        else
            hand.rotation = Quaternion.Lerp(lastRotation, newRotation, easingSpeed * 100 * Time.deltaTime);
    }

    public void MoveWithEase(Transform hand, Vector3 newPosition, bool local = false)
    {
        MoveWithEase(hand, newPosition, local ? hand.localPosition : hand.position, local);
    }

    public void MoveWithEase(Transform hand, Vector3 newPosition, Vector3 lastPosition, bool local = false)
    {
        if (local)
            hand.localPosition = Vector3.Lerp(lastPosition, newPosition, easingSpeed * 100 * Time.deltaTime);
        else
            hand.position = Vector3.Lerp(lastPosition, newPosition, easingSpeed * 100 * Time.deltaTime);
    }
}
