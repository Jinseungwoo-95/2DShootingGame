using System.Collections;
using UnityEngine;

public class FollowBullet : EnemyBullet
{
    public Transform target;
    [SerializeField] private float speed;
    private bool isExplosion;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Boss.OnBulletClear += Explosion;
        isExplosion = false;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(DestroyBullet());
    }

    void Update()
    {
        if (!isExplosion && target != null)
        {
            transform.right = target.position - transform.position;
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isExplosion = true;
        if (collision.CompareTag("Player"))
            GameController.instance.DamagePlayer(damage);
        animator.SetTrigger("isExplosion");
    }
}
