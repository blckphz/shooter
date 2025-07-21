using UnityEngine;

public class gunSwitch : MonoBehaviour
{
    public GunBehaviour gunBehaviour;
    public AudioClip switchSound; // Sound effect when switching weapons
    public AudioSource audioSource;

    void Start()
    {
        if (gunBehaviour == null)
        {
            gunBehaviour = GetComponent<GunBehaviour>();
        }

        // Add or find AudioSource
    }

    void Update()
    {
        if (gunBehaviour == null || gunBehaviour.guns.Length == 0) return;

        bool isHooked = IsPlayerHooked();

        if (isHooked)
        {
            // Debug.Log("[gunSwitch] Player is hooked — weapon switching disabled.");
            return;  // Block switching while hooked
        }

        // Switch using E (next weapon)
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextWeapon();
        }

        // Switch using Q (previous weapon)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PreviousWeapon();
        }

        // Switch with mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            NextWeapon();
        }
        else if (scroll < 0f)
        {
            PreviousWeapon();
        }
    }

    void NextWeapon()
    {
        gunBehaviour.currentGunIndex = (gunBehaviour.currentGunIndex + 1) % gunBehaviour.guns.Length;
        gunBehaviour.SwitchGun(gunBehaviour.guns[gunBehaviour.currentGunIndex]);
        PlaySwitchSound();
    }

    void PreviousWeapon()
    {
        gunBehaviour.currentGunIndex--;
        if (gunBehaviour.currentGunIndex < 0)
            gunBehaviour.currentGunIndex = gunBehaviour.guns.Length - 1;
        gunBehaviour.SwitchGun(gunBehaviour.guns[gunBehaviour.currentGunIndex]);
        PlaySwitchSound();
    }

    void PlaySwitchSound()
    {
        if (audioSource != null && switchSound != null)
        {
            audioSource.PlayOneShot(switchSound);
        }
    }

    bool IsPlayerHooked()
    {
        grapplingHook[] hooks = FindObjectsOfType<grapplingHook>();

        foreach (var hook in hooks)
        {
            if (hook.player != null && IsSameOrChild(hook.player, transform))
            {
                if (hook.IsAttached)
                    return true;
            }
        }

        return false;
    }

    // Helper: checks if 'child' transform is the same or descendant of 'parent'
    bool IsSameOrChild(Transform parent, Transform child)
    {
        if (parent == child) return true;
        while (child.parent != null)
        {
            if (child.parent == parent) return true;
            child = child.parent;
        }
        return false;
    }
}
