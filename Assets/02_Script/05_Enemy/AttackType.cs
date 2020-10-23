using System.Collections;
using UnityEngine;

public abstract class AttackType : MonoBehaviour
{
    protected const string BASIC = "BASIC", FOLLOW = "FOLLOW", BOUND = "BOUND";

    public abstract void Attack(Transform _point);
}
