using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image playerCurrentHealthbar;
    public Image playerImage;
    public Text playerName;
    public Text livesText;

    public GameObject enemyUI;
    public Image enemyCurrentHealthbar;
    public Text enemyName;
    public Image enemyImage;

    public float enemyUITime = 4f;

    private float enemyTimer;
    private Player player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        playerName.text = player.playerName;
        playerImage.sprite = player.playerImage;
    }
	
	// Update is called once per frame
	void Update () {
        enemyTimer += Time.deltaTime;
        if (enemyTimer >= enemyUITime)
        {
            enemyUI.SetActive(false);
            enemyTimer = 0;
        }
	}

    private void UpdateHealthbar(int maxHealth, int currentHealth, Image healthBar)
    {
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        float ratio = (float)(currentHealth) / maxHealth;
        healthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);

        Debug.Log(ratio + " | current: " + currentHealth + " | max: " + maxHealth);
    }

    public void UpdateHealth(int amount)
    {
        UpdateHealthbar(player.maxHealth, amount, playerCurrentHealthbar);
    }

    public void UpdateEnemyUI(int maxHealth, int currentHealth, string name, Sprite image)
    {
        enemyName.text = name;
        enemyImage.sprite = image;

        enemyTimer = 0;
        enemyUI.SetActive(true);

        UpdateHealthbar(maxHealth, currentHealth, enemyCurrentHealthbar);
    }
}
