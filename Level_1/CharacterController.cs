using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed = 1f;
    public float blockSpeed = 50f;
    public float minSpeed = 100f;
    public float maxSpeed = 300f;
    public float speedShield = 0f;
    public float speedRotation = 1f;
    public bool isGround = false;

    public float speedY = 0f;

    Quaternion saveRotation;

    private Animator animation;
    private Rigidbody rb;
    CharacterAnimation shield;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        shield = GetComponent<CharacterAnimation>();

        animation= GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        CharacterRun();
        StopMoveInShield();
        CharacterMoving();
    }


    public void CharacterMoving()
    {

        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        Vector3 CameraMoving = Camera.main.transform.forward;
        CameraMoving.y = 0f;
        CameraMoving = CameraMoving.normalized;

        Vector3 direction = CameraMoving * Vertical + Camera.main.transform.right * Horizontal;
        Vector3 movement = direction * speed * Time.fixedDeltaTime;

        Vector3 newVelocity = rb.velocity;

        newVelocity.x = movement.x;
        newVelocity.z = movement.z;
        newVelocity.y -= speedY * Time.fixedDeltaTime;

        rb.velocity = newVelocity;

        if(direction.magnitude > 0.1)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.fixedDeltaTime);
            saveRotation = targetRotation;
            animation.SetBool("walk", true);
        } else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, saveRotation, speedRotation * Time.fixedDeltaTime);
            animation.SetBool("walk", false);
        }
    }

    private void CharacterRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = maxSpeed;
            animation.SetBool("run", true);
        }else
        {
            speed = minSpeed;
            animation.SetBool("run", false);
        }
    }
    private void StopMoveInShield()
    {
        if(shield.isDefence)
        {
            speed = speedShield;
        }
    }

}
