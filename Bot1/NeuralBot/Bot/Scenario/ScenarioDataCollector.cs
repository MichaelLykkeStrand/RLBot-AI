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
        private List<double[]> _values = new List<double[]>();
        private List<double[]> _targets = new List<double[]>();

        private Bot _bot;

        public ScenarioDataCollector(Bot bot)
        {
            _bot = bot;
            Reset(); // bit dirty maybe? idk and idc enough.
        }

        public void Reset ()
        {
            _values.Clear();
            _targets.Clear();
        }

        public void Collect(double[] targets)
        {
            double[] values = GatherValues();

            _values.Add(values);
            _targets.Add(targets);
            Console.WriteLine("Data collected.");
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
    }
}
