using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    private bool canSpawnPlayer = true;
    private bool canSpawnEnemy = true;
    private GoldManager goldManager;
    private AudioSource towerSfxManager;
    private Dialog dialog;
    private Vector2 enemySpawnPosition = new Vector2(6.65f, -4.47f);
    private Vector2 playerSpawnPosition = new Vector2(-6.65f, -4.47f);
    private Vector2 spearSpawnPosition = new Vector2(-4.7f, -1.6f);
    private Quaternion enemyRotation = Quaternion.identity;
    private Quaternion playerRotation = Quaternion.identity;
    private Quaternion spearRotation = Quaternion.Euler(new Vector3(0, 0, -25));

    public float respawnTimerPlayer = 3;
    public float respawnTimerEnemy = 3;
    public float totalRespawnTimerPlayer = 3;
    public float totalRespawnTimerEnemy = 3;
    public AudioClip ballistaShotSfx;
    public AudioClip buttonSuccessSfx;
    public AudioClip buttonFailSfx;
    public GameObject unitKnight;
    public GameObject unitCavalry;
    public GameObject unitArcher;
    public GameObject unitSpear;
    public GameObject unitGolem;
    public GameObject[] enemies;
    public TextMeshProUGUI playerButtonTxt;
    public TextMeshProUGUI enemyButtonTxt;


    private void Awake()
    {
        goldManager = GameObject.Find("GoldManager").GetComponent<GoldManager>();
        dialog = GameObject.Find("DialogManager").GetComponent<Dialog>();
        towerSfxManager = gameObject.AddComponent<AudioSource>();
        towerSfxManager.volume = 0.15f;
    }

    private void Update() =>  Timer();

    public void SpawnUnit(GameObject unitType, int goldCost, bool canSpawn, Vector2 spawnPosition, Quaternion rotation, AudioClip successSoundFX, AudioClip failSoundFX)
    {
        if(unitType != null && canSpawn && goldManager.currentGold >= goldCost && !dialog.isDialogPresent)
        {
            PlayTowerSFX(successSoundFX);
            GameObject playerUnit = Instantiate(unitType, spawnPosition, rotation);
            playerUnit.gameObject.tag = "Player";
            goldManager.currentGold -= goldCost;
            StartCoroutine(RespawnTimerPlayerCoroutine());
        }
        else if(unitType != null && !canSpawn && goldManager.currentGold < goldCost)
        {
            PlayTowerSFX(failSoundFX);
            StartCoroutine(RedErrorNotifyCoroutine(playerButtonTxt));
            StartCoroutine(RedErrorNotifyCoroutine(goldManager.currentGoldText));
        }
        else if(unitType !=null && canSpawn && goldManager.currentGold < goldCost)
        {
            PlayTowerSFX(failSoundFX);
            StartCoroutine(RedErrorNotifyCoroutine(goldManager.currentGoldText));
        }
        else if (unitType != null && !canSpawn)
        {
            StartCoroutine(RedErrorNotifyCoroutine(playerButtonTxt));
            PlayTowerSFX(failSoundFX);
        }
        else
        {
            PlayTowerSFX(failSoundFX);
        }
    }

    public void SpawnKnight()
    {
        SpawnUnit(unitKnight, goldManager.knightCost, canSpawnPlayer, playerSpawnPosition, playerRotation, buttonSuccessSfx, buttonFailSfx);
    }

    public void SpawnCavalryUnit()
    {
        SpawnUnit(unitCavalry, goldManager.cavalryCost, canSpawnPlayer, playerSpawnPosition, playerRotation, buttonSuccessSfx, buttonFailSfx);
    }

    public void SpawnArcher()
    {
        SpawnUnit(unitArcher, goldManager.archerCost, canSpawnPlayer, playerSpawnPosition, playerRotation, buttonSuccessSfx, buttonFailSfx);
    }

    public void SpawnGolem()
    {
        SpawnUnit(unitGolem, goldManager.golemCost, canSpawnPlayer, playerSpawnPosition, playerRotation, buttonSuccessSfx, buttonFailSfx);
    }

    public void SpawnSpear()
    {
        SpawnUnit(unitSpear, goldManager.spearCost, canSpawnPlayer, spearSpawnPosition, spearRotation, ballistaShotSfx, buttonFailSfx);
    }

    public void SpawnEnemyUnit()
    {
        if (enemies != null && canSpawnEnemy && !dialog.isDialogPresent)
        {
            GameObject enemyUnit = Instantiate(enemies[Random.Range(0, enemies.Length)], enemySpawnPosition, enemyRotation);
            enemyUnit.gameObject.tag = "Enemy";
            StartCoroutine(RespawnTimerEnemyCoroutine());
        }
        else
        {
            Debug.Log("SpawnEnemyUnit error, enemy may be null. Enemy: " + gameObject.name);
        }
    }

    private void Timer()
    {
        if (!canSpawnPlayer && respawnTimerPlayer > 0)
        {
            respawnTimerPlayer -= Time.deltaTime;
            playerButtonTxt.text = "Hire Cooldown: " + respawnTimerPlayer.ToString("f0");
        }
        else
        {
            respawnTimerPlayer = totalRespawnTimerPlayer;
            playerButtonTxt.text = "Hire Knight";
        }
        if (!canSpawnEnemy && respawnTimerEnemy > 0)
        {
            respawnTimerEnemy -= Time.deltaTime;
            enemyButtonTxt.text = "Hire Cooldown: " + respawnTimerEnemy.ToString("f0");
        }
        else
        {
            respawnTimerEnemy = totalRespawnTimerEnemy;
            enemyButtonTxt.text = "Spawn Enemy";
        }
    }

    private IEnumerator RespawnTimerPlayerCoroutine()
    {
        canSpawnPlayer = false;
        yield return new WaitForSeconds(respawnTimerPlayer);
        canSpawnPlayer = true;  
    }

    private IEnumerator RespawnTimerEnemyCoroutine()
    {
        canSpawnEnemy = false;
        yield return new WaitForSeconds(respawnTimerEnemy);
        canSpawnEnemy = true;
    }

    private IEnumerator RedErrorNotifyCoroutine(TextMeshProUGUI buttonText)
    {
        buttonText.color = Color.red;
        yield return new WaitForSeconds(.5f);
        buttonText.color = Color.black;
    }

    public void PlayTowerSFX(AudioClip Sfx)
    {
        towerSfxManager.clip = Sfx;
        towerSfxManager.Play();
    }

}
