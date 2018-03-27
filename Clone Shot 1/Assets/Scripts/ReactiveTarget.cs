using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class ReactiveTarget : MonoBehaviour
{

    public int Health = 1;
    private bool IsDead = false;

	public void ReactToHit(int Damage)
    {
		WanderingAI behavior = GetComponent<WanderingAI>();
		if (behavior != null)
        {
			behavior.SetAlive(false);
		}

        Health -= Damage;
        Debug.Log("Health: " + Health);

        if(Health <= 0 && !IsDead)
        {
            StartCoroutine(Die());
        }
	}

	private IEnumerator Die()
    {
        IsDead = true;
        EnemyCounter.SubtractEnemy();
        this.transform.Rotate(-75, 0, 0);

        yield return new WaitForSeconds(1.5f);

        Destroy(this.gameObject);
        
	}
}
