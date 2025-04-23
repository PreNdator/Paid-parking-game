using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

public abstract class OffenderCar : Car
{

    [Header("Law violations")]
    [SerializeField] private GameObject _offenderObject;
    [SerializeField] private float _violationPoints = 100f;
    [SerializeField] private float _violationDuration = 10f;

    private LawViolationList _violationList;

    [Inject]
    private void Construct(LawViolationList violationList)
    {
        _violationList = violationList;
    }

    protected override async UniTask DoSpecialAction()
    {
        if (_violationList == null)
        {
            Debug.LogWarning($"{name}: LawViolationList reference is missing.");
            return;
        }

        CancellationToken token = this.GetCancellationTokenOnDestroy();

        LawViolation violation = _violationList.AddLawViolation(
        _offenderObject,
        _violationPoints);

        UniTask behaviorTask = PerformWhileViolating(token, _violationDuration);
        await behaviorTask;
        _violationList.RemoveViolation(violation);

    }

    protected abstract UniTask PerformWhileViolating(CancellationToken token, float violationDuration);
}