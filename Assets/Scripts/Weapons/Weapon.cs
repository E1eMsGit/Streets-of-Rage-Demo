using UnityEngine;

public class Weapon : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private int _durability;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ActivateWeapon(Sprite sprite, Color color, int durabilityValue, int damage)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        _durability = durabilityValue;
        GetComponent<Attack>().damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            _durability--;
            if (_durability <= 0)
            {
                spriteRenderer.sprite = null;
                GetComponentInParent<Player>().SetHoldingWeaponToFalse();
            }
        }
    }
}
