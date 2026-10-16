using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D bulletLight;
    public PlayerHealth health;
    public EnemyHealth enemyHealth;
    public Transform player;
    public Rigidbody2D bulletRb;
    public float maxStamina = 10f;
    public float currentStamina;
    public float staminDrain = 1f;
    public float shootPower = 5f;
    public float playerSpeed = 5;
    float returnSpeedBost = 2;
    public float slowDown = 5f;


    [SerializeField] bool isShot = false;
    [SerializeField] bool isStopped = false;
    [SerializeField] bool isReturning = false;

    private Vector3 lightPlace;

    // Start is called before the first frame update
    void Start()
    {
        lightPlace = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        bulletLight.intensity = currentStamina/5;
        bulletLight.pointLightInnerRadius = currentStamina;

        if (Input.GetKeyDown(KeyCode.RightShift) && !isShot)
        {
            transform.SetParent(null);
            isShot = true;

            isReturning = false;
            isStopped = false;
            float yRocation = player.eulerAngles.y;
            if (yRocation < 90f || yRocation > 270f)
            {
                bulletRb.velocity = Vector2.right * shootPower;
            }
            else
            {
                bulletRb.velocity = Vector2.left * shootPower;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightShift) && isStopped && !isReturning)
        {
            isReturning = true;
        }
        if (isReturning && Input.GetKeyUp(KeyCode.RightShift))
        {
            isReturning = false;
            isStopped = true;
            bulletRb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!isShot)
        {
            return;
        }
        if (isReturning)
        {
            Vector2 direction = ((Vector2)player.position - bulletRb.position).normalized;
            float returnSpeed = playerSpeed * returnSpeedBost;
            bulletRb.velocity = direction * returnSpeed;
            if (Vector2.Distance(bulletRb.position, player.position) < 0.2f)
            {
                bulletRb.velocity = Vector2.zero;

                transform.SetParent(player);
                transform.localPosition = lightPlace;

                if (currentStamina <= 0)
                {
                    health.TakeDamage(1);
                }
                bulletLight.enabled = true;
                currentStamina = maxStamina;
                isShot = false;
                isStopped = false;
                isReturning = false;
            }
        }

        else if (!isStopped)
        {
            bulletRb.velocity = Vector2.MoveTowards(bulletRb.velocity, Vector2.zero, slowDown * Time.fixedDeltaTime);
            if (bulletRb.velocity.magnitude < 0.05f)
            {
                bulletRb.velocity = Vector2.zero;
                isStopped = true;
            }
        }
        if (isStopped)
        {
            currentStamina -= staminDrain * Time.fixedDeltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
        }
        if (currentStamina <= 0)
        {
            isReturning = true;
            bulletLight.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyHealth.TakeDamage(1);
        }
    }
}
