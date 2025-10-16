using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = .5f;
    public float startZ;
    public float smoothTime = .3f;
    private float maxOffset = 0.8f;

    private UnityEngine.Vector2 startPosition;
    private UnityEngine.Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
        startZ = transform.position.z;
    }

    private void Update()
    {
        UnityEngine.Vector2 mouseOffset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        UnityEngine.Vector2 target2D = startPosition + (mouseOffset * offsetMultiplier);

        UnityEngine.Vector2 maxCameraPosition = startPosition + UnityEngine.Vector2.ClampMagnitude(target2D - startPosition, maxOffset);

        UnityEngine.Vector3 targetCameraPosition = new UnityEngine.Vector3(maxCameraPosition.x, maxCameraPosition.y, startZ);
        transform.position = UnityEngine.Vector3.SmoothDamp(transform.position, targetCameraPosition, ref velocity, smoothTime);
    }
}
