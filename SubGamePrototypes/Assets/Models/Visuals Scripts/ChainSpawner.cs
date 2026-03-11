using UnityEngine;

public class ChainSpawner : MonoBehaviour
{
    public ChainChild salpPrefab;
    public int chainLength = 14;
    public float minXRotation = 6f;
    public float maxXRotation = 22f;

    void Start()
    {
        BuildChain();
    }

    void BuildChain()
    {
        if (salpPrefab == null) return;

        ChainChild previous = null;

        for (int i = 0; i < chainLength; i++)
        {
            ChainChild salp = Instantiate(salpPrefab, transform.position, transform.rotation);

            if (previous != null)
            {
                salp.transform.rotation = previous.chainOut.rotation;
                salp.transform.Rotate(Random.Range(minXRotation, maxXRotation), 0f, 0f, Space.Self);

                Vector3 offset = previous.chainOut.position - salp.chainIn.position;
                salp.transform.position += offset;
            }

            previous = salp;
        }
    }
}