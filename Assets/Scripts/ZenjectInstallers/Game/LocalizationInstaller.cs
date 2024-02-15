using System;
using Game.Core.Localization;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers.Game
{
    [Serializable]
    public class LocalizationInstaller : IUnityInstaller
    {
        [SerializeField] private LocalizationConfig _config;
        
        public void InstallBindings(DiContainer container)
        {
            container.BindInstance(_config);
            container.BindInterfacesAndSelfTo<LocalizationService>().AsSingle().NonLazy();
        }
    }
}