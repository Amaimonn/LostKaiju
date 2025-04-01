using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Creatures.Views;
using UnityEngine;

public class SpikesTile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<ICreatureBinder>(out var creature))
        {
            if (creature.Features.TryResolve<IDamageReceiver>(out var damageReceiver))
            {
                damageReceiver.TakeDamage(9999); // TODO: class with damage Types
            }
        }
    }
}
