using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UnitBehavior : MonoBehaviour
{

    [SerializeField] int attackDamage;
    [SerializeField] bool canShootArrows = false;
    [SerializeField] float arrowFireRate;
    [SerializeField] float currentMoveSpeed;
    [SerializeField] float defualtMoveSpeed; 
    [SerializeField] string unitID = "";

   
    private bool isArrowReady = true;
    private bool isUnitMoving = true;
    private bool isUnitFlipped = false;
    private bool isEnemyUnit = false;
    private bool isPlayerUnit = false;
    private bool isSpeedReduced = false;
    private Color playerColor = new Color(0.7f, .87f, 1f, 1f);
    private Color enemyColor = new Color(1f, .72f, .64f, 1f);
    private Color damageColor = new Color(.98f, .04f, 0f, .83f);
    private float mapLimitX = 9.0f;
    private GoldManager goldManager;
    private Rigidbody2D rbUnit;
    private SFXManager sFXManager;
    private SpriteRenderer srUnit;
    private TowerBehavior towerBehavior;

    public bool canBeDamaged = true; // used in arrowbehavior
    public float health = 3.0f;
    public AudioClip arrowShotSfx;
    public AudioClip coinSfx;
    public AudioClip golemDeathSfx;
    public AudioClip swordHitSfx;
    public ParticleSystem goldParticle;
    public ParticleSystem bloodParticle;
    public GameObject UnitArrow;

    private void Awake()
    {
        goldManager = GameObject.Find("GoldManager").GetComponent<GoldManager>();
        sFXManager = GameObject.Find("SFXManager").GetComponent<SFXManager>();
        towerBehavior = GameObject.Find("PlayerTower").GetComponent<TowerBehavior>();
        rbUnit = GetComponent<Rigidbody2D>();
        srUnit = GetComponent<SpriteRenderer>();
    }


    protected virtual void Update()
    {
        isPlayerUnit = gameObject.CompareTag("Player");
        isEnemyUnit = gameObject.CompareTag("Enemy");

        BoundaryChecker();
        HealthChecker();
        KillReward();
        ApplyUnitColor();
    }

    protected virtual void FixedUpdate()
    {
        if (rbUnit != null)
        {
            ApplyForce();
        } 
        else
        {
            print("Could not apply a rigidbody to object: " + gameObject.name);
        }
    }

    protected virtual void ApplyForce()
    {
        float xForce = currentMoveSpeed * Time.deltaTime;
        Vector2 force = new Vector2(xForce, 0);
        Vector2 unitScale = transform.localScale;

        if (isPlayerUnit)
        {
            rbUnit.AddForce(force);
        } 
        else if (isEnemyUnit)
        {
            rbUnit.AddForce(-force);
            if (isUnitFlipped) return;
            transform.localScale = new Vector3(unitScale.x * -1, unitScale.y);
            isUnitFlipped = true;
        }
        else
        {
            print("Could not apply force to object: " + gameObject.name);
        }
    }

    protected virtual void ApplyUnitColor()
    {
        if(isPlayerUnit && canBeDamaged)
        {
            srUnit.color = playerColor;
        }
        else if(isEnemyUnit && canBeDamaged)
        {
            srUnit.color = enemyColor;
        }
        else if (isPlayerUnit && !canBeDamaged || isEnemyUnit && !canBeDamaged)
        {
            srUnit.color = damageColor;
        }
        else
        {
            print("Could not apply color to object: " + gameObject.name);
        }
    }

    protected virtual void DestroyUnit()
    {
        Destroy(gameObject);
        if (unitID == "Golem")
        {
            sFXManager.PlaySFX(golemDeathSfx);
        }
    }
    
    protected virtual void HealthChecker()
    {
        if (health > 0) return;
        DestroyUnit();
    }

    protected virtual void PlayGoldParticleEffect()
    {
        if (goldParticle == null) return;
        Destroy(Instantiate(goldParticle.gameObject, transform.position, Quaternion.identity), goldParticle.startLifetime);
        sFXManager.PlaySFX(coinSfx);
    }

    public virtual void PlayBloodParticleEffect()
    {
        if (bloodParticle == null) return;
        Destroy(Instantiate(bloodParticle.gameObject, transform.position + new Vector3(0,.6f,0), Quaternion.identity), bloodParticle.startLifetime);
    }

    protected virtual void KillReward()
    {
        if (health <= 0 && gameObject.CompareTag("Enemy"))
        {
            PlayGoldParticleEffect();

            switch (unitID)
            {
                case ("Archer"):
                    goldManager.currentGold += goldManager.archerCost;
                    print("Gained: " + goldManager.archerCost + "GP");
                    break;
                case ("Knight"):
                    goldManager.currentGold += goldManager.knightCost;
                    print("Gained: " + goldManager.knightCost + "GP");
                    break;
                case ("Golem"):
                    goldManager.currentGold += goldManager.golemCost;
                    print("Gained: " + goldManager.golemCost + "GP");
                    break;
                case ("Cavalry"):
                    goldManager.currentGold += goldManager.cavalryCost;
                    print("Gained: " + goldManager.cavalryCost + "GP");
                    break;
                default:
                    print("Missing Kill Reward case for this unit!");
                    break;
            }       
        } 
    }

    protected virtual void BoundaryChecker()
    {
        if (transform.position.x > mapLimitX || transform.position.x < -mapLimitX)
        {
            DestroyUnit();
        }
    }

    public virtual IEnumerator ImmunityTimerCoroutine()
    {
        canBeDamaged = false;
        WaitForSeconds waitTime = new WaitForSeconds(1);
        yield return waitTime;
        canBeDamaged = true;
    }

    protected virtual IEnumerator ReducedSpeedCoolDownCoroutine()
    {
        isSpeedReduced = true;
        yield return new WaitForSeconds(3);
        isSpeedReduced = false;
    }

    protected virtual IEnumerator ShootArrowCooldownCoroutine(float arrowFireRateSeconds)
    {
        isArrowReady = false;
        yield return new WaitForSeconds(arrowFireRateSeconds);
        isArrowReady = true;
    }

    protected virtual void StopMovement()
    {
        currentMoveSpeed = 0;
        isUnitMoving = false;
    }

    protected virtual void StartMovement()
    {
        currentMoveSpeed = defualtMoveSpeed;
        isUnitMoving = true;
    }

    protected virtual void ShootArrow()
    {
        if(UnitArrow != null && canShootArrows && isArrowReady && !isUnitMoving)
        {
            sFXManager.PlaySFX(arrowShotSfx);
            float randomFloat = Random.Range(0.70f, 0.95f); //prevent arrow stalemates between 2 archers

            if (isPlayerUnit)
            {
                Vector3 playerArrowOffset = new Vector3(0.4f, randomFloat, 0); // arrow spawns at archer's feet without this
                GameObject ArrowUnit = Instantiate(UnitArrow, transform.position + playerArrowOffset, Quaternion.identity);
                ArrowUnit.gameObject.tag = "Player";
                StartCoroutine(ShootArrowCooldownCoroutine(arrowFireRate));
            }
            else if(isEnemyUnit)
            {
                Vector3 enemyArrowOffset = new Vector3(-0.4f, randomFloat, 0);
                GameObject ArrowUnit = Instantiate(UnitArrow, transform.position + enemyArrowOffset , Quaternion.identity);
                ArrowUnit.gameObject.tag = "Enemy";
                StartCoroutine(ShootArrowCooldownCoroutine(arrowFireRate));
            }
        }
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        //Player on Enemy Collisions
        if (isPlayerUnit)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                UnitBehavior enemy = collision.gameObject.GetComponent<UnitBehavior>();
                if (!canBeDamaged) return;
                enemy.health -= attackDamage;
                StartCoroutine(ImmunityTimerCoroutine());
                if (enemy.health < 1) return;
                enemy.PlayBloodParticleEffect();
                sFXManager.PlaySFX(swordHitSfx);
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                UnitBehavior otherPlayer = collision.gameObject.GetComponent<UnitBehavior>();
                if (otherPlayer != null && otherPlayer.currentMoveSpeed < currentMoveSpeed && isSpeedReduced == false)
                {
                    currentMoveSpeed = defualtMoveSpeed / 2;
                    StartCoroutine(ReducedSpeedCoolDownCoroutine());
                }
                else if (otherPlayer == null && isSpeedReduced == true)
                {
                    currentMoveSpeed = defualtMoveSpeed;
                }
            }
        }
        else if (isEnemyUnit)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                UnitBehavior player = collision.gameObject.GetComponent<UnitBehavior>();
                if (!canBeDamaged) return;
                player.health -= attackDamage;
                StartCoroutine(ImmunityTimerCoroutine());
                if (player.health < 1) return;
                player.PlayBloodParticleEffect();
                sFXManager.PlaySFX(swordHitSfx);
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                UnitBehavior otherEnemy = collision.gameObject.GetComponent<UnitBehavior>();
                if (otherEnemy != null && otherEnemy.currentMoveSpeed < currentMoveSpeed)
                {
                    currentMoveSpeed = defualtMoveSpeed / 2;
                    StartCoroutine(ReducedSpeedCoolDownCoroutine());
                }
                else if (otherEnemy == null && isSpeedReduced == true)
                {
                    currentMoveSpeed = defualtMoveSpeed;
                }  
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isPlayerUnit)
        {
            if(collision.gameObject.CompareTag("Enemy") && canShootArrows)
            {
                StopMovement();
                ShootArrow();
            }   
        }
        else if(isEnemyUnit)
        {
            if(collision.gameObject.CompareTag("Player") && canShootArrows)
            {
                StopMovement();
                ShootArrow();
            }   
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => StartMovement();
}
