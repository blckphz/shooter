using UnityEngine;

public class dashcollect : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerMove player = collision.gameObject.GetComponent<PlayerMove>();
        if (player != null)
        {
            player.ResetJumps();
            Destroy(gameObject);
        }
    }

}
