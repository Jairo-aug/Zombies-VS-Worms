using UnityEngine;

public class Projetil : MonoBehaviour
{
    private GameObject target;
    public float speed = 5f; // Velocidade do projétil
    [SerializeField] float damage = 25f; // Dano causado pelo projétil

    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] Vector3 rotationDirection = new Vector3(0, 0, 15);

    private AudioSource somAtaque;

    void Start()
    {
        somAtaque = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.Rotate(rotateSpeed * rotationDirection * Time.deltaTime);
            // somAtaque.Play();

            // Se o projétil estiver perto o suficiente do alvo, atinge-o
            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
            {
                HitTarget();
            }
        }
        else
        {
            Destroy(gameObject); // Destrói o projétil caso o alvo seja destruído antes
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    void HitTarget()
    {
        // Aplica dano ao inimigo, se ele tiver o componente necessário
        // Refatorável
        Worm minhoca = target.GetComponent<Worm>();
        Larva larva = target.GetComponent<Larva>();
        Verm verme = target.GetComponent<Verm>();

        if (minhoca != null)
        {
            minhoca.TakeDamage(damage);
        }
        else if (larva != null)
        {
            larva.TakeDamage(damage);
        }
        else if (verme != null)
        {
            verme.TakeDamage(damage);
        }

        // Destrói o projétil após atingir o alvo
        Destroy(gameObject);
    }

    public void UpgradeStatus()
    {
        damage += 20f;
    }
}
