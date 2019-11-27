using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

	public static int EnemiesAlive = 0;

    public GameObject cat = CAT;
	public Wave[] waves;
    public GameObject[] ene= { CAT, DUCK, MOLE, PENGUIN, SHEEP };

	public Transform spawnPoint;

	public float timeBetweenWaves = 0f;
	private float countdown = 0f;

	public Text waveCountdownText;

	public GameManager gameManager;

	private int waveIndex = 0;

	void Update ()
	{
		if (waveIndex == waves.Length && EnemiesAlive == 0)
		{
			gameManager.WinLevel();
			this.enabled = false;
		}

		if (EnemiesAlive == 0 && countdown <= 0f)
		{
			StartCoroutine(SpawnWave());
			countdown = timeBetweenWaves;
			return;
		} else if(EnemiesAlive == 0) {
			countdown -= Time.deltaTime;
		}

		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

		waveCountdownText.text = string.Format("{0:00.00}", countdown);
	}

	IEnumerator SpawnWave ()
	{
		PlayerStats.Rounds++;
		Wave wave = waves[waveIndex];
        if (PlayerStats.Rounds == 6)
        {
            for (i = 0; i < 5; i++)
                ene[i].Enemy.health += 10000;
        }
		EnemiesAlive = wave.count;

		for (int i = 0; i < wave.count; i++)
		{
			SpawnEnemy(wave.enemy);
			yield return new WaitForSeconds(1f / wave.rate);
		}

		waveIndex++;
	}

	void SpawnEnemy (GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}

}
