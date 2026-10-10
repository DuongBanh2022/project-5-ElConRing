using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    [Header("Chest Settings")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    [Header("Reward")]
    [SerializeField] private GameObject apple;

    private SpriteRenderer spriteRenderer;
    private bool playerNearby = false;
    private bool isOpen = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (closedSprite != null)
        {
            spriteRenderer.sprite = closedSprite;
        }

        if (apple == null)
        {
            Debug.LogError("CHEST ERROR: Apple is NOT assigned!");
        }
        else
        {
            Debug.Log("CHEST: Apple assigned = " + apple.name);

            // Hide apple when game starts
            apple.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerNearby)
            return;

        if (isOpen)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            Debug.Log("PLAYER NEAR CHEST - PRESS E");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void OpenChest()
    {
        isOpen = true;

        Debug.Log("CHEST OPENED!");

        // Change chest sprite
        if (spriteRenderer != null &&
            openSprite != null)
        {
            spriteRenderer.sprite = openSprite;
        }

        // Show apple
        if (apple != null)
        {
            apple.SetActive(true);

            Debug.Log(
                "APPLE ACTIVATED: " +
                apple.name
            );
        }
        else
        {
            Debug.LogError(
                "APPLE ERROR: Apple reference is NULL!"
            );
        }
    }
}


