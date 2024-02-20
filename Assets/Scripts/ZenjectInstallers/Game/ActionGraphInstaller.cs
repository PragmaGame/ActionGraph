using System;
using Game.Core.ActionGraph.Runtime;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers.Game
{
    [Serializable]
    public class ActionGraphInstaller : IUnityInstaller
    {
        [SerializeField] private ActionGraphData _graphData;
        
        public void InstallBindings(DiContainer container)
        {
            container.BindInstance(_graphData);
            container.BindInterfacesAndSelfTo<ActionGraphReceiver>().AsSingle().NonLazy();
            container.BindInterfacesAndSelfTo<ProcessorSelector>().AsSingle().NonLazy();
            container.BindInterfacesAndSelfTo<RunnerFactory>().AsSingle().NonLazy();
            container.BindInterfacesAndSelfTo<ActionNodeSelector>().AsSingle().NonLazy();
            
            //Processors
            container.BindInterfacesTo<TextProcessor>().AsSingle().NonLazy();
            container.BindInterfacesTo<ProcessorContainer>().AsSingle().NonLazy();
        }
    }
}