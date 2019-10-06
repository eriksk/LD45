
using UnityEngine;

namespace Skoggy.LD45.Game
{
    public interface IPickupable
    {
        bool IsCart();
        bool IsProduct();
        Vector3 Position { get; }
    }
}