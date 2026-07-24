using UnityEngine;

public class Bone : MonoBehaviour {
    private GameObject target;
    public float speed = 5f;
    [SerializeField] float damage = 25f;

    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] Vector3 rotationDirection = new Vector3(0, 0, 15);

    private AudioSource somAtaque;

    private void Start() => somAtaque = GetComponent<AudioSource>();

    private void Update() {
        if (target != null) {
            Vector2 direction = (target.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            transform.Rotate(rotateSpeed * rotationDirection * Time.deltaTime);
            // somAtaque.Play();

            // Se o projétil estiver perto o suficiente do alvo, atinge-o
            if (Vector2.Distance(transform.position, target.transform.position) < 0.1f) {
                HitTarget();
            }
        }
        
        else {
            Destroy(gameObject); // Destrói o projétil caso o alvo seja destruído antes
        }
    }

    public void SetTarget(GameObject target) => this.target = target;

    private void HitTarget() {
        IDamageable t = target.GetComponent<IDamageable>();

        t.TakeDamage(damage);
        Destroy(gameObject);
    }

    public void UpgradeStatus() => damage += 20f;
}