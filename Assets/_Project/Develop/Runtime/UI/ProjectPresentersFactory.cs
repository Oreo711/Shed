
using Shed.Runtime.Infrastructure.DI;


namespace Shed.Runtime.UI
{
    public class ProjectPresentersFactory
    {
        private readonly DIContainer _container;

        public ProjectPresentersFactory(DIContainer container)
        {
            _container = container;
        }
    }
}
