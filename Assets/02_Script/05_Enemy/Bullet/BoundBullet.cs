using UnityEngine;

public class BoundBullet : EnemyBullet
{
    [SerializeField] private float boundCnt;    // boundCnt만큼 팅기면 없어지기
    [SerializeField] private float currentCnt;
    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        currentCnt = 0;
        Boss.OnBulletClear += Explosion;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            currentCnt++;
            transform.right = rigidbody.velocity;
            if (currentCnt == boundCnt)
            {
                rigidbody.velocity = Vector2.zero;
                animator.SetTrigger("isExplosion");
            }
        }
        else if (collision.transform.CompareTag("Player"))
        { 
            rigidbody.velocity = Vector2.zero;
            GameController.instance.DamagePlayer(damage);
            animator.SetTrigger("isExplosion");
        }
    }
}
