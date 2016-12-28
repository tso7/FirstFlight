using UnityEngine;
using System.Collections;

public enum Direction
{
    Horizontal,
    Vertical,
    Diagonal
}

public enum BirdType
{
    Regular,
    Fast,
    Big
}

public class SpawnController : GenericSingleton<SpawnController>
{
    // Bird models
    public GameObject _regularBird, _fastBird, _bigBird;

    // Other variables
    public Vector3 spawnValuesVer, spawnValuesHor;
    public float spawnWaitVer, spawnWaitHor, spawnWaitDia;
    public SoundManager soundManager;

    public void SpawnBird (BirdType _type, Direction _dir, int _count)
    {
        GameObject _BirdModel = _regularBird;

        switch (_type)
        {
            case BirdType.Regular:
                _BirdModel = _regularBird;
                break;
            case BirdType.Fast:
                _BirdModel = _fastBird;
                break;
            case BirdType.Big:
                _BirdModel = _bigBird;
                break;
        }

        switch (_dir)
        {
            case Direction.Horizontal:
                StartCoroutine(SpawnWavesHorizontal(_BirdModel, _count));
                break;
            case Direction.Vertical:
                StartCoroutine(SpawnWavesVertical(_BirdModel, _count));
                break;
            case Direction.Diagonal:
                StartCoroutine(SpawnWavesDiagonal(_BirdModel, _count));
                break;
        }
    }

    // Method to instantiate appropriate bird
    void BirdSpawn(GameObject bird, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        Instantiate(bird, spawnPosition, spawnRotation);
        soundManager.playBirdSpawnSound();
    }

    // Spawn Vertical
    IEnumerator SpawnWavesVertical(GameObject _bird, int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValuesVer.x, spawnValuesVer.x), spawnValuesVer.y, spawnValuesVer.z);
            Quaternion spawnRotation = Quaternion.identity;
            BirdSpawn(_bird, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(spawnWaitVer);
        }
    }

    // Spawn Horizontal
    IEnumerator SpawnWavesHorizontal(GameObject _bird, int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            // Randomize spawn from left or right side of screen
            int[] randomSpawnX = new int[] { (int)spawnValuesHor.x, (int)-spawnValuesHor.x };
            Vector3 spawnPosition = new Vector3(randomSpawnX[Random.Range(0, 2)], spawnValuesHor.y, Random.Range(0, 4));

            Quaternion spawnRotation = Quaternion.identity;

            if (spawnPosition.x > 0)
                spawnRotation.eulerAngles = new Vector3(0, 90f, 0);
            else
                spawnRotation.eulerAngles = new Vector3(0, -90f, 0);

            BirdSpawn(_bird, spawnPosition, spawnRotation);

            yield return new WaitForSeconds(spawnWaitHor);
        }
    }

    // Spawn Diagonal
    IEnumerator SpawnWavesDiagonal(GameObject _bird, int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            int[] randomSpawnX = new int[] { -7, 7 };
            Vector3 spawnPosition = new Vector3(randomSpawnX[Random.Range(0, 2)], spawnValuesVer.y, spawnValuesVer.z);
            Vector3 _direction = spawnPosition - GameObject.FindGameObjectWithTag("Airplane").transform.position;

            Quaternion spawnRotation = Quaternion.LookRotation(_direction) ;

            BirdSpawn(_bird, spawnPosition, spawnRotation);

            yield return new WaitForSeconds(spawnWaitDia);
         }
    }

}
