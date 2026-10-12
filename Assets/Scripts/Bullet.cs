using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D bulletRb;
    public float shootPower = 5f;
    public float slowDown = 5f;


    [SerializeField] bool isShot = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightShift) && !isShot)
        {
            transform.SetParent(null);
            isShot = true;

            bulletRb.velocity = Vector2.right * shootPower;
        }
    }

    private void FixedUpdate()
    {
        if (isShot && bulletRb.velocity.magnitude > 0)
        {
            bulletRb.velocity = Vector2.MoveTowards(bulletRb.velocity, Vector2.zero, slowDown * Time.fixedDeltaTime);
        }
    }
}
