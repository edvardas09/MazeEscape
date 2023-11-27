using UnityEngine;
using Zenject;

namespace MazeEscape.MazeGenerator.Signals
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Player>
            Container.DeclareSignal<PlayerHealthChangedSignal>();
        }
    }
}