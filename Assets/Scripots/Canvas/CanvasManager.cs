using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public GunBehaviour gunReload;               // Reference to GunReload (added)
    public TextMeshProUGUI ammoText;          // Assign in Inspector
    public TextMeshProUGUI playerHealthText;  // Assign in Inspector

    void Update()
    {

        if (gunReload == null)
        {

            gunReload = GameObject.Find("Gun").GetComponent<GunBehaviour>();

        }

        if (gunReload != null && gunReload.currentGunso != null)
        {
            ammoText.text = $"{gunReload.currentGunso.currentClipSize} / {gunReload.currentGunso.maxClipSize}";
        }

        // Display static health directly
        playerHealthText.text = PlayerHealth.Health.ToString();
    }

    public bool gunsoIsReloading()
    {
        // Check GunReload's IsReloading now
        return gunReload != null && gunReload.currentGunso.IsReloading;
    }
}
