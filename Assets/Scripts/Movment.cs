using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movment : MonoBehaviour
{
    [SerializeField] LayerMask ground;
    [SerializeField] Vector2 boxSize;
    [SerializeField] float Distance;
    public Rigidbody2D rb;
    public Animator animator;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPwer;
    float horizontal;

    bool faceRight;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Move
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveSpeed * horizontal, rb.velocity.y);

        //Jump
        animator.SetBool("isJumping", !IsGrounded());
        Jump();

        //Flip
        Flip();
    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPwer);
        }
    }
    private void Flip()
    {
        if (horizontal > 0 && faceRight)
        {
            transform.Rotate(0, 180, 0);
            faceRight = false;
        }
        else if (horizontal < 0 && !faceRight)
        {
            transform.Rotate(0, 180, 0);
            faceRight = true;
        }
    }
    public bool IsGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, Distance, ground))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * Distance, boxSize);
    }
}
