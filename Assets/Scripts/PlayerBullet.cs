using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 10f;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null && mainCamera.WorldToViewportPoint(transform.position).y > 1f)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        if (GameManager.instance != null)
        {
            GameManager.instance.AddScore(100);

            // Check whether this is the last enemy.
            GameObject[] enemies =
                GameObject.FindGameObjectsWithTag("Enemy");

            if (enemies.Length <= 1)
            {
                GameManager.instance.WinGame();
            }
        }

        Destroy(other.gameObject);
        Destroy(gameObject);
    }

}
