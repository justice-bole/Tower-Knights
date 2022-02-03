using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    [SerializeField] float arrowDamage = .5f;
    [SerializeField] float arrowMoveSpeed = 100f; 

    private bool isFlipped = false;
    private bool enemyArrow = false;
    private bool playerArrow = false;
    private Color enemyColor = new Color(1f, .72f, .64f, 1f);
    private Color playerColor = new Color(0.7f, .87f, 1f, 1f);
    private Rigidbody2D rbArrow;
    private SpriteRenderer srArrow;

    private void Awake()
    {
        rbArrow = GetComponent<Rigidbody2D>();
        srArrow = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update()
    {
        playerArrow = gameObject.CompareTag("Player");
        enemyArrow = gameObject.CompareTag("Enemy");

        ApplyColor();
    }
    protected virtual void FixedUpdate()
    {
        if (rbArrow == null) return;
        ApplyForce();
    }

    protected virtual void ApplyForce()
    {
        float xForce = arrowMoveSpeed * Time.deltaTime;
        Vector2 force = new Vector2(xForce, 0);
        Vector2 unitScale = transform.localScale;

        if (playerArrow)
        {
            rbArrow.AddForce(force);
        }
        else if (enemyArrow)
        {
            rbArrow.AddForce(-force);
            if (isFlipped) return;
            transform.localScale = new Vector3(unitScale.x * -1, unitScale.y);
            isFlipped = true;
        }
    }

    protected virtual void ApplyColor()
    {
        if (playerArrow)
        {
            srArrow.color = playerColor;
        }
        else if (enemyArrow)
        {
            srArrow.color = enemyColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerArrow)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                UnitBehavior enemy = collision.gameObject.GetComponent<UnitBehavior>();
                if(enemy != null && enemy.canBeDamaged)
                {
                    enemy.health -= arrowDamage;
                    enemy.PlayBloodParticleEffect();
                }
                
                Destroy(gameObject);
            }
        }
        else if(enemyArrow)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                UnitBehavior player = collision.gameObject.GetComponent<UnitBehavior>();
                if(player != null && player.canBeDamaged)
                {
                    player.health -= arrowDamage;
                    player.PlayBloodParticleEffect();
                }

                Destroy(gameObject);
            }
        }
        
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(.25f);
    }

}

