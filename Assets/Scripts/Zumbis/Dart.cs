//Refatorar
using UnityEngine;

public class Dart : MonoBehaviour {
    private Transform target;
    public float speed = 5f;
    [SerializeField] float damage = 25f;

    private AudioSource somAtaque;
    private Vector2 direction;

    private void Start() => somAtaque = GetComponent<AudioSource>();

    private void Update() {
        if (target != null) {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        
        else {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 direction) {
        this.direction = direction;

        SetTarget(direction);
    }

    private void HitEnemy(Enemy enemy) {

    }

    public void SetTarget(Vector2 direction) {
        GameObject targetObj = new GameObject("Dart Target");
        target = targetObj.transform;
        targetObj.transform.parent = transform;

        bool facingLeft = transform.position.x < 0;

        target.position = new(direction.x + 100 * (facingLeft ? -1 : 1), direction.y, 0);
        GetComponent<SpriteRenderer>().flipX = !facingLeft;
    }

    public void UpgradeStatus() => damage += 20f;
}