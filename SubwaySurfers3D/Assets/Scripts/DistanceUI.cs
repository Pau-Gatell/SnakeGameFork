using UnityEngine;
using TMPro;

public class DistanceUI : MonoBehaviour
{
    public Transform player;
    public TMP_Text distanceText;

    private float startZ;

    void Start()
    {
        startZ = player.position.z;
    }

    void Update()
    {
        float distance = player.position.z - startZ;
        distanceText.text = "Distance: " + Mathf.FloorToInt(distance) + " m";
    }
}