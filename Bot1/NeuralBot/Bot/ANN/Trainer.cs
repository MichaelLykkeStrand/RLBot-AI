using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.ANN
{
    public class Trainer
    {
        private const int HIDDEN_LAYERS = 2;
        private const int NEURONS_IN_HIDDEN_LAYERS = 10;

        private DataSet[] _dataSet;
        private int _epochs;

        public ANN NeuralNetwork { get; private set; }

        public Trainer (IEnumerable<DataSet> dataSet, int epochs)
        {
            _dataSet = dataSet.ToArray();
            _epochs = epochs;
        }

        public void TrainAFucker ()
        {
            DataSet peek = _dataSet[0];
            NeuralNetwork = new ANN(peek.values.Length, NEURONS_IN_HIDDEN_LAYERS, peek.targets.Length, HIDDEN_LAYERS);
            NeuralNetwork.Train(_dataSet.ToList(), _epochs);
        }
    }
}
