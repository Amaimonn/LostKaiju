using R3;

using LostKaiju.Boilerplates.UI.MVVM;
using LostKaiju.Game.Constants;
using LostKaiju.Services.Audio;
using LostKaiju.Game.GameData.Campaign.Missions;
using LostKaiju.Game.GameData.Campaign.Locations;

namespace LostKaiju.Game.UI.MVVM.Gameplay.MissionResults
{
    public class MissionResultsBinder : Binder<MissionResultsViewModel>
    {
        private readonly AudioPlayer _audioPlayer;
        private readonly ILocationData _locationData;
        private readonly IMissionData _missionData;
        private MissionResultsParameters _results;

        public MissionResultsBinder(IRootUIBinder rootUIBinder, AudioPlayer audioPlayer, 
            ILocationData locationData, IMissionData missionData) : 
            base(rootUIBinder)
        {
            _audioPlayer = audioPlayer;
            _locationData = locationData;
            _missionData = missionData;
        }

        public void SetResults(MissionResultsParameters results)
        {
            _results = results;
        }

        public override bool TryBindAndOpen(out MissionResultsViewModel viewModel)
        {
            if (_currentViewModel != null) // if already exists
            {
                viewModel = null;
                return false;
            }

            var resultsView = LoadAndInstantiateView<MissionResultsView>(Paths.MISSION_RESULTS_VIEW);
            resultsView.Construct(_audioPlayer);
            
            _currentViewModel = new MissionResultsViewModel(_locationData, _missionData, _results);
            
            _currentViewModel.OnClosingCompleted.Subscribe(_ => {
                _rootUIBinder.ClearView(resultsView);
            });
            resultsView.OnDisposed.Take(1).Subscribe(_ => {
                // _currentViewModel?.Dispose();
                _currentViewModel = null;
                // _modelFactory.Release();
            });

            resultsView.Bind(_currentViewModel);
            _rootUIBinder.AddView(resultsView);
            _currentViewModel.Open();
            _onOpened.OnNext(_currentViewModel);
            
            viewModel = _currentViewModel;
            return true;
        }
    }
}