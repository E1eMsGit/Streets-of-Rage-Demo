using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] player;

	void Awake()
	{

		int index = FindObjectOfType<GameManager>().characterIndex - 1;
		Instantiate(player[index], transform.position, transform.rotation);

	}
}
