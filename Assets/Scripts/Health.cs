using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isPlayer;
    [SerializeField] int health = 50;
    int maxHealth = 0;
    [SerializeField] int score = 50;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] bool applyCameraShake;

    [Header("Collecting")]
    [SerializeField] private bool useCollecting = false;
    [SerializeField] private float dropRate = 10f;
    [SerializeField] List<GameObject> collectItem = new();

    [Space(10)]
    CameraShake cameraShake;
    AudioPlayer audioPlayer;
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;
    void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
    }
    private void Start()
    {
        maxHealth = health;
    }

    private GameObject GetCollectItem()
    {
        int pos = Random.Range(0, collectItem.Count);

        return collectItem[pos];
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            if (isPlayer)
            {
                if (!damageDealer.IsPlayerBullet())
                {
                    TakeDamage(damageDealer.GetDamage());
                    PlayHitEffect();
                    audioPlayer.PlayDamageClip();
                    ShakeCamera();
                    damageDealer.Hit();
                }
            }
            else
            {
                int damage = damageDealer.GetDamage();
                switch (damageDealer.GetBulletType())
                {
                    case BulletType.Fire:
                        damage += (int)damageDealer.GetValue();
                        break;
                    case BulletType.Ice:
                        if (TryGetComponent<Pathfinder>(out var finder))
                        {
                            finder.ChangeSpeed(damageDealer.GetValue(), damageDealer.GetEffectTime());
                        }
                        break;

                }
                TakeDamage(damage);
                PlayHitEffect();
                audioPlayer.PlayDamageClip();
                ShakeCamera();
                damageDealer.Hit();
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (!isPlayer)
        {
            scoreKeeper.ModifyScore(score);
        }
        else
        {
            levelManager.LoadGameOver();
        }
        if (useCollecting)
        {
            float ran = Random.Range(0f, 100f);
            if (ran <= dropRate)
            {
                Instantiate(GetCollectItem(), transform.position, Quaternion.identity);
            }
        }
        Destroy(gameObject);
    }

    void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            ParticleSystem instance = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, instance.main.duration + instance.main.startLifetime.constantMax);
        }
    }

    void ShakeCamera()
    {
        if (cameraShake != null && applyCameraShake)
        {
            cameraShake.Play();
        }
    }
    public void ObjectHealth(int hp)
    {
        health = Mathf.Min(health + hp, maxHealth);
    }
}
