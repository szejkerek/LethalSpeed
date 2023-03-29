using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 acceleration;

    [Header("Player properties")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float groundAcceleration;
    [SerializeField] private float groundFriction;
    [SerializeField] private float maxGroundSpeed;
    [SerializeField] private float airAcceleration;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityForce;

    private bool isGrounded;
    private bool isJumpQueued = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if(isGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.0f;
        }
        
        if(isGrounded)
        {
            GroundMove();
            ApplyFriction();
        }
        else
        {
            AirMove();
        }

        velocity.y += gravityForce * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void ApplyFriction()
    {
        if(velocity.magnitude == 0.0f)
        {
            return;
        }

        Vector3 frictionForce = new Vector3(-velocity.x, 0.0f, -velocity.z) * groundFriction;

        velocity += frictionForce * Time.deltaTime;
    }

    private void GroundMove()
    {
        Vector3 wishDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            wishDir += this.transform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            wishDir += this.transform.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            wishDir -= this.transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            wishDir -= this.transform.right;
        }

        wishDir.Normalize();

        if(Input.GetButtonDown("Jump") || isJumpQueued)
        {
            if(isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2.0f * gravityForce);
                isJumpQueued = false;
            }
            else
            {
                isJumpQueued = true;
            }
        }

        Vector3 move = wishDir * groundAcceleration * Time.deltaTime;

        velocity += move;

        if(velocity.magnitude <= maxGroundSpeed)
        {
            velocity += move;
        }
        else
        {
            velocity.Normalize();
            velocity *= maxGroundSpeed;
        }
    }

    private void AirMove()
    {
        Vector3 wishDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            wishDir += this.transform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            wishDir += this.transform.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            wishDir -= this.transform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            wishDir -= this.transform.right;
        }

        wishDir.Normalize();

        Vector3 move = wishDir * airAcceleration * Time.deltaTime;

        velocity += (1.0f - Vector3.Dot(velocity.normalized, wishDir)) * move;

        if (velocity.magnitude <= maxGroundSpeed)
        {
            velocity += move;
        }
        else
        {
            velocity.Normalize();
            velocity *= maxGroundSpeed;
        }
    }
}
