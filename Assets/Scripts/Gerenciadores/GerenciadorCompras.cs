using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GerenciadorCompras : MonoBehaviour
{
    public GameObject[] prefabsTorres; // Array de prefabs de torres
    public GameObject cursorTorre; // Sprite que segue o cursor quando uma torre é comprada

    public GameObject imagemIndicacaoZumbi; // Para indicar onde os zumbis podem ser colocados
    public GameObject imagemIndicacaoArmadilha; // Para indicar onde as armadilhas podem ser colocadas

    public Vector3 origemDoGrid;
    public float tamanhoQuadradinho;

    private bool relocatingTower = false;
    private bool torreSelecionada = false; // Controle se uma torre já foi comprada
    private GameObject torreAtual; // Torre atualmente selecionada
    private bool[] torresCompradas; // Array para rastrear se cada torre foi comprada
    private int[] custosTorres = { 100, 50, 150, 200, 50, 50, 50, 0 }; // Custos das torres

    public GameObject[] overlaysCinzas; // Array para os overlays cinzas
    public Sprite spriteNegativa;

    private FleshStack fleshStack; // Referência ao script que gerencia os pontos
    private SpriteRenderer cursorRenderer;
    private Sprite torreSprite;

    private AudioSource somColocartorre;

    private Dictionary<Vector2Int,GameObject> posicoesTorres;

    public Button[] botoes;

    private void OnEnable()
    {
        cursorRenderer = cursorTorre.GetComponent<SpriteRenderer>();
        somColocartorre = GetComponent<AudioSource>();

        // Inicializa o array de torres compradas com o tamanho do número de prefabs
        torresCompradas = new bool[prefabsTorres.Length];

        posicoesTorres = new Dictionary<Vector2Int,GameObject>();

        // Inicializa o cursorTorre para estar desativado no início
        cursorTorre.SetActive(false);

        // Desativa as imagens de indicação no início
        imagemIndicacaoZumbi.SetActive(false);
        imagemIndicacaoArmadilha.SetActive(false);

        // Obtém a referência ao script FleshStack
        fleshStack = FindObjectOfType<FleshStack>();
    }

    private void UpdateListaTorres(GameObject torre)
    {
        Vector2Int keyToRemove = Vector2Int.zero;

        foreach (var par in posicoesTorres)
        {
            if (par.Value == torre)
            {
                keyToRemove = par.Key;
            }
        }

        if (keyToRemove != null)
        {
            posicoesTorres.Remove(keyToRemove);
           
            Zombie z = torreAtual.GetComponent<Zombie>();
            z.OnTorreMorreu += UpdateListaTorres;
        }
        
    }

    void AtualizarUI()
    {
        if (botoes == null || botoes.Length == 0) return;
        if (fleshStack == null) return;

        for (int i = 0; i < botoes.Length; i++)
        {
            if (botoes[i] == null) continue;

            // Animator animator = botoes[i].GetComponent<Animator>();
            // if (animator == null) continue;

            bool pontosSuficientes = fleshStack.rottenPoints >= custosTorres[i];

            // Se pontos são suficientes, o botão começa a pulsar
            // animator.enabled = pontosSuficientes;

            // Ativar ou desativar os overlays
            if (overlaysCinzas != null && overlaysCinzas.Length > i)
            {
                overlaysCinzas[i].SetActive(!pontosSuficientes);
            }
        }
    }

    public void ComprarTorre(int idTorre)
    {
        if (idTorre >= 0 && idTorre < prefabsTorres.Length)
        {
            int custo = custosTorres[idTorre];

            // Verifica se o jogador tem pontos suficientes
            if (fleshStack.rottenPoints < custo)
            {
                Debug.Log("rottenPoints insuficientes para comprar esta torre!");
                return;
            }

            // Verifica se já há uma torre selecionada
            if (torreSelecionada)
            {
                Debug.Log("Já há uma torre selecionada!");
                return;
            }

            // Deduz os pontos e configura o cursor
            fleshStack.ModifyPointQuantity(-custo);
            torreSelecionada = true;
            torreAtual = prefabsTorres[idTorre];

            // Ativa a imagem de indicação apropriada
            if (torreAtual.TryGetComponent(out Construcao tipoConstrucao))
            {
                // Desativa todas as imagens primeiro
                imagemIndicacaoZumbi.SetActive(false);
                imagemIndicacaoArmadilha.SetActive(false);

                // Ativa a imagem correspondente
                if (tipoConstrucao.tipo == TipoConstrucao.Torre)
                {
                    imagemIndicacaoZumbi.SetActive(true);
                }
                else if (tipoConstrucao.tipo == TipoConstrucao.Armadilha)
                {
                    imagemIndicacaoArmadilha.SetActive(true);
                }
            }

            // Configura o cursor
            torreSprite = torreAtual.GetComponentInChildren<SpriteRenderer>().sprite;
            if (torreSprite != null)
            {
                cursorRenderer.sprite = torreSprite;
                cursorTorre.SetActive(true);
                cursorTorre.transform.localScale = new Vector3(0.5f, 0.5f, 0);
            }
        }
    }

    private void Update() {
        if (torreSelecionada && cursorTorre.activeSelf) {
            DragTowerOnMap();
        }

        AtualizarUI();
    }
    
    private void DragTowerOnMap() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorTorre.transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);

        UpdateMouseSprite(cursorTorre.transform.position, torreAtual);

        if (Input.GetMouseButtonDown(0)) {
            if (PodeColocarTorre(cursorTorre.transform.position, torreAtual)) {
                if (relocatingTower) {
                    RelocateTower(cursorTorre.transform.position, torreAtual);
                }

                else {
                    ColocarTorreNoMapa(cursorTorre.transform.position);
                }
            }
        }
    }

    private void ColocarTorreNoMapa(Vector3 positionToSpawn)
    {
        int x, y;
        GetXZ(positionToSpawn, out x, out y);

        if ((x == 6 && y == 6) || (x == 6 && y == 7) || (x == 7 && y == 6) || (x == 7 && y == 7))
        {
            return;
        }

        GameObject temp = Instantiate(torreAtual, GetWorldPosition(x, y), Quaternion.identity);
        posicoesTorres.Add(new Vector2Int(x, y),temp);
        somColocartorre.Play();
        
        Zombie torre = temp.GetComponent<Zombie>();
        torre.OnTorreMorreu += UpdateListaTorres;

        SubscribeToTowerEvents(torre);

        // Desativa as imagens de indicação
        imagemIndicacaoZumbi.SetActive(false);
        imagemIndicacaoArmadilha.SetActive(false);

        // Reseta o cursor
        cursorTorre.SetActive(false);
        torreSelecionada = false;
    }

    private void SubscribeToTowerEvents(Zombie z) {
        if (z.TryGetComponent<TheTower>(out var theTower)) {
            theTower.Relocate += () => {
                relocatingTower = true;
                torreSelecionada = true;
                torreAtual = theTower.gameObject;

                // Ativa a imagem de indicação apropriada
                if (torreAtual.TryGetComponent(out Construcao tipoConstrucao))
                {
                    // Desativa todas as imagens primeiro
                    imagemIndicacaoZumbi.SetActive(false);
                    imagemIndicacaoArmadilha.SetActive(false);

                    // Ativa a imagem correspondente
                    if (tipoConstrucao.tipo == TipoConstrucao.Torre)
                    {
                        imagemIndicacaoZumbi.SetActive(true);
                    }
                    else if (tipoConstrucao.tipo == TipoConstrucao.Armadilha)
                    {
                        imagemIndicacaoArmadilha.SetActive(true);
                    }
                }

                // Configura o cursor
                torreSprite = torreAtual.GetComponentInChildren<SpriteRenderer>().sprite;
                if (torreSprite != null)
                {
                    cursorRenderer.sprite = torreSprite;
                    cursorTorre.SetActive(true);
                    cursorTorre.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                }
            };
        }
    }

    public void RelocateTower(Vector3 positionToSpawn, GameObject tower) {
        Debug.Log("Entrou em relocatetower");
        int x, y;
        GetXZ(positionToSpawn, out x, out y);

        if ((x == 6 && y == 6) || (x == 6 && y == 7) || (x == 7 && y == 6) || (x == 7 && y == 7))
        {
            return;
        }

        Vector2Int newPos = new(x, y);
        Vector3 newWorldPos = GetWorldPosition(x, y);

        Vector2Int oldPos = default;
        bool found = false;

        foreach (var entry in posicoesTorres){
            if (entry.Value == torreAtual) {
                oldPos = entry.Key;
                found = true;
                break;
            }
        }

        if (found) {
            posicoesTorres.Remove(oldPos);
            posicoesTorres[newPos] = torreAtual;

            torreAtual.transform.position = newWorldPos;
        }

        somColocartorre.Play();
        
        // Desativa as imagens de indicação
        imagemIndicacaoZumbi.SetActive(false);
        imagemIndicacaoArmadilha.SetActive(false);

        // Reseta o cursor
        cursorTorre.SetActive(false);
        torreSelecionada = false;
        relocatingTower = false;
    }

    private void UpdateMouseSprite(Vector3 position, GameObject construcao)
    {
        if (PodeColocarTorre(position, construcao))
        {
            cursorRenderer.sprite = torreSprite;
        }
        else
        {
            cursorRenderer.sprite = spriteNegativa;
        }
    }

    private void GetXZ(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origemDoGrid).x / tamanhoQuadradinho);
        y = Mathf.FloorToInt((worldPosition - origemDoGrid).y / tamanhoQuadradinho);
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y, 0) * tamanhoQuadradinho + origemDoGrid;
    }

    private bool PodeColocarTorre(Vector3 position, GameObject construcao)
    {
        int x, y;
        GetXZ(position, out x, out y);

        if (construcao.TryGetComponent(out Construcao tipoConstrucao))
        {
            if (tipoConstrucao.tipo == TipoConstrucao.Torre)
            {
                if ((x >= 0 && y >= 0) && (x <= 13 && y <= 13)) return true && !posicoesTorres.ContainsKey(new Vector2Int(x, y));
            }

            if (tipoConstrucao.tipo == TipoConstrucao.Armadilha)
            {
                if ((x < 0 || y < 0) || (x > 13 || y > 13)) return true && !posicoesTorres.ContainsKey(new Vector2Int(x, y));
            }
        }

        return false;
    }
}