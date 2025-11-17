using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform Waypoint1;
    public Transform Waypoint2;

    [Header("Configuració de moviment")]
    public float duration = 2f;
    public Ease easeType = Ease.InOutSine;
    public float startDelay = 0f; //Delay abans de començar
    public bool startOnAwake = true; 

    private Tweener moveTween;

    private void Start()
    {
        if (startOnAwake)
            StartMovement();
    }

    public void StartMovement()
    {
        transform.position = Waypoint1.position;

        moveTween = transform.DOMove(Waypoint2.position, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(startDelay); //Delay
    }

    public void StopMovement()
    {
        if (moveTween != null && moveTween.IsActive())
            moveTween.Kill();
    }

    private void OnDestroy()
    {
        StopMovement();
    }
}