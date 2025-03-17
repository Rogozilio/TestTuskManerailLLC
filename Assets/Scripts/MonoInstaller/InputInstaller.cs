using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Input>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}
