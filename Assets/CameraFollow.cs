using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    
    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    
    public float moveDamping = 1f;

    // Place the script in the Camera-Control group in the component menu
    [AddComponentMenu("Camera-Control/Smooth Follow")]

    void LateUpdate () {
        // Early out if we don't have a target
        if (!target) return;
        
        float wantedHeight = target.position.y + height;

        Vector3 targetPos = target.position;
        Vector3 desiredPos = new Vector3(targetPos.x, wantedHeight, distance);
        transform.position = Vector3.Lerp(transform.position, desiredPos, moveDamping * Time.deltaTime);
        
        transform.LookAt(target);
    }
}
