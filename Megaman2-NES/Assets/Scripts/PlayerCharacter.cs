using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCharacter : MonoBehaviour
{
    public enum State
    {
        None,
        Idle,
        Walk,
        Jump,
        Hit,
        Die
    }

    private State currentState_ = State.None;

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private PlayerFoot foot;
    [SerializeField] private Bullet bulletPrefab;

    private const float DeadZone = 0.1f;
    private const float MoveSpeed = 2.0f;
    private const float JumpSpeed = 5.0f;

    private bool facingRight_ = true;
    private bool jumpButtonDown_ = false;
    private bool shootButtonDown_ = false;

    private Vector3 moveDir_ = new Vector2();

    private Transform transform_;
    private float angle_ = -90.0f;

    void Start()
    {
        ChangeState(State.Jump);
        transform_ = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpButtonDown_ = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            shootButtonDown_ = true;
        }
    }

    void FixedUpdate()
    {
        float moveDir = 0.0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir -= 1.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir += 1.0f;
        }

        if (foot.FootContact > 0 && jumpButtonDown_)
        {
            Jump();
        }
        jumpButtonDown_ = false;

       
        if (shootButtonDown_)
        {
            moveDir_ = Quaternion.AngleAxis(angle_, Vector3.forward) * Vector3.up;
            var bullet = Instantiate(bulletPrefab, transform_.position, Quaternion.identity);
            bullet.Direction = moveDir_;
            
        }
        shootButtonDown_ = false;

        var vel = body.velocity;
        body.velocity = new Vector2(MoveSpeed * moveDir, vel.y);
        //We flip the characters when not facing in the right direction
        if (moveDir > DeadZone && !facingRight_)
        {
            Flip();
            angle_ = -90.0f;
        }

        if (moveDir < -DeadZone && facingRight_)
        {
            Flip();
            angle_ = 90.0f;
        }
        //We manage the state machine of the character
        switch (currentState_)
        {
            case State.Idle:
                if (Mathf.Abs(moveDir) > DeadZone)
                {
                    ChangeState(State.Walk);
                }

                if (foot.FootContact == 0)
                {
                    ChangeState(State.Jump);
                }
                break;
            case State.Walk:
                if (Mathf.Abs(moveDir) < DeadZone)
                {
                    ChangeState(State.Idle);
                }

                if (foot.FootContact == 0)
                {
                    ChangeState(State.Jump);
                }
                break;
            case State.Jump:
                if (foot.FootContact > 0)
                {
                    ChangeState(State.Idle);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Jump()
    {
        var vel = body.velocity;
        body.velocity = new Vector2(vel.x, JumpSpeed);
    }

    void ChangeState(State state)
    {
        switch (state)
        {
            case State.Idle:
                anim.Play("Idle");
                break;
            case State.Walk:
                anim.Play("Walk");
                break;
            case State.Jump:
                anim.Play("Jump");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentState_ = state;
    }

    void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        facingRight_ = !facingRight_;
    }
}
