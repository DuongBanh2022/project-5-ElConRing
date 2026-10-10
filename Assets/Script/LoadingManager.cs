using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text loadingText;

    [Header("Loading Settings")]
    [SerializeField] private float minimumLoadingTime = 3f;
    [SerializeField] private float barAnimationSpeed = 2f;

    private void Start()
    {
        StartCoroutine(LoadGameplay());
        StartCoroutine(AnimateLoadingText());
    }
    private IEnumerator AnimateLoadingText()
    {
        string[] dots =
        {
        "LOADING.",
        "LOADING..",
        "LOADING..."
    };

        int index = 0;

        while (true)
        {
            if (loadingText != null)
            {
                loadingText.text = dots[index];
            }

            index++;

            if (index >= dots.Length)
            {
                index = 0;
            }

            yield return new WaitForSeconds(0.35f);
        }
    }
    private IEnumerator LoadGameplay()
    {
        float startTime = Time.time;

        float displayedProgress = 0f;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync("Gameplay");

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float actualProgress =
                Mathf.Clamp01(operation.progress / 0.9f);

            // Smoothly animate the bar
            displayedProgress = Mathf.MoveTowards(
                displayedProgress,
                actualProgress,
                barAnimationSpeed * Time.deltaTime
            );

            if (loadingBar != null)
            {
                loadingBar.value = displayedProgress;
            }

            if (loadingText != null)
            {
                loadingText.text =
                    "LOADING... " +
                    Mathf.RoundToInt(displayedProgress * 100f) +
                    "%";
            }

            if (operation.progress >= 0.9f)
            {
                float elapsedTime = Time.time - startTime;

                if (elapsedTime >= minimumLoadingTime)
                {
                    displayedProgress = 1f;

                    if (loadingBar != null)
                    {
                        loadingBar.value = 1f;
                    }

                    if (loadingText != null)
                    {
                        loadingText.text = "LOADING... 100%";
                    }

                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

}