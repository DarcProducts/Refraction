using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] UnityEvent OnEnter;
    [SerializeField] UnityEvent OnExit;
    public bool IsPaused { get; set; }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        IsPaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        IsPaused = false;
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }

    public void QuitGame() => Application.Quit();

    public void OnMouseEnter() => OnEnter?.Invoke();

    public void OnMouseExit() => OnExit?.Invoke();
}
