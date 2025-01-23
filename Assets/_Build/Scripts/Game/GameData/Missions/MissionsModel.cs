using System.Collections.Generic;
using System.Linq;
using R3;

namespace LostKaiju.Game.GameData.Missions
{
    public class MissionsModel
    {
        public readonly ReactiveProperty<MissionData> SelectedMission;
        public readonly IReadOnlyList<MissionData> MissionDataList;

        public MissionsModel(IEnumerable<MissionData> missionDatas, MissionData selectedMission = null)
        {
            MissionDataList = new List<MissionData>(missionDatas);

            if (selectedMission == null && missionDatas.Count() > 0)
            {
                var baseSelectedMission = missionDatas.First();
                SelectedMission = new ReactiveProperty<MissionData>(baseSelectedMission);
            }
            else 
            {
                SelectedMission = new ReactiveProperty<MissionData>(selectedMission);
            }
        }
    }
}