using UnityEngine;

public class MultiShotEnemy : AttackType
{
    [SerializeField] private int cnt;
    [SerializeField] private float angle;

    public override void Attack(Transform _point)
    {
        int decrease = angle == 360 ? 0 : 1;
        float gap = cnt > 1 ? angle / (cnt - decrease) : 0;
        float startAngle = _point.eulerAngles.z - (angle / 2.0f);
        float theta;
        Quaternion rot;

        for (int i = 0; i < cnt; i++)
        {
            theta = startAngle + gap * (float)i;
            rot = Quaternion.Euler(0, 0, theta);
            GameObject obj = ObjectPoolingManager.instance.GetBullet(BASIC);
            obj.transform.position = _point.position;
            obj.transform.rotation = rot;
        }
    }
}
