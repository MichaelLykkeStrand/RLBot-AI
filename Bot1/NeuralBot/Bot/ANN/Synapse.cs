using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.ANN
{

    public class Synapse
    {
        public Neuron inputNeuron;
        public Neuron outputNeuron;
        public double weight;

        public Synapse(Neuron input, Neuron output)
        {
            Random random = new Random();
            inputNeuron = input;
            outputNeuron = output;
            weight = (random.NextDouble() * (1 - 1) + 1);
        }
    }
}
