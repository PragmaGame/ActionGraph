using System;
using Game.Core.Input;
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