using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CharactersLevels : MonoBehaviour
{
    [SerializeField] int currentStatus, currentLevel;
    [SerializeField] int maxStatus = 450;

    [SerializeField] SliderBar expBar;
    [SerializeField] TextMeshProUGUI diaTexto; // Usando TextMeshProUGUI para exibir o texto

    [SerializeField] GeradorInimigo listaInimigos;

    private void Start()
    {
        currentStatus = 0;
        currentLevel = 1;
        ExperienceManager.Instance.OnExperienceChange += HandleExperienceChange;
        expBar.Set(maxStatus, 0);
        
        MostrarDiaAtual(); // Mostrar "DIA 01" no início do jogo
    }

    private void OnDisable()
    {
        ExperienceManager.Instance.OnExperienceChange -= HandleExperienceChange;    
    }

    private void HandleExperienceChange(int newStatus)
    {
        currentStatus += newStatus;
        expBar.UpdateSlider(currentStatus);
        if (currentStatus >= maxStatus)
        {
            Debug.Log("Loop infinito");
            LevelUp();
        }
    }

    private void LevelUp()
    {
        currentLevel++;
        currentStatus = 0; // Reinicia o status atual
        maxStatus += 300; // Aumenta o limite para o próximo nível
        expBar.Set(maxStatus, 0); // Reinicia a barra de progresso

        // Atualiza o estado para o próximo dia
        if (currentLevel <= 3)
        {            
            AvancarParaProximoDia();
        }
        else
        {
            AtivarVitoria();
        }
    }

    private void AvancarParaProximoDia()
    {
        MostrarDiaAtual();
        listaInimigos.levelUp();
        ExperienceManager.Instance.EndDay(currentLevel);
    }

    private void AtivarVitoria()
    {
        SceneManager.LoadScene("Vitória");
    }

    private void MostrarDiaAtual()
    {
        if (diaTexto != null)
        {
            diaTexto.text = "Dia 0" + currentLevel;
            diaTexto.gameObject.SetActive(true);
        }
    }
}
