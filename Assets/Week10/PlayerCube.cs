using UnityEngine;
using Vector3 = System.Numerics.Vector3;
using UnityEngine.SceneManagement;
using System.Numerics;

public class PlayerCube : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 6f;
    public float gravity = -9.8f;


    public Vector3 position = new Vector3(0, 2, 0);
    public float yVelocity;

    public bool grounded;

    public float platformY = 0;

    public Material cubeMaterial;

    void Update()
    {
        Move();
        Jump();
        ApplyGravity();
        CheckGround();
        CheckSphereCollision();
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        position.X += horizontal * moveSpeed * Time.deltaTime;
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            yVelocity = jumpForce;
            grounded = false;
        }
    }

    void ApplyGravity()
    {
        yVelocity += gravity  * Time.deltaTime;
        position.Y += yVelocity * Time.deltaTime; 
    }

    void CheckGround()
    {
        if (position.Y <= platformY + 0.5f)
        {
            position.Y = platformY + 0.5f;
            yVelocity = 0;
            grounded = true;

            cubeMaterial.color = Color.red;
        }
        else
        {
            cubeMaterial.color = Color.white;
        }
    }

    void CheckSphereCollision()
    {
        Vector3 spherePos = new Vector3(5, 1, 0);

        float distance = Vector3.Distance(position, spherePos);

        if (distance < 1.2f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
