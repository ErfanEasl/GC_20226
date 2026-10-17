using UnityEngine;

public class JumpMeter : MonoBehaviour
{
    [SerializeField] private Movment player;
    [SerializeField] private float playerHeight = 4f; // your player's height in units

    private Vector2 start;
    private float maxY;
    private bool inAir;

    void Update()
    {
        bool grounded = player.IsGrounded();

        if (!inAir && !grounded)
        {
            inAir = true;
            start = transform.position;
            maxY = start.y;
        }

        if (inAir)
        {
            maxY = Mathf.Max(maxY, transform.position.y);

            if (grounded)
            {
                inAir = false;
                float h = maxY - start.y;
                float d = Mathf.Abs(transform.position.x - start.x);
                Debug.Log("Jump height " + h.ToString("F2") + " units (" + (h / playerHeight).ToString("F2") + " player heights) | distance " + d.ToString("F2") + " units (" + (d / playerHeight).ToString("F2") + " player heights)");
            }
        }
    }
}