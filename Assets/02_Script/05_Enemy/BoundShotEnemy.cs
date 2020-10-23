using UnityEngine;

public class BoundShotEnemy : AttackType
{
    private Transform target;
    [SerializeField] float force;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void Attack(Transform _point)
    {
        GameObject obj = ObjectPoolingManager.instance.GetBullet(BOUND);
        obj.transform.position = _point.position;
        obj.transform.rotation = _point.rotation;
        Vector2 dir = target.transform.position - _point.position;
        dir.Normalize();
        obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x * force, dir.y * force));
    }
}
