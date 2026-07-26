namespace Shed.Runtime.UI.Core
{
    public interface ISubscribedPresenter : IPresenter
    {
        void Subscribe();

        void Unsubscribe();
    }
}
