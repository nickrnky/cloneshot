using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour {

	private int CurrentHealth;
    public int MaxHealth = 100;
    public Text HealthBar;
    public Image damageImage;
    public AudioSource DeathSound;
    public AudioSource DamageSound;

    public FPSInput PlayerMovement;

    private bool Damaged = false;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    public float flashSpeed = 5f;                             

    private bool IsDead = false;

    void Start()
    {
		CurrentHealth = 100;
        PlayerMovement = GetComponentInParent<FPSInput>();
    }
    void Update()
    {
        if (Damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        
        Damaged = false;
    }

    public void Hurt(int damage)
    {
		CurrentHealth -= damage;

        HealthBar.text = "Health: " + CurrentHealth;

        Damaged = true;
        
        if(CurrentHealth <= 0)
        {
            if(!IsDead)
            {
                Die();
            }
        }
        else
        {
            DamageSound.Play();
        }
	}

    private void Die()
    {
        PlayerMovement.AllowMovement = false;
        damageImage.color = flashColour;

        if(DeathSound != null)
        {
            DeathSound.Play();
        }
    }
}
