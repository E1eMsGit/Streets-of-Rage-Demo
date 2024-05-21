using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : Enemy
{
    public GameObject boomerang;
    public float minBoomerangTime;
    public float maxBoomerangTime;

    private MusicController _musicController;

    void Awake()
    {
        Invoke("ThrowBoomerang", Random.Range(minBoomerangTime, maxBoomerangTime));
        _musicController = FindObjectOfType<MusicController>();
        _musicController.PlaySong(_musicController.bossSong);
    }

    private void ThrowBoomerang()
    {
        if (!_isDead)
        {
            _animator.SetTrigger("Boomerang");
            GameObject tempBoomerang = Instantiate(boomerang, transform.position, transform.rotation);
           
            if (_isFacingRight)
            {
                tempBoomerang.GetComponent<Boomerang>().direction = 1;
            }
            else
            {
                tempBoomerang.GetComponent<Boomerang>().direction = -1;
            }
            
            Invoke("ThrowBoomerang", Random.Range(minBoomerangTime, maxBoomerangTime));
        }
    }

    private void BossDefeated()
    {
        _musicController.PlaySong(_musicController.levelClearSong);
        FindObjectOfType<UIManager>().UpdateDisplayMessage("Level Clear");
        Invoke("LoadScene", 8f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
