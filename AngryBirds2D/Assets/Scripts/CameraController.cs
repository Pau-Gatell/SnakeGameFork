using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform pivot; //Posició on ha de tornar la càmara després de llençar un bird
    public float HorizontalOffset = 2f;
    private Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Transform target = SlingshotController.instance.GetCurrentTarget(); //Ocell actual

        if (target.position.x > _camera.transform.position.x)
        {
            _camera.transform.position = new Vector3(target.position.x, _camera.transform.position.y, _camera.transform.position.z);
        }
    }

    public void ResetCamera()
    {
        //Torna a la posició inicial del pivot
        transform.position = pivot.transform.position;
    }
}