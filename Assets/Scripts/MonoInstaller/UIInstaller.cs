using Zenject;

public class UIInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UI>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
