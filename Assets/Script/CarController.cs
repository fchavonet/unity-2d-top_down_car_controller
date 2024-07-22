using UnityEngine;

public class CarController : MonoBehaviour
{
    // Public variables.
    [Space(10)]
    public Rigidbody2D carRigidbody2D;

    [Space(10)]
    [Header("Car Settings")]
    public float accelerationFactor = 30.0f;
    public float driftFactor = 0.95f;
    public float maxSpeed = 20;
    public float turnFactor = 3.5f;

    // Private variables.
    private float accelerationInput = 0;
    private float rotationAngle = 0;
    private float steeringInput = 0;
    private float velocityVSUp = 0;

    private void Awake()
    {
        // Get the Rigidbody2D component attached to the car.
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Get input values for steering and acceleration.
        steeringInput = Input.GetAxis("Horizontal");
        accelerationInput = Input.GetAxis("Vertical");

        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();
    }

    private void ApplyEngineForce()
    {
        // Calculate speed relative to the car's up direction.
        velocityVSUp = Vector2.Dot(transform.up, carRigidbody2D.velocity);

        // Check if the car has reached max speed and is accelerating.
        if (velocityVSUp > maxSpeed && accelerationInput > 0)
            return;

        // Check if the car is moving backward too fast and accelerating.
        if (velocityVSUp < -maxSpeed * 0.5f && accelerationInput > 0)
            return;

        // Check if the car has exceeded max speed.
        if (carRigidbody2D.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        // Adjust drag based on acceleration input.
        if (accelerationInput == 0)
            carRigidbody2D.drag = Mathf.Lerp(carRigidbody2D.drag, 3.0f, Time.fixedDeltaTime * 3);
        else
            carRigidbody2D.drag = 0;

        // Calculate the force to apply for acceleration.
        Vector2 engineForceVector = accelerationFactor * accelerationInput * transform.up;
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        // Determine the minimum speed before allowing steering.
        float minSpeedBeforeAllowTurningFactor = carRigidbody2D.velocity.magnitude / 8;
        _ = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        // Update the car's rotation angle based on steering input.
        rotationAngle -= steeringInput * turnFactor * minSpeedBeforeAllowTurningFactor;

        // Apply the new rotation to the car.
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    private void KillOrthogonalVelocity()
    {
        // Calculate forward and right velocities.
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);

        // Update the car's velocity to reduce sideways movement.
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * driftFactor;
    }
}
