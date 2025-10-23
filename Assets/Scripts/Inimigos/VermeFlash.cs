using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VermeFlash : Verme
{
    [SerializeField] private float bonusSpeed = 15f; 
    
    public new void Start()
    {
        base.Start(); // Garante que o Start da classe base seja chamado
        maxSpeed += bonusSpeed; // Adiciona um bônus de velocidade
        currentSpeed = maxSpeed; // Atualiza a velocidade atual

        expAmount = 40; // Modifica o valor diretamente na classe base
    }
}
