using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingItem : MonoBehaviour
{
    public const string playerTag = "Player";
    [SerializeField] private GameObject collectingItemInventory;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float value = 2f;
    [SerializeField] private ItemType type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            InventoryController.Instance.AddCollection(collectingItemInventory, value, type);
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        transform.position -= speed * Time.deltaTime * Vector3.up;
    }
}
