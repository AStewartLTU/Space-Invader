using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject endGamePanel;

    private void Start()
    {
        pausePanel.SetActive(false);

    }

    private void Update()
    {
        if (endGamePanel.activeSelf)
            return;

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool isPaused = pausePanel.activeSelf;
            pausePanel.SetActive(!isPaused);
            Time.timeScale = isPaused ? 1f : 0f;

        }


    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

    }






}
