using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class KiteStringWithSlack : MonoBehaviour
{
    public Transform kite; // Reference to the kite GameObject
    public Transform lockedEnd; // Reference to the locked end GameObject
    public int numSlackNodes = 5; // Number of slack nodes to create
    public float slackLength = 0.2f; // Length of each slack segment

    private LineRenderer lineRenderer;
    private Vector3[] slackNodePositions;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2 + numSlackNodes; // Set the number of positions for the LineRenderer

        // Initialize slack node positions array
        slackNodePositions = new Vector3[numSlackNodes];

        // Initialize slack nodes along the path of the kite string
        for (int i = 0; i < numSlackNodes; i++)
        {
            slackNodePositions[i] = transform.position + (lockedEnd.position - transform.position).normalized * slackLength * ((i + 1) / (float)(numSlackNodes + 1));
        }
    }

    void Update()
    {
        // Update positions of slack nodes based on kite and locked end positions
        for (int i = 0; i < numSlackNodes; i++)
        {
            slackNodePositions[i] = transform.position + (lockedEnd.position - transform.position).normalized * slackLength * ((i + 1) / (float)(numSlackNodes + 1));
        }

        // Set positions of LineRenderer
        lineRenderer.SetPosition(0, kite.transform.position); // Start position is the kite position
        for (int i = 0; i < numSlackNodes; i++)
        {
            lineRenderer.SetPosition(i + 1, slackNodePositions[i]); // Slack node positions
        }
        lineRenderer.SetPosition(numSlackNodes + 1, lockedEnd.position); // End position is the locked end position
    }
}
