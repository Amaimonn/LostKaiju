using UnityEngine;

using LostKaiju.Game.Creatures.Features;
using LostKaiju.Game.Creatures.Views;
using LostKaiju.Game.Creatures.Presenters;

namespace LostKaiju.Game.Player.Views
{
    public class PlayerBinder : CreatureBinder, ICreatureUpdater
    {    
        [Header("Creature features")]
        [SerializeField] private Flipper _flipper;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private DamageReceiver _damageReceiver;
        [SerializeField] private Attacker _attacker;

        protected IUpdatablePresenter _updatablePresenter;

#region ICreatureUpdater
        public void SetUpdatablePresenter(IUpdatablePresenter updatablePresenter)
        {
            _updatablePresenter = updatablePresenter;
        }
#endregion

        private void Awake()
        {
            _features.Register<Flipper>(_flipper);
            _features.Register<GroundCheck>(_groundCheck);
            _features.Register<DamageReceiver>(_damageReceiver);
            _features.Register<ICreatureUpdater>(this);
            _features.Register<IAttacker>(_attacker);
            Debug.Log("Player features registered");
        }

        private void Update()
        {
            _updatablePresenter?.UpdateLogic();
        }

        private void FixedUpdate()
        {
            _updatablePresenter?.FixedUpdateLogic();
        }
    }
}
