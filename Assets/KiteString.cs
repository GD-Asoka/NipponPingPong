using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KiteString : MonoBehaviour
{
    public Transform lockedEnd, kiteEnd; // The Transform of the locked end of the string
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Set the number of positions for the LineRenderer
    }

    void Update()
    {
        // Update the positions of the LineRenderer
        lineRenderer.SetPosition(0, kiteEnd.position); // Start position is the kite position
        lineRenderer.SetPosition(1, lockedEnd.position); // End position is the locked end position
    }
}
