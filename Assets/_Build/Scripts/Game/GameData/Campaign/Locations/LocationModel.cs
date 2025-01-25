using System.Collections.Generic;
using System.Linq;
using R3;
using ObservableCollections;

using LostKaiju.Game.GameData.Campaign.Missions;

namespace LostKaiju.Game.GameData.Campaign.Locations
{
    public class LocationModel
    {
        public ReactiveProperty<bool> IsOpened { get; }
        public ObservableList<MissionModel> Missions { get; } // all existing missions in this location
        public LocationState State { get; }

        public LocationModel(LocationState locationState, IEnumerable<MissionModel> missions)
        {
            State = locationState;

            IsOpened = new ReactiveProperty<bool>(locationState.IsOpened);
            IsOpened.Skip(1).Subscribe(x => State.IsOpened = true);

            Missions = new ObservableList<MissionModel>(missions);
            Missions.ForEach(mission => 
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