using UnityEngine;
using Zenject;

public class CameraInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.Bind<IPhotoAnalyzer>().To<PhotoAnalyzer>().FromNew().AsSingle();
    }
}
