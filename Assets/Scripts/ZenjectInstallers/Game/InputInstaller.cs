using System;
using Game.Services.Input;
using Zenject;

namespace ZenjectInstallers.Game
{
    [Serializable]
    public class InputInstaller : IUnityInstaller
    {
        public void InstallBindings(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<InputService>().AsSingle().NonLazy();
        }
    }
}