using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ActionGraph.Runtime.Commands.Absrtract;
using ActionGraph.Runtime.Commands.Implementation.Container.Runners.Abstract;
using ActionGraph.Runtime.Commands.Implementation.Container.Runners.Implementation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace ActionGraph.Runtime.Commands.Implementation.Container
{
    [Serializable]
    public class ContainerCommand : IActionCommand
    {
        [SerializeReference] private IProcessRunner _runner = new ParallelProcessRunner();
        [SerializeReference] private List<IActionCommand> _commands = new();

        public ContainerCommand()
        {
        }
        
        protected ContainerCommand(ContainerCommand data)
        {
            _runner = data._runner.Clone();
            _commands = data._commands.Select(processor => processor.Clone()).ToList();
        }

        [Inject]
        public void Construct(DiContainer container)
        {
            foreach (var processor in _commands)
            {
                container.Inject(processor);
            }
        }

        public IActionCommand Clone()
        {
            return new ContainerCommand(this);
        }
        
        public UniTask Execute(CancellationToken token = default)
        {
            return _runner.RunProcess(_commands.Select(processor => processor.Execute(token)));
        }
        
#if UNITY_EDITOR
        public string GetInfo(string separator)
        {
            var info = new StringBuilder();
            
            foreach (var command in _commands)
            {
                var commandInfo = command.GetInfo();

                if (string.IsNullOrEmpty(commandInfo))
                {
                    continue;
                }

                info.Append(commandInfo);
                info.Append(separator);
            }

            if (info.Length > 0)
            {
                info.Remove(info.Length - separator.Length, separator.Length);
            }
            
            return info.ToString();
        }
#endif
    }
}