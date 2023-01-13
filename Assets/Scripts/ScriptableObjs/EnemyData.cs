using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Characters/Enemy", order = 0)]
public class EnemyData : CharacterData
{
    public int _level;
    public bool _isShielded;
    public bool _isShoots;

    [Header("���� Is Shielded = true")]
    public float _koefShieldDefense;

    [Header("���� Is Shoots = true")]
    public TypesGun _typeGun;    
    public enum TypesGun { Unarmed, SingleGun, TwinGun };
}
