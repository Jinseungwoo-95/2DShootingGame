using UnityEngine;

public class BasicBullet : EnemyBullet
{
    [SerializeField] private float speed;
    private bool isExplosion;

    private void OnEnable()
    {
        Boss.OnBulletClear += Explosion;
        isExplosion = false;
    }

    void Update()
    {
        if(!isExplosion)
            transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isExplosion = true;
        if (collision.CompareTag("Player"))
            GameController.instance.DamagePlayer(damage);

        animator.SetTrigger("isExplosion");
    }
}
