using UnityEngine;

public class FollowShotEnemy : AttackType
{
    public override void Attack(Transform _point)
    {
        GameObject obj = ObjectPoolingManager.instance.GetBullet(FOLLOW);
        obj.transform.position = _point.position;
        obj.transform.rotation = _point.rotation;
    }
}
