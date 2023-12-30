using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZenjectInstallers
{
    public class UnityMonoInstaller : MonoInstaller<UnityMonoInstaller>
    {
        [SerializeReference] private List<IUnityInstaller> _installers = new List<IUnityInstaller>();

        public override void InstallBindings()
        {
            _installers.ForEach(installer => installer?.InstallBindings(Container));
        }
    }
}