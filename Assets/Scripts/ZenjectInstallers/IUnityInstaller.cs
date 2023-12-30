using Zenject;

namespace ZenjectInstallers
{
    public interface IUnityInstaller
    {
        void InstallBindings(DiContainer container);
    }
}