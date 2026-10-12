using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movment : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPwer;
    float horizontal;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontal));


        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveSpeed * horizontal, rb.velocity.y);
    }
}
