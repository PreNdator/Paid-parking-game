using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    [SerializeField] private InputModeManager _inputModeManager;

    public override void InstallBindings()
    {
        Container.Bind<PlayerInput>().AsSingle();

        Container.Bind<CursorManager>().AsSingle();

        Container.Bind<InputModeManager>().FromInstance(_inputModeManager).AsSingle();
    }
}