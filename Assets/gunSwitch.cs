using UnityEngine;

public class gunSwitch : MonoBehaviour
{
    public GunBehaviour gunBehaviour;

    void Update()
    {
        if (gunBehaviour == null || gunBehaviour.guns.Length == 0) return;

        // Switch forward (E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            gunBehaviour.currentGunIndex = (gunBehaviour.currentGunIndex + 1) % gunBehaviour.guns.Length;
            gunBehaviour.SwitchGun(gunBehaviour.guns[gunBehaviour.currentGunIndex]);
        }

        // Switch backward (Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gunBehaviour.currentGunIndex--;
            if (gunBehaviour.currentGunIndex < 0)
                gunBehaviour.currentGunIndex = gunBehaviour.guns.Length - 1;
            gunBehaviour.SwitchGun(gunBehaviour.guns[gunBehaviour.currentGunIndex]);
        }
    }
}
