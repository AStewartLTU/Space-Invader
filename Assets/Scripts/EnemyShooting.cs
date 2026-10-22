
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 2f;

    private static float nextFormationShotTime;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetShotTimer()
    {
        nextFormationShotTime = 0f;
    }

    void Update()
    {
        // Stop shooting when no enemies remain.
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
            return;

        if (Time.time < nextFormationShotTime)
            return;

        Shoot(enemies);
        nextFormationShotTime = Time.time + Mathf.Max(0.1f, fireRate);
    }

    void Shoot(GameObject[] enemies)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Enemy bullet prefab is not assigned.", this);
            enabled = false;
            return;
        }

        if (enemies.Length == 0)
            return;

        // Enemy objects share one cooldown and take turns firing.
        Transform shooter = enemies[Random.Range(0, enemies.Length)].transform;
        Instantiate(bulletPrefab, shooter.position, Quaternion.identity);
    }
}
