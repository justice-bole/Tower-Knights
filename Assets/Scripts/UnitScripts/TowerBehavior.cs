using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    private bool enemyTower;
    private bool playerTower;
    private int playerTowerStartingSpriteIndex = 0;
    private int enemyTowerStartingSpriteIndex = 0;
    private AudioManager audioManager;
    private SFXManager sfxManager;
    private SpriteRenderer towerSpriteRenderer;

    public int enemyTowerHealth = 6;
    public int playerTowerHealth = 6;
    public AudioClip battleMusic;
    public AudioClip gameOverMusic;
    public AudioClip vicotryMusic;
    public AudioClip towerDamageSfx;
    public GameSceneManager gameSceneManager;
    public SpawnManager spawnManager;
    public Sprite[] towerSprites;

    private void Awake()
    {
        playerTower = gameObject.CompareTag("PlayerTower");
        enemyTower = gameObject.CompareTag("EnemyTower");

        sfxManager =  GameObject.Find("SFXManager").GetComponent<SFXManager>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    private void Start()
    {
        audioManager = GameObject.Find("MusicManager").GetComponent<AudioManager>();
        playerTowerStartingSpriteIndex = 0;
        enemyTowerStartingSpriteIndex = 0; 
        towerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Time.timeScale = 1;
        audioManager.ChangeMusic(battleMusic);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerTower)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                playerTowerStartingSpriteIndex += 1;
                ChangeSprite(playerTowerStartingSpriteIndex);
                DecreaseTowerHealth(2, true);
                TowerHealthCheck();
                Destroy(collision.gameObject);
                spawnManager.totalRespawnTimerPlayer -= 1;
                Debug.Log("Enemy collided with your tower!");
            }
        }
        if (enemyTower)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                enemyTowerStartingSpriteIndex += 1;
                ChangeSprite(enemyTowerStartingSpriteIndex);
                DecreaseTowerHealth(2, false);
                TowerHealthCheck();
                Destroy(collision.gameObject);
                spawnManager.totalRespawnTimerEnemy -= 1;
                Debug.Log("You collided with the enemy tower!");
            }
        }
    }

    private void DecreaseTowerHealth(int damageAmount, bool isPlayer)
    {
        sfxManager.PlaySFX(towerDamageSfx);

        if (isPlayer)
        {
            playerTowerHealth -= damageAmount;
            Debug.Log("Player Tower just took: " + damageAmount + " damage! Remaining HP is: " + playerTowerHealth);
        }
        else
        {
            enemyTowerHealth -= damageAmount;
            Debug.Log("Enemy Tower just took: " + damageAmount + " damage! Remaining HP is: " + enemyTowerHealth);
        }
    }

    private void ChangeSprite(int i)
    {
        towerSpriteRenderer.sprite = towerSprites[i];
    }

    private void TowerHealthCheck()
    {
        if (enemyTowerHealth <= 0)
        {
            Victory();
        }
        else if (playerTowerHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameSceneManager.RevealGameOverScreen();
        Time.timeScale = 0;
        audioManager.ChangeMusic(gameOverMusic);
    }

    public void Victory()
    {
        gameSceneManager.RevealVictoryScreen();
        audioManager.ChangeMusic(vicotryMusic);
        Time.timeScale = 0;
    }


}
