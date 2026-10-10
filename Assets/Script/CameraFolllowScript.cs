using UnityEngine;

public class CameraFolllowScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float followspeed = 0.1f;
    [SerializeField] private Vector3 offset;

    void Update()
    {
        if (player == null)
        {
            return;
        }

        Vector3 targetPosition = player.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            followspeed
        );
    }
}