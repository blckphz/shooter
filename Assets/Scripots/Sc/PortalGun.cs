using UnityEngine;

[CreateAssetMenu(fileName = "NewPortalGun", menuName = "Guns/PortalGun")]
public class PortalGun : GunSO
{
    public GameObject portalProjectilePrefab;
    public GameObject bluePortalPrefab;
    public GameObject orangePortalPrefab;

    public GameObject currentBluePortal;
    public GameObject currentOrangePortal;

    public override void ShootGun(Transform spawnPoint, float bulletSpeed)
    {
        // Not used directly; we call ShootBluePortal or ShootOrangePortal instead
    }

    public void ShootBluePortal(Transform spawnPoint, float bulletSpeed)
    {
        SpawnPortalBullet(spawnPoint, bulletSpeed, true);
    }

    public void ShootOrangePortal(Transform spawnPoint, float bulletSpeed)
    {
        SpawnPortalBullet(spawnPoint, bulletSpeed, false);
    }

    private void SpawnPortalBullet(Transform spawnPoint, float bulletSpeed, bool isBlue)
    {


        Vector3 spawnPosition = spawnPoint.position + spawnPoint.forward * 0.5f; // Offset forward
        GameObject portalBullet = Object.Instantiate(portalProjectilePrefab, spawnPosition, spawnPoint.rotation);

        Rigidbody rb = portalBullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = spawnPoint.forward * bulletSpeed;

        PortalBullet pb = portalBullet.GetComponent<PortalBullet>();
        if (pb != null)
            pb.Initialize(this, isBlue);

        Object.Destroy(portalBullet, 5f);
    }


    public void PlacePortal(Vector3 position, Quaternion rotation, bool isBlue)
    {
        position += rotation * Vector3.forward * 0.01f;

        if (isBlue)
        {
            if (currentBluePortal != null)
                Object.Destroy(currentBluePortal);

            currentBluePortal = Object.Instantiate(bluePortalPrefab, position, rotation);
        }
        else
        {
            if (currentOrangePortal != null)
                Object.Destroy(currentOrangePortal);

            currentOrangePortal = Object.Instantiate(orangePortalPrefab, position, rotation);
        }

        LinkPortals();
    }

    private void LinkPortals()
    {
        if (currentBluePortal == null || currentOrangePortal == null) return;
        PortalBrain.Instance.RegisterPortals(currentBluePortal.transform, currentOrangePortal.transform);
    }
}
