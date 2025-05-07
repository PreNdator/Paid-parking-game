using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public abstract class Car : MonoBehaviour
{
    private IPathProvider _pathProvider;

    [Header("Obstacle avoid")]
    [SerializeField] private float _detectDistance = 2f;
    [SerializeField] private float _detectRadius = 0.5f;

    [Header("Car power")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _actionChance = 0.3f;
    [SerializeField] private float _rotationSpeed = 5f;

    public event System.Action<Car> PathCompleted;
    [Header("Car parts")]
    [SerializeField]
    private Headlights _headlights;

    public float Speed => _speed;
    public float RotationSpeed => _rotationSpeed;
    public Headlights Headlights => _headlights; 

    [Inject]
    private void Construct(IPathProvider pathProvider)
    {
        _pathProvider = pathProvider;
    }

    protected virtual void Start()
    {
        if (_pathProvider == null)
            return;

        Drive(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTask Drive(System.Threading.CancellationToken token)
    {
        try
        {
            foreach (Vector3 target in _pathProvider.GetPoints())
            {
                await MoveToPoint(target, token);
                
                if (Random.value < _actionChance)
                {
                    _actionChance = 0;
                    await DoSpecialAction();
                    await ReturnToPath(target, token);
                }
            }

            OnPathCompleted();
        }
        catch (System.OperationCanceledException)
        {
            //its ok
        }
    }

    public async UniTask MoveToPoint(Vector3 target, System.Threading.CancellationToken token)
    {
        while (!token.IsCancellationRequested &&Vector3.Distance(transform.position, target) > 0.1f)
        {
            if (IsCarInFront())
            {
                await UniTask.Yield(PlayerLoopTiming.Update, token);
                continue;
            }

            Vector3 direction = (target - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }

            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }

    

    private async UniTask ReturnToPath(Vector3 pathPoint, System.Threading.CancellationToken token)
    {
        await MoveToPoint(pathPoint, token);
    }

    private bool IsCarInFront()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 direction = transform.forward;

        if (Physics.SphereCast(origin, _detectRadius, direction, out RaycastHit hit, _detectDistance))
        {
            var otherCar = hit.collider.GetComponent<CarObstacle>();
            if (otherCar != null)
                return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * _detectDistance;
        center.y += 0.5f;
        Gizmos.DrawWireSphere(center, _detectRadius);
    }

    protected abstract UniTask DoSpecialAction();

    protected virtual void OnPathCompleted()
    {
        PathCompleted?.Invoke(this);
    }
}