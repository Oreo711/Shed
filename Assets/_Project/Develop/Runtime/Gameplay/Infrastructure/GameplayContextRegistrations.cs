using _Project.Develop.Runtime.Gameplay.Features;
using _Project.Develop.Runtime.Gameplay.Features.Entrance;
using _Project.Develop.Runtime.Infrastructure.DI;
using _Project.Develop.Runtime.Utilities.CoroutineManagement;


namespace _Project.Develop.Runtime.Gameplay.Infrastructure
{
    public static class GameplayContextRegistrations
    {
        public static void Process (DIContainer container)
        {
            container.Register(CreateTimer);

            container.Register(CreateEntranceActivatorService);
        }

        private static Timer CreateTimer (DIContainer c)
        {
            return new Timer(c.Resolve<ICoroutinePerformer>());
        }

        private static EntranceActivatorService CreateEntranceActivatorService (DIContainer c)
        {
            return new EntranceActivatorService(
                c.Resolve<Timer>(),
                c.Resolve<ICoroutinePerformer>());
        }
    }
}
