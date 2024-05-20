using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;

    private SpriteRenderer spriteRenderer;
    private bool isTrap;
    public static event Action OnPlayedDied;

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isTrap = false;
        GameController.OnReset += ResetHealth;
        HealthItem.OnHealthCollect += AddHeart;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            //nhan damage tu enemy
            TakeDamage(enemy.damage);
            SoundEffectManager.Play("PlayerHit");
        }

        Trap trap = collision.GetComponent<Trap>();
        if (trap && trap.damage > 0 && !isTrap )
        {
            TakeDamage(trap.damage);
            isTrap = true;
        }
        else if(trap && !isTrap && !transform.GetComponent<PlayerMovement>().isGround)
        {
            SoundEffectManager.Play("Bounce");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTrap = false;
    }


    //them mang khi nhat item mang tu quai 
    private void AddHeart(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthUI.UpdateHearts(currentHealth);
    }




    private void ResetHealth()//khi nhan retry thi cung reset thanh mau
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }



    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);
        StartCoroutine(FlashRed());

        if(currentHealth <= 0)
        {
            //gameover
            OnPlayedDied.Invoke();
        }
    }

    private IEnumerator FlashRed()//tao hieu ung nhap nhay khi dinh damage tu enemy
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color32(216, 134, 217, 255);
    }
}
