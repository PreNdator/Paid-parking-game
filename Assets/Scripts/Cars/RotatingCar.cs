using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class RotatingCar : OffenderCar
{
    [SerializeField]
    private float _violationRotationSpeed = 1000f;
    protected override async UniTask PerformWhileViolating(CancellationToken token, float violationDuration)
    {
        float elapsedTime = 0f;

        while (!token.IsCancellationRequested && elapsedTime < violationDuration)
        {
            transform.Rotate(Vector3.up, _violationRotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }
}
