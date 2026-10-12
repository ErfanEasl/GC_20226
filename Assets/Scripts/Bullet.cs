using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   public Rigidbody2D bulletRb;
    float shootPower = 5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            bulletRb.velocity = new Vector2(shootPower, bulletRb.velocity.y);
        }
    }
}
