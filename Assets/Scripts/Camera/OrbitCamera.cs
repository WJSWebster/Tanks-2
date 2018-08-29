using UnityEngine;

public class OrbitCamera : MonoBehaviour {

    public bool isOrbiting = true;    // whether or not the camera will currently orbit
    public Vector3 target = new Vector3(0f, 0f, 0f);  // the target the camera will orbit around
    public float rotationSpeed = 5f;  // the speed at which the camera will orbit

    // Keeps the camera looking at a specific point, while orbiting around that point
    void Update()
    {
        if (isOrbiting)
        {
            transform.RotateAround(target, Vector3.up, Time.deltaTime * rotationSpeed);

            transform.LookAt(target);
        }
    }
}
