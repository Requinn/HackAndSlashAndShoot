using UnityEngine;
/// <summary>
/// Used only on objects that will be fixed to face the camera no matter the parent's rotation.
/// </summary>
public class FixedRotation : MonoBehaviour {
    Quaternion rotation;
    void Awake() {
        rotation = transform.rotation;
    }
    void LateUpdate() {
        transform.rotation = rotation;
    }
}
