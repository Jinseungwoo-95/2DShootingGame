using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected float damage;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Boss.OnBulletClear += Explosion;
    }

    private void OnDisable()
    {
        Boss.OnBulletClear -= Explosion;
    }

    protected void Explosion()
    {
        animator.SetTrigger("isExplosion");
    }

    private void ClearBullet()
    { 
        gameObject.SetActive(false);
    }
}
