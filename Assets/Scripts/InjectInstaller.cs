using UnityEngine;
using Zenject;

public class InjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameDataController>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<InputController>().AsSingle().NonLazy();


        Container.Bind<PanelManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<DarkPanel>().FromComponentInHierarchy().AsSingle().NonLazy();
    }


}