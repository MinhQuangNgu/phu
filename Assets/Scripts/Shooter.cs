using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifeTime = 5f;
    [SerializeField] float baseFiringRate = 0.2f;
    [Header("AI")]
    [SerializeField] bool useAI;
    [SerializeField] float firingRateVariance = 0f;
    [SerializeField] float minimumFiringRate = 0.1f;
    [HideInInspector] public bool isFiring;
    Coroutine firingCoroutine;
    AudioPlayer audioPlayer;


    // Player bullet config
    GameObject currentBullet;
    float bulletRemainTime = 0f;
    void Awake()
    {
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }
    void Start()
    {
        if (useAI)
        {
            isFiring = true;
        }
        currentBullet = projectilePrefab;
    }

    // Update is called once per frame
    void Update()
    {
        bulletRemainTime = Mathf.Max(0f, bulletRemainTime - Time.deltaTime);
        if (bulletRemainTime == 0f)
        {
            currentBullet = projectilePrefab;
        }
        Fire();
    }
    void Fire()
    {
        if (isFiring && firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        else if (!isFiring && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject bullet = Instantiate(currentBullet, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.up * projectileSpeed;
            }
            Destroy(bullet, projectileLifeTime);
            float timeToNextProjectile = Random.Range(baseFiringRate - firingRateVariance,
                                                        baseFiringRate + firingRateVariance);
            timeToNextProjectile = Mathf.Clamp(timeToNextProjectile, minimumFiringRate, float.MaxValue);
            audioPlayer.PlayShootingClip();

            yield return new WaitForSeconds(timeToNextProjectile);
        }
    }
    public void SwitchBullet(GameObject newBullet, float bulletRemainTime)
    {
        this.bulletRemainTime = bulletRemainTime;
        currentBullet = newBullet;
    }
}
