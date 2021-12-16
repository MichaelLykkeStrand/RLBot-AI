using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.ANN
{
    [Serializable]
    public class Neuron
    {
        public List<Synapse> inputs;
        public List<Synapse> outputs;
        public double value;
        public double gradient;
        double bias;

        public Neuron()
        {
            Random random = new Random();
            inputs = new List<Synapse>();
            outputs = new List<Synapse>();
            bias = (random.NextDouble() * (1 - 1) + 1);
                
        }

        public Neuron(IEnumerable<Neuron> inputNeurons) : this()
        {
            foreach (Neuron inputNeuron in inputNeurons)
            {
                Synapse s = new Synapse(inputNeuron, this);
                inputNeuron.outputs.Add(s);
                inputs.Add(s);
            }
        }

        public double CalculateValue()
        {
            double sum = 0;
            foreach (Synapse s in inputs)
            {
                sum += s.weight * s.inputNeuron.value + bias;
            }

            value = Sigmoid(sum);
            return value;
        }

        double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp((float)-x));
        }

        double SigmoidDerivative(double x)
        {
            return x * (1 - x);
        }

        public double CalculateError(double target)
        {
            return target - value;
        }

        public void CalculateGradient(double target) //for output layer
        {
            gradient = CalculateError(target) * SigmoidDerivative(value);
        }

        public void CalculateGradient() //for hidden layers
        {
            double sumOfErrorGradients = 0;
            foreach (Synapse s in outputs)
            {
                sumOfErrorGradients += s.weight * s.inputNeuron.gradient;
            }
            gradient = sumOfErrorGradients * SigmoidDerivative(value);
        }

        public void UpdateWeights(double learningRate)
        {
            //update bias
            double biasDelta = learningRate * gradient;
            bias += biasDelta;

            foreach (Synapse s in inputs)
            {
                double delta = learningRate * gradient * s.inputNeuron.value;
                s.weight += delta;
            }
        }
    }
}
