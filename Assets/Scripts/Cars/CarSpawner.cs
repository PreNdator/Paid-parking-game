using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class CarSpawner : MonoBehaviour
{
    private CarsFactory _factory;
    private IPathProvider _pathProvider;
    private LawViolationList _violationList;

    [SerializeField]
    float _minSpawnTime;
    [SerializeField]
    float _maxSpawnTime;
    [SerializeField]
    private Transform _spawnPoint;

    private CancellationTokenSource _cts;

    [Inject]
    private void Construct(CarsFactory factory, IPathProvider pathProvider, LawViolationList violationList)
    {
        _factory = factory;
        _pathProvider = pathProvider;
        _violationList = violationList;
    }

    private void Start()
    {
        _cts = new CancellationTokenSource();
        StartSpawningLoop(_cts.Token).Forget();
    }

    private async UniTaskVoid StartSpawningLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            float delay = Random.Range(_minSpawnTime, _maxSpawnTime);
            await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);

            Car car = _factory.CreateRandomCar(_spawnPoint.position);
        }
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
