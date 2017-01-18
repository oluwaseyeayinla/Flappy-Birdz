using UnityEngine;
using System.Collections;

public class ColumnSpawnner : MonoBehaviour {

    public Transform poolContainer;
    public GameObject columnPrefab;
    public int poolSize = 5;
    public float spawnRate = 4;
    public float columnMin = -1f;
    public float columnMax = 3.5f;

    private GameObject[] columns;
    private Vector2 poolPosition = new Vector2(-8f, 0f); // an arbitrary position off screen or camera
    private int currentColumn;
    private float timeSinceLastSpawned;
    private float spawnXPosition = 8f;
    
	// Use this for initialization
	void Start ()
    {
        columns = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            columns[i] = (GameObject) Instantiate(columnPrefab, poolPosition, Quaternion.identity);
            columns[i].transform.SetParent(poolContainer, false);
        }
	}

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;

        if (!SceneController.Instance.IsGameOver && timeSinceLastSpawned >= spawnRate)
        {
            timeSinceLastSpawned = 0;
            float spawnYPosition = Random.Range(columnMin, columnMax);
            columns[currentColumn].transform.position = new Vector2(spawnXPosition, spawnYPosition);
            currentColumn++;

            if (currentColumn >= poolSize)
            {
                currentColumn = 0;
            }
        }
	}
}
