using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D bulletLight;
    public PlayerHealth health;
    public PlayerUi ui;
    public EnemyHealth enemyHealth;


    public Transform player;
    public Rigidbody2D bulletRb;


    public float maxStamina = 10f;
    public float currentStamina;
    public float staminDrain = 1f;
    public float staminCharger = 1f;


    public float shootPower = 5f;
    public float playerSpeed = 5;
    float returnSpeedBost = 2;
    public float slowDown = 5f;

    [SerializeField] bool isShot = false;
    [SerializeField] bool isStopped = false;
    [SerializeField] bool isReturning = false;
    [SerializeField] bool isRecharging = false;
    [SerializeField] bool LightFader = false;

    private Vector3 lightPlace;

    // Used by the checkpoint to know if the light has been thrown
    public bool IsShot => isShot;

    // Mouse click, E, or Right Shift
    bool PressDown => Input.GetMouseButtonDown(0)
                   || Input.GetKeyDown(KeyCode.E)
                   || Input.GetKeyDown(KeyCode.RightShift);

    bool Released => Input.GetMouseButtonUp(0)
                  || Input.GetKeyUp(KeyCode.E)
                  || Input.GetKeyUp(KeyCode.RightShift);

    void Start()
    {
        lightPlace = transform.localPosition;
        currentStamina = maxStamina;
        if (ui != null)
        {
            ui.UpdateStamina(1f);
        }
    }

    void Update()
    {
        if (bulletLight != null)
        {
            bulletLight.intensity = currentStamina / 5;
            bulletLight.pointLightInnerRadius = currentStamina;
        }


        if (PressDown && !isShot && !isRecharging && currentStamina >= maxStamina && (ui != null || ui.hasLight))
        {
            transform.SetParent(null);

            isShot = true;
            isReturning = false;
            isStopped = false;
            LightFader = false;

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
        else if (PressDown && isStopped && !isReturning)
        {
            isReturning = true;
        }

        if (isReturning && Released && currentStamina > 0)
        {
            isReturning = false;
            isStopped = true;
            bulletRb.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isRecharging)
        {
            currentStamina = Mathf.MoveTowards(currentStamina, maxStamina, staminCharger * Time.fixedDeltaTime);
            if (currentStamina >= maxStamina)
            {
                isRecharging = false;
                if (bulletLight != null)
                {
                    bulletLight.enabled = true;
                    if (ui != null && ui.hasLight)
                    {
                        ui.UpdateStamina(1f);
                    }
                }
            }
        }

        if (!isShot)
        {
            return;
        }

        if (isReturning)
        {
            Vector2 direction = ((Vector2)player.position - bulletRb.position).normalized;
            float returnSpeed = playerSpeed * returnSpeedBost;
            bulletRb.velocity = direction * returnSpeed;

            if (Vector2.Distance(bulletRb.position, player.position) < 0.5f)
            {
                bulletRb.velocity = Vector2.zero;

                transform.SetParent(player);
                transform.localPosition = lightPlace;

                isShot = false;
                isStopped = false;
                isReturning = false;

                if (currentStamina <= 0)
                {
                    isRecharging = true;
                }
                else
                {
                    currentStamina = maxStamina;
                    if (ui != null && ui.hasLight)
                    {
                        ui.UpdateStamina(1f);

                        if (bulletLight != null)
                        {
                            bulletLight.enabled = true;
                        }
                    }
                    return;
                }

                isShot = false;
                isStopped = false;
                isReturning = false;
            }
        }
        if (!isStopped)
        {
            bulletRb.velocity = Vector2.MoveTowards(bulletRb.velocity, Vector2.zero, slowDown * Time.fixedDeltaTime);
            if (bulletRb.velocity.magnitude < 0.05f)
            {
                bulletRb.velocity = Vector2.zero;
                isStopped = true;
            }
            return;
        }


        if (isStopped)
        {
            currentStamina -= staminDrain * Time.fixedDeltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);

            if (ui != null)
            {
                float staminaPercent = currentStamina / maxStamina;
                ui.UpdateStamina(staminaPercent);
            }
        }

        if (currentStamina <= 0f && !LightFader)
        {
            LightFader = true;

            if (ui != null)
            {
                ui.LoseLight();
            }
            isReturning = true;
        }
        if (bulletLight != null && currentStamina <= 0)
        {
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