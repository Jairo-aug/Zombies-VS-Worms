using System.Collections;
using UnityEngine;
using TMPro;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    public delegate void ExperienceChangeHandler(int amount);
    public event ExperienceChangeHandler OnExperienceChange;

    [SerializeField] private Canvas passagemDiaCanvas; // Referência ao Canvas PassagemDia
    [SerializeField] private TextMeshProUGUI DiaTexto;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddExperience(int amount)
    {
        OnExperienceChange?.Invoke(amount);
    }

    // Método para ativar o Canvas PassagemDia
    public void ActivatePassagemDiaCanvas()
    {
        if (passagemDiaCanvas != null)
        {
            passagemDiaCanvas.gameObject.SetActive(true);
            StartCoroutine(DesativarTela());
        }
        else
        {
            Debug.LogWarning("Canvas PassagemDia não foi atribuído no Inspector.");
        }
    }

    // Exemplo de método para encerrar o dia
    public void EndDay(int currentLevel)
    {
        DiaTexto.text = "Day 0" + currentLevel;
        ActivatePassagemDiaCanvas();
    }

    private IEnumerator DesativarTela()
    {
        yield return new WaitForSeconds(2);
        passagemDiaCanvas.gameObject.SetActive(false);
    }
}
