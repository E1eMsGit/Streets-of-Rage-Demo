using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    public WeaponItem weapon;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = weapon.sprite;
        _spriteRenderer.color = weapon.color;
    }
}
