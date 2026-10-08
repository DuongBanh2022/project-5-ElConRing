using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform hpFill;
    [SerializeField] private float shrinkSpeed = 5f;

    private float targetScale = 1f;

    private void Start()
    {
        if (hpFill != null)
        {
            hpFill.localScale = new Vector3(
                1f,
                1f,
                1f
            );
        }

        targetScale = 1f;
    }

    private void Update()
    {
        if (hpFill == null)
            return;

        float currentScale = hpFill.localScale.x;

        float newScale = Mathf.Lerp(
            currentScale,
            targetScale,
            shrinkSpeed * Time.deltaTime
        );

        hpFill.localScale = new Vector3(
            newScale,
            1f,
            1f
        );
    }

    public void UpdateHealth(int currentHP, int maxHP)
    {
        if (hpFill == null)
            return;

        float healthPercent =
            (float)currentHP / maxHP;

        targetScale = Mathf.Clamp01(
            healthPercent
        );

        Debug.Log(
            "HP BAR: " +
            currentHP +
            "/" +
            maxHP +
            " = " +
            targetScale
        );
    }
}

