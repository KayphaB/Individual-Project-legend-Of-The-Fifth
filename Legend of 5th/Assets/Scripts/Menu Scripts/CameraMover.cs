using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    public float waitTime = 15f;                // Time to wait before each move
    public float moveSpeed = 2f;                // Speed of movement
    public Vector3 targetOffset = new Vector3(0, -5, 0); // How far down to move

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition + targetOffset;
        StartCoroutine(CameraLoop());
    }

    IEnumerator CameraLoop()
    {
        while (true)
        {
            // Wait before moving
            yield return new WaitForSeconds(waitTime);

            // Start moving down
            isMoving = true;
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Reset position instantly
            transform.position = originalPosition;
            isMoving = false;

            // Loop again
        }
    }
}
