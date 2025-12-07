using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform pivot;
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
        Transform target = SlingshotController.instance.GetCurrentTarget();

        if (target.position.x > _camera.transform.position.x)
        {
            //Debug.Log("La posicion horizontal del pajaro es mayor que la de la camara");
            _camera.transform.position = new Vector3(target.position.x, _camera.transform.position.y, _camera.transform.position.z);
        }
    }

    public void ResetCamera()
    {
        // Bring the camera to its original position
        transform.position = pivot.transform.position;
    }
}