using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    private float chunkSpawnZ;
    //Gameplay
    private Queue<Chunk> activeChunks = new Queue<Chunk>();
    private List<Chunk> chunkPool = new List<Chunk>();

    //Configurable Fields
    /// <summary>
    /// SerializeField can be configured in the inspector
    /// </summary>
    [SerializeField] private int firstChunkSpawnPosition = -10;
    [SerializeField] private int chunkOnScreen = 5;
    [SerializeField] private float despawnDistance = 5.0f;

    [SerializeField] private List<GameObject> chunkPrefab;
    [SerializeField] private Transform cameraTransform;


    //This awake function is for testing - will be deleted when the controls are set up later
    private void Awake()
    {
        ResetWorld();
    }
    private void Start()
    {
        //Check if we have an empty chunk Prefab list. 
        if(chunkPrefab.Count == 0)
        {
            Debug.LogError("No Chunk prefab found on the world generator. Please assign some chunks!");
            return;
        }

        //try to assign camera if it is not already assigned.
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
            Debug.Log("Camera transform assigned automatically to Camera.main");
        }
    }

    private void Update()
    {
        ScanPosition();
    }

    private void ScanPosition()
    {
        float cameraZ = cameraTransform.position.z;
        Chunk lastChunk = activeChunks.Peek();
        if (cameraZ >= lastChunk.transform.position.z + lastChunk.chunkLength + despawnDistance) 
        {
            SpawnNewChunk();
            DeleteLastChunk();
        }
    }

    private void SpawnNewChunk()
    {
        //Get a random index for whic prefab to spawn
        int randomIndex = Random.Range(0, chunkPrefab.Count);

        //does it exits in our current pool? if yes - use it
        Chunk chunk = chunkPool.Find(x => !x.gameObject.activeSelf && x.name == (chunkPrefab[randomIndex].name + "(Clone)"));

        //if it does not already exits in our pool, then create one.
        if(!chunk)
        {
            GameObject go = Instantiate(chunkPrefab[randomIndex], transform);
            chunk = go.GetComponent<Chunk>();
        }

        //place the object & show it
        chunk.transform.position = new Vector3(0, 0, chunkSpawnZ);
        chunkSpawnZ += chunk.chunkLength;

        //store the value to our pool for reuse
        activeChunks.Enqueue(chunk);
        chunk.ShowChunk();
    }

    private void DeleteLastChunk()
    {
        Chunk chunk = activeChunks.Dequeue(); //removes the oldest chunk from the queue and assigns it a reference (chunk) that we can then use to hide & add
        chunk.HideChunk();
        chunkPool.Add(chunk);
    }

    private void ResetWorld()
    {
        //remove everything from the pool, Reset the chunkSpawnZ, reformat the world

        chunkSpawnZ = firstChunkSpawnPosition;
        for (int i = activeChunks.Count; i != 0; i--) //working backwards, for as long as there is something in the pool
            DeleteLastChunk();

        for (int i = 0; i < chunkOnScreen; i++) // spawn a new chunk as long as there is less than the number we specified in cunkInScreen
            SpawnNewChunk();
        
    }
}
