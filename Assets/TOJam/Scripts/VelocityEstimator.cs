using UnityEngine;

public class VelocityEstimator : MonoBehaviour
{
    public Vector3 Velocity { get; protected set; }
    public Vector3 AngularVelocity { get; protected set; }

    private Vector3 lastPosition;
    private Quaternion lastRotation;
        
    private void LateUpdate()
    {
        var currentPosition = transform.position;
        var currentRotation = transform.rotation;

        var factor = 1 / Time.deltaTime;

        //Assign Velocity
        Velocity = factor * (currentPosition - lastPosition);

        //Assign Angular Velocity
        var delta = currentRotation * Quaternion.Inverse(lastRotation);
        var theta = 2 * Mathf.Acos((Mathf.Clamp(delta.w, -1, 1)));
        if (theta > Mathf.PI)
        {
            theta -= 2 * Mathf.PI;
        }
            
        AngularVelocity = new Vector3(delta.x, delta.y, delta.z);

        if (AngularVelocity.sqrMagnitude > 0)
        {
            AngularVelocity = theta * factor * AngularVelocity.normalized;
        }
            
        //Assign last position and rotation
        lastPosition = currentPosition;
        lastRotation = currentRotation;
    }
}
