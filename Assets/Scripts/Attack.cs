using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        Player player = other.GetComponent<Player>();
        
        if (enemy != null)
        {
            enemy.TookDamage(damage);
        }
        if (player != null)
        {
            player.TookDamage(damage);
        }
    }
}
