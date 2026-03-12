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

    public WorldRend world;

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
        // Vector3 playerPos = position;
        // Vector3 spherePos = new Vector3(5, 1, 2);

        // float collisionRadius = 2f; 
        // float dist = Vector3.Distance(playerPos, spherePos);
        

        // if(dist < collisionRadius)
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // }

        Vector3 spherePos = world.spherePos;

        float sphereRadius = 1.5f;
        float cubeRadius = 0.8f;

        float dist = Vector3.Distance(position, spherePos);

        if (dist < sphereRadius + cubeRadius)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}
