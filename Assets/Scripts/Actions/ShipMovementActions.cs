using UnityEngine;

public class ShipMovementActions : MonoBehaviour
{
    [SerializeField] private int _movementSpeed;
    [SerializeField] private int _rotationSpeed;
    [SerializeField] private Rigidbody _shipRigidBody;
    [SerializeField] private GameObject _camera;

    private void Update()
    {
        UpdateShipMovement();
        UpdateCameraMovement();
    }

    private void UpdateShipMovement()
    {
        _shipRigidBody.linearVelocity = Vector3.zero;
        
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");

        var movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        gameObject.transform.Translate(movementDirection * Time.deltaTime * _movementSpeed, Space.World);

        if (movementDirection != Vector3.zero)
        {
            var toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdateCameraMovement()
    {
        _camera.transform.position = _shipRigidBody.gameObject.transform.position + new Vector3(0f, 27.430000608f, -34.9300003f);
    }
}
