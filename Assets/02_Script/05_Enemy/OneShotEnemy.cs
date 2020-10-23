﻿using UnityEngine;

public class OneShotEnemy : AttackType
{
    public override void Attack(Transform _point)
    {
        GameObject obj = ObjectPoolingManager.instance.GetBullet(BASIC);
        obj.transform.position = _point.position;
        obj.transform.rotation = _point.rotation;
    }
}