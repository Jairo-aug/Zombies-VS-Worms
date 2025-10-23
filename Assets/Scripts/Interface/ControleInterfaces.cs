using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleInterfaces : MonoBehaviour
{
    public AudioSource clickSound; // Referência ao áudio de clique

    public void botaoIniciar()
    {
        PlayClickSound(); // Toca o som
        SceneManager.LoadScene("Jogo");
    }

    public void botaoCreditos()
    {
        PlayClickSound(); // Toca o som
        SceneManager.LoadScene("Créditos");
    }

    public void botaoQuit()
    {
        PlayClickSound(); // Toca o som
        Application.Quit();
    }

    public void botaoVoltarMenu()
    {
        PlayClickSound(); // Toca o som
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
