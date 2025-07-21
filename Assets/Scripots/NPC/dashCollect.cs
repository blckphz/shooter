using UnityEngine;

public class DashCollect : MonoBehaviour
{
    public Rigidbody rb;
    public float hoverSpeed = 2f;       // Speed of hovering
    public float hoverHeight = 0.5f;    // How high it moves up/down
    public float boostBonus = 5f;
    private Vector3 startPos;

    void Start()
    {
        startPos = rb.position; // Save the initial position
    }

    void Update()
    {

        //up down
        float newY = startPos.y + Mathf.PingPong(Time.time * hoverSpeed, hoverHeight);
        rb.MovePosition(new Vector3(startPos.x, newY, startPos.z));


        //rotate
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, 180 * Time.deltaTime, 0));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // Add upward velocity boost
                Vector3 velocity = playerRb.linearVelocity;
                velocity.y = boostBonus;  // directly set upward velocity
                playerRb.linearVelocity = velocity;

                // Alternatively, you could use AddForce:
                // playerRb.AddForce(Vector3.up * boostBonus, ForceMode.VelocityChange);
            }


            Destroy(gameObject);
            // Add your pickup logic here
        }
    }
}
