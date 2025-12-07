using System.Collections;
using UnityEngine;

public class BirdWhiteController : BirdController
{
    [Header("White Bird")]
    public Transform eggPrefab;
    public Vector3 eggOffset;

    private bool usedAbility = false;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (!isActive) return;

        DrawTrace();
        DetectAlive();

     
        if (!usedAbility && Input.GetKeyDown(KeyCode.Space))
        {
            DropEgg();
            usedAbility = true;
        }
    }

    private void DropEgg()
    {
        Vector3 pos = transform.position + eggOffset;
        Transform egg = Instantiate(eggPrefab, pos, Quaternion.identity);

        //Espera 0.5s perquè no col·lisioni amb l’ocell
        CircleCollider2D col = egg.GetComponent<CircleCollider2D>();
        StartCoroutine(EnableCollider(col));

        //La càmera passa a seguir l’ou
        SlingshotController.instance.SetCurrentTarget(egg);
    }

    IEnumerator EnableCollider(CircleCollider2D col)
    {
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
    }
}
