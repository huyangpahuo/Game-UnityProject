using UnityEngine;

namespace MyAssets.Framework
{
    public interface IMovable
    {
        void Init(BackgroundItemController controller);
        void Move();
    }
}