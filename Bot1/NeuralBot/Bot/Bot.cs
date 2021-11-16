using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using Bot.AnalysisUtils;
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

namespace Bot
{
    // We want to our bot to derive from Bot, and then implement its abstract methods.
    public class Bot : RLBotDotNet.Bot
    {
        List<Node> _nodes;
        private PrioritySelector tmpRootNode;
        public ScenarioController scenarioController = new ScenarioController();

        public List<Node> Nodes { get => _nodes; set => _nodes = value; }

        // We want the constructor for our Bot to extend from RLBotDotNet.Bot, but we don't want to add anything to it.
        // You might want to add logging initialisation or other types of setup up here before the bot starts.
        public Bot(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex)
        {
            Nodes = new List<Node>();
            FlipToBall ftb = new FlipToBall();
            BezierDrive bzd = new BezierDrive();
            //nodes.Add(ftb);
            Nodes.Add(bzd);
            tmpRootNode = new PrioritySelector(Nodes);

            BotTrainerForm botTrainer = new BotTrainerForm(this);
            Thread thread = new Thread(() =>
            {
                Application.Run(botTrainer);
            });
            thread.Start();

            scenarioController.Generate();

        }

        public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
        {
            
            // We process the gameTickPacket and convert it to our own internal data structure.
            Packet packet = new Packet(gameTickPacket);
            // Updates the ball's position, velocity, etc
            Objects.Ball.Update(this, gameTickPacket.Ball.Value);
            // Updates the game's score, time, etc
            Game.Update(gameTickPacket);

            var tmpController = new Controller();
            var tmpControl = tmpRootNode.Update(this, packet, ref tmpController).controller;
            return tmpControl;
        }
        
        // Hide the old methods that return Flatbuffers objects and use our own methods that
        // use processed versions of those objects instead.
        internal new FieldInfo GetFieldInfo() => new FieldInfo(base.GetFieldInfo());
        internal new BallPrediction GetBallPrediction() => new BallPrediction(base.GetBallPrediction());
    }
}