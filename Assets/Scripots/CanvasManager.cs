using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public GunBehaviour gun;                  // Reference to GunBehaviour
    public TextMeshProUGUI ammoText;          // Assign in Inspector
    public TextMeshProUGUI playerHealthText;  // Assign in Inspector

    void Update()
    {
        if (gun != null && gun.currentGunso != null)
        {
            ammoText.text = $"{gun.currentGunso.currentClipSize} / {gun.currentGunso.maxClipSize}";
        }

        // Display static health directly
        playerHealthText.text = PlayerHealth.Health.ToString();
    }

    public bool gunsoIsReloading()
    {
        return gun != null && gun.IsReloading;
    }
}
