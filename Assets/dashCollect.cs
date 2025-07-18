using UnityEngine;

public class DashCollect : MonoBehaviour
{
    public Rigidbody rb;
    public float hoverSpeed = 2f;       // Speed of hovering
    public float hoverHeight = 0.5f;    // How high it moves up/down

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
            Destroy(collision.gameObject);
            // Add your pickup logic here
        }
    }
}
