using System.Collections;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public int direction = 1;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        StartCoroutine(MoveBoomerang());
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(6 * direction, 0, 2 * direction);
    }

    private IEnumerator MoveBoomerang()
    {
        yield return new WaitForSeconds(2f);
        direction *= -1;
    }
}
