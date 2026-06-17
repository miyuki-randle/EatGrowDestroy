using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LifeHUD : MonoBehaviour
{
    public static LifeHUD Instance;
    public GameObject[] hearts;
    public int lives;
    public int maxLives;
    [SerializeField] private GameObject hurt;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;
        maxLives = hearts.Length;
        lives = maxLives;
    }

    public void HurtPlayer()
    {
        if (lives > 0)
        {
            Instantiate(hurt, FindObjectOfType<PlayerController>().transform);
            lives -= 1;
            RefreshHUD();
        }
    }

    public void HealPlayer()
    {
        if (lives < maxLives)
        {
            lives += 1;
            RefreshHUD();
        }
    }

    void RefreshHUD()
    {
        // loop through each heart
        for (int heart = 0; heart < hearts.Length; heart++)
        {
            // if the heart index is less than the number of lives then activate the heart
            if (heart < lives)
            {
                hearts[heart].SetActive(true);
            }
            // if the heart index is greater than or equal to the number of lives disable the heart
            else
            {
                hearts[heart].SetActive(false);
            }
        }
        // if there are no more lives, end game
        if (lives <= 0) StartCoroutine(GameManager.Instance.GameOver());
    }
}
