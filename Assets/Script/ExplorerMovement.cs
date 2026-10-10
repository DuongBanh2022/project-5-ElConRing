using UnityEngine;
using UnityEngine.InputSystem;

public class ExplorerMovement : MonoBehaviour
{
    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        transform.Translate(moveInput * Time.deltaTime * 5f);
    }
}
