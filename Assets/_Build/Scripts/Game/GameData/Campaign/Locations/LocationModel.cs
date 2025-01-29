using System.Collections.Generic;
using System.Linq;
using R3;
using ObservableCollections;

using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    public class LocationModel : Model<LocationState>
    {
        public readonly ReactiveProperty<bool> IsOpened;
        public readonly ObservableList<MissionModel> AvailableMissions;

        public LocationModel(LocationState locationState, IEnumerable<MissionModel> availableMissions) : 
            base(locationState)
        {
            AvailableMissions = new ObservableList<MissionModel>(availableMissions);

            IsOpened = new ReactiveProperty<bool>(locationState.IsOpened);
            IsOpened.Skip(1).Subscribe(x => State.IsOpened = true);

            AvailableMissions = new ObservableList<MissionModel>(availableMissions);
            AvailableMissions.ForEach(mission => 
            {
                if (!mission.IsOpened.Value)
                    mission.IsOpened.Skip(1).Subscribe(x => {
                        if (x)
                            State.OpenedMissions.Add(mission.State);
                    });
            });
        }
    }
}