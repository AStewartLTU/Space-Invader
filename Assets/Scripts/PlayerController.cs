
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 7f;
    public int health = 3;
    public TMP_Text healthText;
    public TMP_Text shieldStatusText;
    public GameObject shieldVisual;
    public float shieldDuration = 3f;
    public float shieldCooldown = 5f;

    private bool shieldActive = false;
    private float shieldTimer = 0f;
    private float cooldownTimer = 0f;
    private Camera mainCamera;
    private Collider2D playerCollider;


    void Awake()
    {
        mainCamera = Camera.main;
        playerCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        UpdateHealthText();
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }

    }

    void UpdateShieldStatus()
    {
        if (shieldStatusText == null)
            return;

        if (shieldActive)
            shieldStatusText.text = "SHIELD ACTIVE: " +
                Mathf.CeilToInt(shieldTimer) + "s";
        else if (cooldownTimer > 0f)
            shieldStatusText.text = "SHIELD: " +
                Mathf.CeilToInt(cooldownTimer) + "s";
        else
            shieldStatusText.text = "SHIELD: READY";
    }
    void Update()
    {
        UpdateShield();
        UpdateShieldStatus();
        if (Keyboard.current == null)
            return;

        float horizontal = 0f;

        if (Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.aKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.rightArrowKey.isPressed ||
            Keyboard.current.dKey.isPressed)
        {
            horizontal = 1f;
        }

        transform.Translate(
            Vector2.right * horizontal * speed * Time.deltaTime
        );

        if (mainCamera == null)
            mainCamera = Camera.main;

        float xPosition = transform.position.x;
        if (mainCamera != null)
        {
            float halfWidth = playerCollider != null
                ? playerCollider.bounds.extents.x
                : 0f;
            float leftEdge = mainCamera.ViewportToWorldPoint(Vector3.zero).x + halfWidth;
            float rightEdge = mainCamera.ViewportToWorldPoint(Vector3.right).x - halfWidth;
            xPosition = Mathf.Clamp(xPosition, leftEdge, rightEdge);
        }

        transform.position = new Vector3(
            xPosition,
            transform.position.y,
            transform.position.z
        );
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Max(health, 0);

        UpdateHealthText();

        Debug.Log("Player health: " + health);

        if (health <= 0)
        {
            Debug.Log("Game Over!");
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }

            Destroy(gameObject);
        }
    }

    void UpdateShield()
    {
        if (Keyboard.current == null)
            return;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame
            && !shieldActive && cooldownTimer <= 0f)
        {
            shieldActive = true;
            shieldTimer = shieldDuration;
            if (shieldVisual != null)
                shieldVisual.SetActive(true);
        }

        if (shieldActive)
        {
            shieldTimer -= Time.deltaTime;

            if (shieldTimer <= 0f)
            {
                shieldActive = false;
                cooldownTimer = shieldCooldown;
                if (shieldVisual != null)
                    shieldVisual.SetActive(false);
            }
        }
    }

    public bool IsShieldActive()
    {
        return shieldActive;
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + health;
        }
    }


}
