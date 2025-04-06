using R3;

using LostKaiju.Game.GameData.Campaign.Locations;
using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.UI.MVVM.Gameplay.MissionResults
{
    public class MissionResultsViewModel : ScreenViewModel
    {
        public ILocationData LocationData { get; }
        public IMissionData MissionData { get; }
        public ReadOnlyReactiveProperty<bool> FirstStarAwarded => _firstStarAwarded;
        public ReadOnlyReactiveProperty<bool> SecondStarAwarded => _secondStarAwarded;
        public ReadOnlyReactiveProperty<bool> ThirdStarAwarded => _thirdStarAwarded;
        
        private readonly ReactiveProperty<bool> _firstStarAwarded = new(false);
        private readonly ReactiveProperty<bool> _secondStarAwarded = new(false);
        private readonly ReactiveProperty<bool> _thirdStarAwarded = new(false);

        public MissionResultsViewModel(ILocationData locationData, IMissionData missionData,
            MissionResultsParameters parameters)
        {
            LocationData = locationData;
            MissionData = missionData;
            _firstStarAwarded.Value = parameters.FirstStar;
            _secondStarAwarded.Value = parameters.SecondStar;
            _thirdStarAwarded.Value = parameters.ThirdStar;
        }
    }
}