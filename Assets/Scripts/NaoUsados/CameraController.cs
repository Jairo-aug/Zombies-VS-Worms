using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5f; // Velocidade de movimento da câmera.

    void Update()
    {
        // Calcula o movimento da câmera com base na entrada do jogador.
        float horizontal = Input.GetAxis("Horizontal"); // Para A e D (ou setas)
        float vertical = Input.GetAxis("Vertical"); // Para W e S (ou setas)

        // Movimenta a câmera para cima, para baixo, para a esquerda ou para a direita.
        Vector3 move = new Vector3(horizontal, vertical, 0) * cameraSpeed * Time.deltaTime;
        transform.position += move;
    }
}
