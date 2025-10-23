using UnityEngine;

public class BotaoPulseController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AtivarPulsacao()
    {
        Debug.Log($"[{name}] Pulsação ativada!");
        animator.SetBool("IsPulsing", true);
    }

    public void DesativarPulsacao()
    {
        Debug.Log($"[{name}] Pulsação desativada!");
        animator.SetBool("IsPulsing", false);
    }

}
