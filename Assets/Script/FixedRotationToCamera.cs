using UnityEngine;
/// <summary>
/// Used only on objects that will be fixed to face the camera no matter the parent's rotation.
/// </summary>
public class FixedRotationToCamera : MonoBehaviour {
    Quaternion rotation;
    void Awake() {
        rotation = transform.rotation;
        rotation.x = Camera.main.transform.rotation.x;
    }
    void LateUpdate() {
        transform.rotation = rotation;
    }
}
