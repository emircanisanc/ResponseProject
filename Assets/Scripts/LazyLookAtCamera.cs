using UnityEngine;

public class LazyLookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform targetCamera; // The camera the object should look at
    [SerializeField] private float rotationSpeed = 2.0f; // Adjust for smoother/slower follow

    private void Start()
    {
        // Default to the main camera if not set
        if (targetCamera == null)
        {
            targetCamera = Camera.main.transform;
        }
    }

    private void Update()
    {
        // Calculate direction to the camera
        Vector3 directionToCamera = transform.position - targetCamera.position;
        directionToCamera.y = 0;

        // Get the target rotation towards the camera
        Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
