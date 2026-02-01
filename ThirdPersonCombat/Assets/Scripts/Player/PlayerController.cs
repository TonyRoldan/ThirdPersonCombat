using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;

    [Header("Jump")]
    [SerializeField] float jumpHeight;
    [SerializeField] float gravity;

    [Header("Components")]
    [SerializeField] CharacterController characterController;
    [SerializeField] Transform camTransform;
    [SerializeField] bool faceMoveDirection;

    private Vector3 currMovement;
    private Vector3 velocity;

    void Update()
    {
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * currMovement.y + right * currMovement.x;
        characterController.Move(moveDirection * walkSpeed * Time.deltaTime);

        if(faceMoveDirection && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion toRot = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRot, 10f * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    public void HandleMovement(InputAction.CallbackContext context)
    {
        currMovement = context.ReadValue<Vector2>();
    }

    public void HandleJump(InputAction.CallbackContext context)
    {
        if(context.performed && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
