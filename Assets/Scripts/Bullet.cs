using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D bulletRb;
    public float shootPower = 5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if (Input.GetKey(KeyCode.RightShift))
        {
            bulletRb.velocity = new Vector2(shootPower, bulletRb.velocity.y);
            transform.SetParent(null);
        }
        else if (shootPower == 0)
        {
            bulletRb.velocity = new Vector2(0, 0);
        }
    }
}
