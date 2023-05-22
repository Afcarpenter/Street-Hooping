using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField, Min(3)]
    private int lineSegments = 60;

    [SerializeField, Min(1)]
    private float timeOfFlight = 3;

    public GameObject collisionMarker;

    public LayerMask layerMask;

    private bool collisionIsDetected = false;

    private void Start()
    {
        lineRenderer.enabled = false;
        collisionMarker.gameObject.SetActive(false);
    }

    public void ShowTrajectoryLine(Vector3 startPoint, Vector3 startVelocity)
    {
        lineRenderer.enabled = true;
        float timeStep = timeOfFlight / lineSegments;

        Vector3[] lineRendererPoints = CalculateTrajectoryLine(startPoint, startVelocity, timeStep);

        lineRenderer.positionCount = lineSegments;
        lineRenderer.SetPositions(lineRendererPoints);
    }

    private Vector3[] CalculateTrajectoryLine(Vector3 startPoint, Vector3 startVelocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[lineSegments];

        lineRendererPoints[0] = startPoint;

        for (int i = 1; i < lineSegments; i++)
        {
            float timeOffset = timeStep * i;

            Vector3 progressBeforeGravity = startVelocity * timeOffset;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * timeOffset * timeOffset;
            Vector3 newPosition = startPoint + progressBeforeGravity - gravityOffset;
            lineRendererPoints[i] = newPosition;
                        
            if(Physics.CheckSphere(newPosition, .15f, layerMask) && !collisionIsDetected)
            {
                ShowCollisionMarker(newPosition);
            } else if (collisionIsDetected)
            {
                lineRendererPoints[i] = collisionMarker.transform.position;
            }
            /*
            if(newPosition.z >= .5f && !collisionIsDetected)
            {
                ShowCollisionMarker(newPosition);
            } else if (collisionIsDetected)
            {
                lineRendererPoints[i] = collisionMarker.transform.position;
            }*/
        }

        collisionIsDetected = false;
        return lineRendererPoints;
    }

    public void HideTrajectoryLine()
    {
        lineRenderer.enabled = false;
        collisionMarker.gameObject.SetActive(false);
    }

    private void ShowCollisionMarker(Vector3 makerPosition)
    {
        collisionMarker.gameObject.SetActive(true);
        collisionMarker.transform.position = makerPosition;
        collisionIsDetected = true;
    }
}
