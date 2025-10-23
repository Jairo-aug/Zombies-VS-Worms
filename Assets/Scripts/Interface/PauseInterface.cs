using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseInterface : MonoBehaviour
{
    public GameObject[] pauseMenu;
    public bool isPaused;

    public AudioSource clickSound;
    
    void Start()
    {
        pauseMenu[0].SetActive(false);        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused) {
                float realTimeDelta = Time.unscaledDeltaTime;
                ResumeGame();
            } 
            else {PauseGame();}
        }
    }

    public void PauseGame()
    {
        pauseMenu[0].SetActive(true);
        if (!pauseMenu[1].activeInHierarchy)
        {
            for (int i = 2; i < pauseMenu.Length; i++)
            {
                if (pauseMenu[i].activeInHierarchy) {pauseMenu[i].SetActive(false);}
            }
            pauseMenu[1].SetActive(true);
        }
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        PlayClickSound();
        pauseMenu[0].SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void BotaoHTP()
    {
        PlayClickSound(); // Toca o som
        SceneManager.LoadScene("HowToPlay");
    }

    public void BotaoOptions()
    {
        PlayClickSound();
        SceneManager.LoadScene("Options");
    }

    public void BotaoQuit()
    {
        PlayClickSound(); // Toca o som
        ResumeGame();
        SceneManager.LoadScene("Menu");
    }

    private void PlayClickSound()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
    }

}
