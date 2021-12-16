using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bot.ANN;
using Bot.BehaviourTree.Selectors;
using Bot.Objects;
using Bot.Utilities.Processed.Packet;

namespace Bot.Scenario
{
    public class ScenarioDataCollector
    {
        private bool _isRunning;

        private List<double[]> _values = new List<double[]>();
        private List<double[]> _targets = new List<double[]>();

        private double _collectionFrequency;
        private Bot _bot;
        private Func<bool>[] _targetFuncs;

        public ScenarioDataCollector(double collectionFrequency, Bot bot, params Func<bool>[] targetFuncs)
        {
            _bot = bot;
            _targetFuncs = targetFuncs;
            Reset(); // bit dirty maybe? idk and idc enough.
        }

        public void StartCollection ()
        {
            Console.WriteLine("Data collection starting..");
            Thread thread = new Thread(new ThreadStart(RunCollection));
            _isRunning = true;
            thread.Start();
        }

        public void StopCollection ()
        {
            Console.WriteLine("Data collection stopping..");
            _isRunning = false;
        }

        public void Reset ()
        {
            _isRunning = false;
            _values.Clear();
            _targets.Clear();
        }

        private void RunCollection ()
        {
            Console.WriteLine("Data collection running..");
            while (!_isRunning) // Was originally a CancellationToken but decided a boolean was simpler.
            {
                double[] values = GatherValues();
                double[] targets = GatherTargets();

                _values.Add(values);
                _targets.Add(targets);
            }
            Console.WriteLine("Data collection stopped.");
        }

        public List<DataSet> ToDataSets () // bit of a reach, could use an intermediary class, but fuck you >:D
        {
            List<DataSet> sets = new List<DataSet>();
            if (_values.Count != _targets.Count) // Something has gone wrong, these should always be the same.
            {
                throw new InvalidOperationException("Data values and targets miscount.");
            }
            int count = _values.Count;
            for (int i = 0; i < count; i++)
            {
                DataSet set = new DataSet(_values[i], _targets[i]);
                sets.Add(set);
            }
            return sets;
        }

        // Alternative is to have Bot.cs call a collect function where the relevant data is given.
        // I decided against this in order to maintain better decoupling between data collection and the bot.
        private double[] GatherValues ()
        {
            Packet packet = _bot.LastPacket;

            int ourIndex = _bot.Index;
            int enemyIndex = ourIndex == 0 ? 1 : 0;

            Player player = packet.Players[ourIndex];
            Player enemy = packet.Players[enemyIndex];
            Utilities.Processed.Packet.Ball ball = packet.Ball;

            return new double[] {
                player.Physics.Location.X,
                player.Physics.Location.Y,
                player.Physics.Location.Z,
                enemy.Physics.Location.X,
                enemy.Physics.Location.Y,
                enemy.Physics.Location.Z,
                ball.Physics.Location.X,
                ball.Physics.Location.Y,
                ball.Physics.Location.Z
                };
        }

        private double[] GatherTargets ()
        {
            double[] targets = new double[_targetFuncs.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = _targetFuncs[i]() ? 1 : 0;
            }
            return targets;
        }
    }
}
