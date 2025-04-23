using UnityEngine;
using Zenject;

public class CarsInstaller : MonoInstaller
{
    [SerializeField]
    private RoadPath _path;
    [SerializeField]
    private CarsFactory _cars;

    public override void InstallBindings()
    {
        Container.Bind<IPathProvider>().FromInstance(_path).AsSingle();
        Container.Bind<CarsFactory>().FromInstance(_cars).AsSingle();
    }
}