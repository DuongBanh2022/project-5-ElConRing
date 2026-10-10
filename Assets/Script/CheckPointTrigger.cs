using System.Collections;
using TMPro;
using UnityEngine;

public class TriggerCheckPoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private bool saveOnlyOnce = true;
    [SerializeField] private int checkpointNumber = 1;

    [Header("UI Message")]
    [SerializeField] private TMP_Text checkpointText;
    [SerializeField] private float displayDuration = 2f;

    private bool activated;
    private Coroutine hideCoroutine;

    private void Start()
    {
        if (checkpointText != null)
        {
            checkpointText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (saveOnlyOnce && activated)
            return;

        activated = true;

        Vector3 savePosition = respawnPoint != null
            ? respawnPoint.position
            : transform.position;

        PlayerPrefs.SetFloat("LastX", savePosition.x);
        PlayerPrefs.SetFloat("LastY", savePosition.y);
        PlayerPrefs.SetFloat("LastZ", savePosition.z);
        PlayerPrefs.SetInt("HasLastLocation", 1);
        PlayerPrefs.SetInt("A4Checkpoint", checkpointNumber);
        PlayerPrefs.Save();

        Debug.Log(
            "CHECKPOINT SAVED: " +
            checkpointNumber + " at " + savePosition
        );

        ShowCheckpointText();
    }

    private void ShowCheckpointText()
    {
        if (checkpointText == null)
        {
            Debug.LogWarning(
                "Checkpoint Text is not assigned!", this
            );
            return;
        }

        checkpointText.text = "Lost Grace Found!";
        checkpointText.gameObject.SetActive(true);

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideTextAfterDelay());
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSecondsRealtime(displayDuration);

        if (checkpointText != null)
        {
            checkpointText.gameObject.SetActive(false);
        }

        hideCoroutine = null;
    }
}
