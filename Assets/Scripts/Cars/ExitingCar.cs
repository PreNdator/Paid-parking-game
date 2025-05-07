using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ExitingCar : OffenderCar
{
    [SerializeField]
    List<Human> _humanPrefabs;
    [SerializeField]
    Transform _humanSpawnPoint;

    GameObject _human;

    protected override async UniTask PerformWhileViolating(CancellationToken token, float violationDuration)
    {
        if (_humanPrefabs != null && _humanPrefabs.Count > 0 && _humanSpawnPoint != null)
        {
            var randomIndex = UnityEngine.Random.Range(0, _humanPrefabs.Count);
            var selectedPrefab = _humanPrefabs[randomIndex];
            if (selectedPrefab != null)
            {
                _human = Instantiate(selectedPrefab.gameObject, _humanSpawnPoint.position, Quaternion.identity);

                Human humanComponent = _human.GetComponent<Human>();
                humanComponent?.OnSpawn(violationDuration);
            }
        }
        try
        {
            await UniTask.WaitForSeconds(violationDuration, false, PlayerLoopTiming.Update, token);
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning("Operation Cancelled.");
        }
        finally
        {
            if (_human != null)
            {
                Destroy(_human);
                _human = null;
            }
        }
    }
}
