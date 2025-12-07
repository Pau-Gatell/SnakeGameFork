using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public static AmmoController instance;

    [Header("Ammo Settings")]
    public Transform[] birdPrefabs;     //Prefabs dels ocells
    public int maxAmmoCount = 3;        //Número d'ocells que vols crear
    public float offset = 0.2f;         //Espai entre els ocells de terra

    private List<BirdController> _birds = new List<BirdController>();

    private void Awake()
    {
        instance = this;
        PopulateBirds();
    }

    
    // Crea tota la cua d'ocells abans que el joc comenci.
    private void PopulateBirds()
    {
        if (birdPrefabs == null || birdPrefabs.Length == 0)
        {
            Debug.LogError("❌ No hi ha ocells assignats a 'birdPrefabs'!");
            return;
        }

        float size = birdPrefabs[0].GetComponent<CircleCollider2D>().radius * 2f + offset;

        for (int i = 0; i < maxAmmoCount; i++)
        {
            int index = i % birdPrefabs.Length;
            Transform prefab = birdPrefabs[index];

            Transform birdObj = Instantiate(
                prefab,
                transform.position + Vector3.left * i * size,
                Quaternion.identity
            );

            
            Rigidbody2D rb = birdObj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Kinematic;

            //Referència al controller del bird
            BirdController bird = birdObj.GetComponent<BirdController>();
            if (bird != null)
            {
                bird.Initialize();
                _birds.Add(bird);
            }
            else
            {
                Debug.LogError("❌ El prefab " + prefab.name + " no té BirdController!");
            }
        }
    }

    public BirdController Reload()
    {
        if (_birds.Count == 0)
        {
            return null; //No queden més birds
        }

        BirdController bird = _birds[0];
        _birds.RemoveAt(0);
        return bird;
    }
}