using DG.Tweening;


namespace Shed.Runtime.UI.Core
{
    public interface IShowableView : IView
    {
        Tween Hide();

        Tween Show();
    }
}
