using UnityEngine;
using Zenject;

namespace MazeEscape.MazeGenerator
{
    public class GenerationInstaller : MonoInstaller<GenerationInstaller>
    {
        [SerializeField] private MazeGenerator m_mazeGenerator;

        public override void InstallBindings()
        {
            if (m_mazeGenerator.TryGetProcess<ICellGeneration>(out var cellGeneration))
                Container.Bind<ICellGeneration>().FromMethod((context) => cellGeneration).AsSingle();

            if (m_mazeGenerator.TryGetProcess<IEntranceGeneration>(out var entranceGeneration))
                Container.Bind<IEntranceGeneration>().FromMethod((context) => entranceGeneration).AsSingle();

            if (m_mazeGenerator.TryGetProcess<IStartCell>(out var startCellGeneration))
                Container.Bind<IStartCell>().FromMethod((context) => startCellGeneration).AsSingle();

            if (m_mazeGenerator.TryGetProcess<IFinishCell>(out var finishGeneration))
                Container.Bind<IFinishCell>().FromMethod((context) => finishGeneration).AsSingle();
            
            foreach (var process in m_mazeGenerator.GenerationProcesses)
            {
                Container.QueueForInject(process);
            }
        }
    }
}