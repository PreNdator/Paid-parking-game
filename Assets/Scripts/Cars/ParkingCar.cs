using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class ParkingCar : OffenderCar
{

    private ParkingPoints _parkingPoints;

    [Inject]
    private void Construct(ParkingPoints parkingPoints)
    {
        _parkingPoints = parkingPoints;
    }

    protected override async UniTask PerformWhileViolating(CancellationToken token, float violationDuration)
    {
        if (_parkingPoints.ViolationPoints == null || _parkingPoints.ViolationPoints.Count == 0)
        {
            Debug.LogWarning("No violation points assigned.");
            return;
        }

        Transform targetPoint = FindClosestPoint();
        if (targetPoint == null)
            return;

        await MoveToPoint(targetPoint.position, token);

        if (Headlights != null)
            Headlights.TurnLights(false);

        float elapsedTime = 0f;
        while (!token.IsCancellationRequested && elapsedTime < violationDuration)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        if (Headlights != null)
            Headlights.TurnLights(true);
    }

    private Transform FindClosestPoint()
    {
        Transform closestPoint = null;
        float closestDistance = Mathf.Infinity;

        foreach (var point in _parkingPoints.ViolationPoints)
        {
            float distance = Vector3.Distance(transform.position, point.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }
}