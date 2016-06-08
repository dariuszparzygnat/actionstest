using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SWDProject.Model;

namespace SWDProject.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AlgorithmViewModel : ViewModelBase
    {
        private double _fuel=70;
        private int _laps=66;
        private double _fuelConsumption=1;
        private TimeSpan _bestLapTime = new TimeSpan(0,1,24);
        private TimeSpan _pitStopTime= new TimeSpan(0,0,20);
        private int _suggestedLapsForSoftTyres=30;
        private int _suggestedLapsForHardTyres=30;
        private int _tyreStateAtTheBeginning=15;
        private List<DataItem> _lapInfos;
        private TimeSpan _raceTime;
        private bool _tyreType = true;

        public bool TyreType
        {
            get { return _tyreType; }
            set
            {
                if (_tyreType == value)
                    return;
                _tyreType = value;
                RaisePropertyChanged();
            }
        }

        public double Fuel
        {
            get
            {
                return _fuel;
            }
            set
            {
                if (_fuel== value)
                    return;
                _fuel = value;
                RaisePropertyChanged();
            }
        }

        public double FuelConsumption
        {
            get
            {
                return _fuelConsumption;
            }
            set
            {
                if (_fuelConsumption == value)
                    return;
                _fuelConsumption = value;
                RaisePropertyChanged();
            }
        }

        public int Laps
        {
            get
            {
                return _laps;
            }
            set
            {
                if (_laps == value)
                    return;
                _laps = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan RaceTime
        {
            get
            {
                return _raceTime;
            }
            set
            {
                if (_raceTime == value)
                    return;
                _raceTime = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan BestLapTime
        {
            get
            {
                return _bestLapTime;
            }
            set
            {
                if (_bestLapTime == value)
                    return;
                _bestLapTime = value;
                RaisePropertyChanged();
            }
        }

        public TimeSpan PitStopTime
        {
            get
            {
                return _pitStopTime;
            }
            set
            {
                if (_pitStopTime == value)
                    return;
                _pitStopTime = value;
                RaisePropertyChanged();
            }
        }

        public int SuggestedLapsForSoftTyres
        {
            get
            {
                return _suggestedLapsForSoftTyres;
            }
            set
            {
                if (_suggestedLapsForSoftTyres == value)
                    return;
                _suggestedLapsForSoftTyres = value;
                RaisePropertyChanged();
            }
        }

        public int SuggestedLapsForHardTyres
        {
            get
            {
                return _suggestedLapsForHardTyres;
            }
            set
            {
                if (_suggestedLapsForHardTyres == value)
                    return;
                _suggestedLapsForHardTyres = value;
                RaisePropertyChanged();
            }
        }

        public int TyreStateAtTheBeginning
        {
            get
            {
                return _tyreStateAtTheBeginning;
            }
            set
            {
                if (_tyreStateAtTheBeginning == value)
                    return;
                _tyreStateAtTheBeginning = value;
                RaisePropertyChanged();
            }
        }

        public List<DataItem> LapInfos
        {
            get
            {
                return _lapInfos;
            }
            set
            {
                if (_lapInfos == value)
                    return;
                _lapInfos = value;
                RaisePropertyChanged();
            }
        }

        public ICommand StartCommand { get; }


        /// <summary>
        /// Initializes a new instance of the AlgorithmViewModel class.
        /// </summary>
        public AlgorithmViewModel()
        {
            StartCommand = new RelayCommand(StartSimulation);
        }

        public async void StartSimulation()
        {
            DriveSimulation myDriveSimulation = new DriveSimulation();
            myDriveSimulation.Race = new Race(Laps, PitStopTime, SuggestedLapsForSoftTyres, SuggestedLapsForHardTyres);
            myDriveSimulation.Car = new Car(new PerformanceSet(BestLapTime), myDriveSimulation.Race);
            TyreSet startTyreType;
            if (_tyreType)
                startTyreType = new SoftTyreSet(TyreStateAtTheBeginning);
            else
                startTyreType = new HardTyreSet(TyreStateAtTheBeginning); 
            myDriveSimulation.CarStateTree = new CarStateTree(new CarState(myDriveSimulation.Car, startTyreType, 0));
            var result = await myDriveSimulation.Start();
            var preaparedData = await PreapareData(result.BestRacePlanOverall);

            LapInfos = preaparedData;
            RaceTime = result.BestRacePlanOverall.TotalTime;
        }

        public Task<List<DataItem>> PreapareData(RacePlan racePlan)
        {
            var result = Task.Run(() =>
            {
                var list = new List<DataItem>();
                var startState = racePlan.CarStates.Last();
                list.Add(new DataItem() { Lap = startState.OnLap, TyreType = startState.TyreSet.ToString(),TyreState = startState.TyreSet.State});
                racePlan.CarStates.Reverse();
                for (int i = 0; i <racePlan.LapTimes.Count; i++)
                {
                    var lapTime = racePlan.LapTimes[i];
                    var carState = racePlan.CarStates[i+1];
                    list.Add(new DataItem() { Lap = carState.OnLap, TyreType = carState.TyreSet.ToString(), LapTime = lapTime, TyreState = carState.TyreSet.State});
                }
                return list;
            });

            return result;
        } 
    }
}