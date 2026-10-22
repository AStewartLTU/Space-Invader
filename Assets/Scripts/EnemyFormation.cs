using UnityEngine;

public class EnemyFormation : MonoBehaviour
{
    public float speed = 2f;
    public float dropAmount = 0.5f;
    public float speedIncreasePerKill = 0.15f;
    public float maxSpeed = 5f;

    private int direction = 1;
    private Camera mainCamera;
    private Collider2D playerCollider;

    void Awake()
    {
        mainCamera = Camera.main;
        FindPlayerCollider();
    }

    void Update()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        if (playerCollider == null)
            FindPlayerCollider();

        // Use the camera's world-space edges so the formation works at any
        // resolution, aspect ratio, or scene position.
        float leftEdge = mainCamera.ViewportToWorldPoint(Vector3.zero).x;
        float rightEdge = mainCamera.ViewportToWorldPoint(Vector3.right).x;
        float formationLeft = float.PositiveInfinity;
        float formationRight = float.NegativeInfinity;

        foreach (Renderer enemyRenderer in GetComponentsInChildren<Renderer>())
        {
            if (enemyRenderer == null || !enemyRenderer.enabled)
                continue;

            formationLeft = Mathf.Min(formationLeft, enemyRenderer.bounds.min.x);
            formationRight = Mathf.Max(formationRight, enemyRenderer.bounds.max.x);
        }

        bool hitEdge = direction > 0
            ? formationRight >= rightEdge
            : formationLeft <= leftEdge;

        if (hitEdge)
        {
            direction *= -1;
            transform.Translate(Vector2.down * dropAmount);
        }

        if (HasReachedPlayer())
        {
            if (GameManager.instance != null)
                GameManager.instance.GameOver();
            else
                Debug.LogError("Enemy formation reached the player, but no GameManager is available.");

            // Freeze the formation immediately even if the scene is missing its manager.
            enabled = false;
        }
    }

    void FindPlayerCollider()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerCollider = player.GetComponent<Collider2D>();
    }

    bool HasReachedPlayer()
    {
        if (playerCollider == null)
            return false;

        foreach (Renderer enemyRenderer in GetComponentsInChildren<Renderer>())
        {
            if (enemyRenderer != null && enemyRenderer.enabled &&
                enemyRenderer.bounds.min.y <= playerCollider.bounds.max.y)
                return true;
        }

        return false;
    }
    public void IncreaseSpeed()
    {
        speed = Mathf.Min(
            speed + speedIncreasePerKill,
            maxSpeed
        );
    }
}
