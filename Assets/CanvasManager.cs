using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public GunBehaviour gun;                  // Reference to GunBehaviour
    public TextMeshProUGUI ammoText;          // Assign in Inspector
    public TextMeshProUGUI playerHealth;  // Optional reload status
    public PlayerHealth playerHealthScript;


    void Update()
    {
        if (gun != null && gun.gunso != null)
        {
            // Update ammo display
            ammoText.text = $"{gun.gunso.currentClipSize} / {gun.gunso.maxClipSize}";

            // Show reload text if reloading
            playerHealth.text = PlayerHealth.Health.ToString();
        }
    }

    public bool gunsoIsReloading()
    {
        // Use GunBehaviour's reloading flag if public or check by coroutine
        // Assuming GunBehaviour exposes isReloading as public or via a property
        return gunsoReloadCheck();
    }

    public bool gunsoReloadCheck()
    {
        // You can modify GunBehaviour to make isReloading public or add a getter
        return (bool)typeof(GunBehaviour)
            .GetField("isReloading", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(gun);
    }
}
