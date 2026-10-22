using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5;
    public int damage = 1;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null && mainCamera.WorldToViewportPoint(transform.position).y < 0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player != null)
        {
            if (!player.IsShieldActive())
            {
                player.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }






}
