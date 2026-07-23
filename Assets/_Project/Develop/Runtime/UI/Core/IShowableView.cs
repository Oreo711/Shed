using _Project.Develop.Runtime.UI;
using DG.Tweening;

namespace _Project.Develop.Runtime.UI.Core
{
    public interface IShowableView : IView
    {
        Tween Hide();

        Tween Show();
    }
}
