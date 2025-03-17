using Zenject;

public class WebRequestInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<WebRequestSystem>().AsSingle().NonLazy();
    }
}
