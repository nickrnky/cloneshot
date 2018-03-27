using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WanderingAI : MonoBehaviour
{
    public enum TypeOfMonster { Default, Bomberman }

    public TypeOfMonster MonsterType = TypeOfMonster.Default;
	public float speed = 6.0f;
	public float obstacleRange = 5.0f;

    private AudioSource SightSound;
	
	[SerializeField] private GameObject fireballPrefab;
	private GameObject _fireball;
	
	private bool _alive;
	
	void Start()
    {
		_alive = true;
        switch(MonsterType)
        {
            case TypeOfMonster.Default:
                speed = 3.0f;
                obstacleRange = 5.0f;
                break;
            case TypeOfMonster.Bomberman:
                speed = 6.0f;
                obstacleRange = 6.0f;
                    
                break;
        }


        SightSound = GetComponent<AudioSource>();
    }
	
	void Update()
    {
		if (_alive)
        {
			transform.Translate(0, 0, speed * Time.deltaTime);
			
			Ray ray = new Ray(transform.position, transform.forward);
			RaycastHit hit;
			if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                if (MonsterType == TypeOfMonster.Default)
                {
                    DefaultMonsterObjectInLOS(ray, hit);
                }
                else if (MonsterType == TypeOfMonster.Bomberman)
                {
                    BombermanObjectInLOS(ray, hit);
                }
			}
		}
	}

    private void DefaultMonsterObjectInLOS(Ray ray, RaycastHit hit)
    {
        GameObject hitObject = hit.transform.gameObject;
        if (hitObject.GetComponent<PlayerCharacter>())
        {
            if (_fireball == null)
            {
                _fireball = Instantiate(fireballPrefab) as GameObject;
                _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                _fireball.transform.rotation = transform.rotation;
            }

            if(SightSound != null)
            {
                SightSound.Play();
            }
        }
        else if (hit.distance < obstacleRange)
        {
            float angle = Random.Range(-110, 110);
            transform.Rotate(0, angle, 0);
        }
    }

    private void BombermanObjectInLOS(Ray ray, RaycastHit hit)
    {
        GameObject hitObject = hit.transform.gameObject;
        if (hitObject.GetComponent<PlayerCharacter>())
        {
            speed = 15.0f;
            if(SightSound != null)
            {
                SightSound.Play();
            }
        }
        else if (hit.distance < obstacleRange)
        {
            speed = 6.0f;
            float angle = Random.Range(-110, 110);
            transform.Rotate(0, angle, 0);
        }
        else
        {
            speed = 6.0f;
        }
    }

	public void SetAlive(bool alive)
    {
		_alive = alive;
	}
    
    void OnCollisionEnter(Collision col)
    {
        if(MonsterType == TypeOfMonster.Bomberman)
        {
            PlayerCharacter player = col.gameObject.GetComponent<PlayerCharacter>();
            if (player != null)
            {
                player.Hurt(10);
                Destroy(this.gameObject);
            }

        }
    }
}
