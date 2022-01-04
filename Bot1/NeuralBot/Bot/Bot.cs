using System.Collections.Generic;
using Bot.BehaviourTree;
using Bot.Utilities.Processed.BallPrediction;
using Bot.Utilities.Processed.FieldInfo;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;
using System.Threading;
using System.Windows.Forms;
using Bot.UI;
using Bot.Objects;
using System;
using Bot.Scenario;
using RLBotDotNet.GameState;
using System.Runtime.ExceptionServices;
using Bot.BehaviourTree.Actions;
using Bot.BehaviourTree.Selectors;

namespace Bot
{
    // We want to our bot to derive from Bot, and then implement its abstract methods.
    public class Bot : RLBotDotNet.Bot
    {
        public ScenarioController scenarioController;
        private BotTrainerForm botTrainer;
        public Packet LastPacket { get; private set; } // do you want to puke yet? :hehehe:

        public CompositeNode RootNode { get; private set; }

        // We want the constructor for our Bot to extend from RLBotDotNet.Bot, but we don't want to add anything to it.
        // You might want to add logging initialisation or other types of setup up here before the bot starts.
        public Bot(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex)
        {
            Console.WriteLine("test");
            scenarioController = new ScenarioController(this);
            scenarioController.OnNewScenarioReady += ScenarioController_OnNewScenarioReady;
            scenarioController.OnPlayScenario += ScenarioController_OnPlayScenario;
            var nodes = new List<Node>();
            nodes.Add(new Kickoff());
            //nodes.Add(new FlipToBall());
            nodes.Add(new Dribble());
            nodes.Add(new Recover());
            nodes.Add(new CollectBoost());
            nodes.Add(new BezierDrive());
            nodes.Add(new Wait());

            RootNode = new NeuralSelector(nodes);

            botTrainer = new BotTrainerForm(this);
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        
                        Application.Run(botTrainer);
                    } catch (Exception exc)
                    {
                        Console.WriteLine("Failed to open bot trainer, trying again in 10 seconds.");
                        Console.WriteLine(exc.Message + " - " + exc.StackTrace);
                    }
                    Thread.Sleep(10000);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        [HandleProcessCorruptedStateExceptions]
        private void ScenarioController_OnPlayScenario(object sender, EventArgs e)
        {
            try
            {
                GameState gameState = scenarioController.currentState;
                gameState.GameInfoState.Paused = false;
                SetGameState(gameState);
                
            }
            catch (Exception) { }
            
        }
        [HandleProcessCorruptedStateExceptions]
        private void ScenarioController_OnNewScenarioReady(object sender, EventArgs e)
        {
            try
            {
                SetGameState(scenarioController.currentState);
            }
            catch (Exception) { }

        }

        public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
        {
            // We process the gameTickPacket and convert it to our own internal data structure.
            LastPacket = new Packet(gameTickPacket);
            // Updates the ball's position, velocity, etc
            Objects.Ball.Update(this, gameTickPacket.Ball.Value);
            // Updates the game's score, time, etc
            Game.Update(gameTickPacket);
            var state = RootNode.Update(this, LastPacket);
            return Game.OutoutControls;
        }
        
        // Hide the old methods that return Flatbuffers objects and use our own methods that
        // use processed versions of those objects instead.
        internal new FieldInfo GetFieldInfo() => new FieldInfo(base.GetFieldInfo());
        internal new BallPrediction GetBallPrediction() => new BallPrediction(base.GetBallPrediction());
    }
}