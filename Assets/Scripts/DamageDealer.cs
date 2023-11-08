using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 10;

    [SerializeField] private bool ofPlayer = false;

    [Header("Bullet config")]
    [SerializeField] private BulletType type;
    [SerializeField] private float value = 1f;
    [SerializeField] private float effectTime = 1f;

    public int GetDamage()
    {
        // Đạn lửa tăng dame, đạn băng làm chậm
        return damage;
    }
    public float GetValue()
    {
        return value;
    }
    public float GetEffectTime()
    {
        return effectTime;
    }
    public BulletType GetBulletType()
    {
        return type;
    }
    public void Hit()
    {
        Destroy(gameObject);
    }
    public bool IsPlayerBullet()
    {
        return ofPlayer;
    }
}
public enum BulletType
{
    Normal,
    Ice,
    Fire
}