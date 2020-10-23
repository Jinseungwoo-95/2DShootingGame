using UnityEngine;

public class RotateEnemy : AttackType
{
    [SerializeField] private Transform bullets;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float moveSpeed;
    private Transform target;
    [SerializeField] private EnemyController enemyController;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(enemyController.currentState != EnemyState.Death)
        {
            bullets.Rotate(0, 0, transform.localScale.x * rotateSpeed * Time.deltaTime);
        }
    }
    public override void Attack(Transform _point)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            GameController.instance.DamagePlayer(1f);
    }
}
