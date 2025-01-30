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
        public readonly ObservableDictionary<string, MissionModel> AvailableMissionsMap;
        public readonly ILocationData Data;

        public LocationModel(LocationState locationState, ILocationData locationData) : 
            base(locationState)
        {
            Data = locationData;
            var availableMissions = locationState.OpenedMissions.Select(availableMission =>
            {
                // TODO: optimize the search
                var missionData = locationData.AllMissionsData
                    .Where(m => m.Id == availableMission.Id)
                    .FirstOrDefault();
                return new MissionModel(availableMission, missionData);
            });

            IsOpened = new ReactiveProperty<bool>(locationState.IsOpened);
            IsOpened.Skip(1).Subscribe(x => State.IsOpened = true);
            var availableMissionsMap = availableMissions
                .Select(x => new KeyValuePair<string, MissionModel>(x.State.Id, x));
            AvailableMissionsMap = new ObservableDictionary<string, MissionModel>(availableMissionsMap);
            AvailableMissionsMap
                .ObserveAdd()
                .Subscribe(x => locationState.OpenedMissions.Add(x.Value.Value.State));
            // foreach (var availableMission in availableMissions)
            // {
            //     if (!availableMission.IsOpened.Value)
            //         availableMission.IsOpened.Skip(1).Subscribe(x => {
            //             if (x)
            //                 State.OpenedMissions.Add(availableMission.State);
            //         });
                
            // }
        }
    }
}