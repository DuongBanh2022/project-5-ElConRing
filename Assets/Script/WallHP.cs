using UnityEngine;

public class WallHP : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 3;

    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;

        Debug.Log(
            "Breakable object HP: " +
            currentHP +
            "/" +
            maxHP
        );
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log(
            "Breakable object HP: " +
            currentHP +
            "/" +
            maxHP
        );

        if (currentHP <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        Debug.Log("BREAKABLE OBJECT DESTROYED!");

        gameObject.SetActive(false);
    }
}

