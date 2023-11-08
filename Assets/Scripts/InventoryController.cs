using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    GameObject player;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void AddCollection(GameObject item, float remainTime, ItemType itemType)
    {
        if (player != null)
        {
            switch (itemType)
            {
                case ItemType.Bullet:
                    if (player.TryGetComponent<Shooter>(out var playerShoot))
                    {
                        playerShoot.SwitchBullet(item, remainTime);
                    }
                    break;
                case ItemType.HP:
                    if (player.TryGetComponent<Health>(out var playerHealth))
                    {
                        playerHealth.ObjectHealth((int)remainTime);
                    }
                    break;
                case ItemType.Boom:
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (var enemy in enemies)
                    {
                        Destroy(enemy);
                    }
                    break;
            }
        }
    }
}
public enum ItemType
{
    Bullet,
    HP,
    Boom
}