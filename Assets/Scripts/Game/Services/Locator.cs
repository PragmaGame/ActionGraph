using System.Collections.Generic;
using Common;

namespace Game.Services
{
    public class Locator : MonoSingleton<Locator>
    {
        private List<object> _instances;

        public T GetInstance<T>() 
        {
            foreach (var entity in _instances)
            {
                if (entity is T entityInstance)
                {
                    return entityInstance;
                }
            }

            return default;
        }
    }
}