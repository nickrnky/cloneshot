using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts;

public class SceneController : MonoBehaviour
{
	[SerializeField]
    private GameObject defaultEnemyPrefab;
    [SerializeField]
    private GameObject bombermanPrefab;
    [SerializeField]
    private GameObject spawnerPrefab;
    
    private GameObject _player;

    private GameObject spawner1, spawner2, spawner3, spawner4;

    public Text SpawnerCount;

    int x = 0;
    bool ShouldRespawn = true;
    int RespawnTimer = 150;

    int SpawnerToUse = 1;

    void Start()
    {
        spawner1 = Instantiate(spawnerPrefab) as GameObject;
        spawner1.transform.position = new Vector3(45, 0, -42);

        spawner2 = Instantiate(spawnerPrefab) as GameObject;
        spawner2.transform.position = new Vector3(45, 0, 42);

        spawner3 = Instantiate(spawnerPrefab) as GameObject;
        spawner3.transform.position = new Vector3(-45, 0, 42);

        spawner4 = Instantiate(spawnerPrefab) as GameObject;
        spawner4.transform.position = new Vector3(-45, 0, -42);
    }

    void Update()
    {
        x++;

        if (x > 300)
        {
            x = 0;

            int Count = 0;
            if(spawner1 != null)
            {
                EnemyCounter.AddEnemy();
                CreateMonster(spawner1.transform.position.x, spawner1.transform.position.y, spawner1.transform.position.z);
                Count++;
            }
            if (spawner2 != null)
            {
                EnemyCounter.AddEnemy();
                CreateMonster(spawner2.transform.position.x, spawner2.transform.position.y, spawner2.transform.position.z);
                Count++;
            }
            if (spawner3 != null)
            {
                EnemyCounter.AddEnemy();
                CreateMonster(spawner3.transform.position.x, spawner3.transform.position.y, spawner3.transform.position.z);
                Count++;
            }
            if (spawner4 != null)
            {
                EnemyCounter.AddEnemy();
                CreateMonster(spawner4.transform.position.x, spawner4.transform.position.y, spawner4.transform.position.z);
                Count++;
            }

            SpawnerCount.text = "Spawner Count: " + Count;
        }
	}

    private void CreateMonster(float x, float y, float z)
    {
        switch(Random.Range(0,2))
        {
            case 0:
                CreateDefaultMonster(x, y, z);
                break;
            case 1:
                CreateBomberman(x, y, z);
                break;
        }
    }

    private void CreateBomberman(float x, float y, float z)
    {
        GameObject _bomberman;
        _bomberman = Instantiate(bombermanPrefab) as GameObject;

        _bomberman.transform.position = new Vector3(x, y, z);

        float angle = Random.Range(0, 360);
        _bomberman.transform.Rotate(0, angle, 0);
    }

    private void CreateDefaultMonster(float x, float y, float z)
    {
        GameObject _enemy;
        _enemy = Instantiate(defaultEnemyPrefab) as GameObject;

        _enemy.transform.position = new Vector3(x, y, z);

        float angle = Random.Range(0, 360);
        _enemy.transform.Rotate(0, angle, 0);
    }
}
