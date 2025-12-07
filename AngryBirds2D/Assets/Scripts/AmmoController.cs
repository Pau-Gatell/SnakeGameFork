using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public static AmmoController instance;

    [Header("Prefabs dels ocells")]
    public Transform[] birdPrefabs;   // Llista de diferents ocells (Red, Bomb, etc.)

    [Header("Config")]
    public int maxAmmoCount = 3;      // Nombre total d’ocells a generar
    public float offset = 0.2f;       // Separació entre ocells a la cua

    private List<BirdController> _birds = new List<BirdController>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (birdPrefabs.Length == 0)
        {
            Debug.LogError("No hi ha ocells assignats a 'birdPrefabs'!");
            return;
        }

        // Agafem el collider del primer prefab per calcular la mida
        float size = birdPrefabs[0].GetComponent<CircleCollider2D>().radius * 2f + offset;

        // Generem els ocells en fila
        for (int i = 0; i < maxAmmoCount; i++)
        {
            // Determina quin ocell es crearà (ex: 0,1,0,1…)
            int index = i % birdPrefabs.Length;

            Transform birdObj = Instantiate(
                birdPrefabs[index],
                transform.position + Vector3.left * i * size,
                Quaternion.identity
            );

            // Com els Angry Birds originals: quiets fins que toca llançar-los
            Rigidbody2D rb = birdObj.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;

            BirdController bird = birdObj.GetComponent<BirdController>();
            _birds.Add(bird);
        }
    }

    // Retorna el següent ocell de la cua
    public BirdController Reload()
    {
        if (_birds.Count == 0)
        {
            Debug.Log("No queden més ocells!");
            return null;
        }

        BirdController nextBird = _birds[0];
        _birds.RemoveAt(0);

        return nextBird;
    }
}