using UnityEngine;

public class Chunk : MonoBehaviour
{
    public float chunkLength;

    public Chunk ShowChunk()
    {
        gameObject.SetActive(true);
        return this;
    }

    public Chunk HideChunk()
    {
        gameObject.SetActive(false);
        return this;
    }

}
