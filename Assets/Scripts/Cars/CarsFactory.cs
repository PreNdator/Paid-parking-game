
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CarsFactory : MonoBehaviour
{
    [SerializeField]
    private List<Car> _carPrefabs;

    private DiContainer _container;

    private readonly List<Car> _createdCars = new();

    [Inject]
    public void Construct(DiContainer container)
    {
        _container = container;
    }

    public Car CreateCar(Car carPrefab, Vector3 position)
    {
        var car = _container.InstantiatePrefabForComponent<Car>(
            carPrefab, position, Quaternion.identity, null);

        car.PathCompleted += OnCarPathCompleted;
        _createdCars.Add(car);

        return car;
    }

    public Car CreateRandomCar(Vector3 position)
    {
        if (_carPrefabs == null || _carPrefabs.Count == 0)
        {
            return null;
        }

        int index = Random.Range(0, _carPrefabs.Count);
        return CreateCar(_carPrefabs[index], position);
    }

    public void DestroyCar(Car car)
    {
        if (!_createdCars.Contains(car)) return;
        _createdCars.Remove(car);
        if (car != null)
        {
            car.PathCompleted -= OnCarPathCompleted;
            Destroy(car.gameObject);
        }
    }

    private void OnCarPathCompleted(Car car)
    {
        DestroyCar(car);
    }
}