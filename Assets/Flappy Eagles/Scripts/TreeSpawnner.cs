using UnityEngine;
using System.Collections;

public class TreeSpawnner : MonoBehaviour
{
    [SerializeField] Transform poolContainer;
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject treePrefab;
    [SerializeField] int poolSize = 5;
    [SerializeField] float spawnRate = 4;

    public float TimeSinceLastSpawn
    {
        get { return timeSinceLastSpawned; }
    }

    public float SpawnRate
    {
        get { return spawnRate; }
    }

    private GameObject[] trees;
    private Vector2 defaultLocation = new Vector2(-8f, 0);
    private int currentTree;
    private float timeSinceLastSpawned;
    private float minTreeY = -1f;
    private float maxTreeY = 3.5f;
    private float treeY;

    void Awake ()
    {
        trees = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            trees[i] = (GameObject) Instantiate(treePrefab, defaultLocation, Quaternion.identity);
            trees[i].transform.SetParent(poolContainer, false);
        }
	}

    public void SpawnNewTree(TreeType type)
    {
        trees[currentTree].GetComponent<TreeController>().Reset();

        switch (type)
        {
            case TreeType.Easy:
            case TreeType.Normal:
                {
                    minTreeY = -3.5f;
                    maxTreeY = 3.5f;
                    trees[currentTree].GetComponent<TreeController>().SetTreeMovement(type);
                }
                break;

            case TreeType.MeduimUp:
                {
                    minTreeY = -2f;
                    maxTreeY = 2f;
                    trees[currentTree].GetComponent<TreeController>().SetTreeMovement(TreeType.MeduimUp);
                }
                break;
            case TreeType.MediumDown:
                {
                    minTreeY = -2f;
                    maxTreeY = 2f;
                    trees[currentTree].GetComponent<TreeController>().SetTreeMovement(TreeType.MediumDown);
                }
                break;
            case TreeType.Hard:
                {
                    minTreeY = -1f;
                    maxTreeY = 1f;
                    trees[currentTree].GetComponent<TreeController>().SetTreeMovement(TreeType.Hard);
                }
                break;
        }

        timeSinceLastSpawned = 0;
        treeY = Random.Range(minTreeY, maxTreeY);
        //Debug.Log("Y Position: " + treeY);
        trees[currentTree].transform.position = new Vector2(spawnPosition.position.x, treeY);

        currentTree++;
        if (currentTree >= poolSize)
        {
            currentTree = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawned += Time.deltaTime;
	}
}
