using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public static AmmoController instance;

    [Header("Prefab dels ocells")]
    public BirdController birdPrefab;

    [Header("Nombre d'ocells")]
    public int maxAmmoCount = 4;

    [Header("Fila al terra")]
    public float queueOffset = 0.7f;     // Separació entre ocells
    public Transform queueStartPoint;    // Punt al terra on comença la fila

    private List<BirdController> birdQueue = new List<BirdController>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Crear tots els ocells a la fila del terra
        for (int i = 0; i < maxAmmoCount; i++)
        {
            BirdController newBird = Instantiate(birdPrefab);

            // Posar a la cua del terra
            Vector3 queuePos = queueStartPoint.position + Vector3.right * (i * queueOffset);
            newBird.transform.position = queuePos;

            // Convertim el Rigidbody a Kinematic per evitar col·lisions
            newBird.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            birdQueue.Add(newBird);
        }
    }

    // Agafa el primer ocell de la cua i el treu de la llista
    public BirdController GetNextBird()
    {
        if (birdQueue.Count == 0)
        {
            return null; // Game Over
        }

        BirdController nextBird = birdQueue[0];
        birdQueue.RemoveAt(0);

        return nextBird;
    }

    // Reorganitza la cua al terra
    public void UpdateQueuePositions()
    {
        for (int i = 0; i < birdQueue.Count; i++)
        {
            Vector3 newPos = queueStartPoint.position + Vector3.right * (i * queueOffset);
            birdQueue[i].transform.position = newPos;
        }
    }
}