using Zenject;

public class RaycasterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Raycaster>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
