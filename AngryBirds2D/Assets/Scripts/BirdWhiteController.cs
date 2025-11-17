using System;
using System.Collections;
using UnityEngine;

public class BirdWhiteController : BirdController
{
    public Transform eggPrefab;
    public Vector3 eggOffset;

    private bool _used;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();

        _used = false;
    }

    private void Update()
    {
        if (isActive)
        {
            DetectAlive();
            DrawTrace();

            if(!_used && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Shooting the egg");
                
                Vector3 eggPosition = transform.position + eggOffset;
                Transform egg = Instantiate(eggPrefab, eggPosition, Quaternion.identity);
                CircleCollider2D collider = egg.GetComponent<CircleCollider2D>();
                
                SlingshotController.instance.StartCoroutine(ManageEggCollision(collider));
                SlingshotController.instance.SetCurrentTarget(egg);

                _used = true;
            }
        }
    }

    public IEnumerator ManageEggCollision(CircleCollider2D col)
    {
        col.enabled = false;
        
        yield return new WaitForSecondsRealtime(0.5f);

        col.enabled = true;
    }
}
