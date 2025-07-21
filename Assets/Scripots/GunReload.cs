using UnityEngine;
using UnityEngine.UI;

public class GunReload : MonoBehaviour
{
    public GunBehaviour GunBehav;
    public Image reloadImage;
    private AudioSource audioSource;

    void Start()
    {
        // Ensure an AudioSource is available
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (reloadImage == null)
        {
            GameObject reloadObj = GameObject.Find("ReloadIcon");
            if (reloadObj != null)
                reloadImage = reloadObj.GetComponent<Image>();
        }

        // Update reload timers for all guns
        foreach (var gun in GunBehav.guns)
        {
            if (gun.IsReloading)
            {
                gun.reloadTimer += Time.deltaTime;
                if (gun.reloadTimer >= gun.reloadTime)
                {
                    gun.currentClipSize = gun.maxClipSize;
                    gun.IsReloading = false;
                    gun.reloadTimer = 0f;
                    Debug.Log($"{gun.name} reload complete!");

                    // Play reload complete sound
                    if (audioSource != null && gun.reloadCompleteSound != null)
                    {
                        audioSource.PlayOneShot(gun.reloadCompleteSound);
                    }
                }
            }
        }

        // Update UI for current gun
        if (GunBehav != null && GunBehav.currentGunso != null && reloadImage != null)
        {
            GunSO current = GunBehav.currentGunso;
            reloadImage.fillAmount = current.IsReloading
                ? Mathf.Clamp01(current.reloadTimer / current.reloadTime)
                : 0f;
        }
    }

    public void StartReload(GunSO gun)
    {
        if (gun == null || gun.currentClipSize == gun.maxClipSize) return;
        if (!gun.IsReloading)
        {
            gun.IsReloading = true;
            Debug.Log($"Started reloading {gun.name}...");
        }
    }
}
