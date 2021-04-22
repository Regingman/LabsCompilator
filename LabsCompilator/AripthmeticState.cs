using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LabsCompilator
{
    public class ProcessAriphmetic
    {
        static Regex regexNumber = new Regex(@"^[\p{N}]+$");

        public class AriphmeticState
        {
            readonly ProcessStateAriphmetic CurrentState;
            readonly CommandAriphmetic CommandAriphmetic;

            public AriphmeticState(ProcessStateAriphmetic ProcessStateAriphmetic, CommandAriphmetic CommandAriphmetic)
            {
                CurrentState = ProcessStateAriphmetic;
                this.CommandAriphmetic = CommandAriphmetic;
            }
            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * CommandAriphmetic.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                AriphmeticState other = obj as AriphmeticState;
                return other != null && this.CurrentState == other.CurrentState && this.CommandAriphmetic == other.CommandAriphmetic;
            }



        }


        public override string ToString()
        {
            switch (CurrentState)
            {
                case ProcessStateAriphmetic.Inactive: return "Inactive";
                case ProcessStateAriphmetic.FirstPhase: return "FirstPhase";
                case ProcessStateAriphmetic.SecondPhase: return "SecondPhase";
                case ProcessStateAriphmetic.ThirdPhase: return "ThirdPhase";
                case ProcessStateAriphmetic.Terminated: return "Terminated";
                default: return "error";
            }
        }

        Dictionary<AriphmeticState, ProcessStateAriphmetic> transitions;
        public ProcessStateAriphmetic CurrentState { get; private set; }

        public ProcessAriphmetic()
        {
            CurrentState = ProcessStateAriphmetic.Inactive;
            transitions = new Dictionary<AriphmeticState, ProcessStateAriphmetic>
            {
                { new AriphmeticState(ProcessStateAriphmetic.Inactive, CommandAriphmetic.Resume), ProcessStateAriphmetic.FirstPhase},
                { new AriphmeticState(ProcessStateAriphmetic.FirstPhase, CommandAriphmetic.Exception), ProcessStateAriphmetic.Error},
                { new AriphmeticState(ProcessStateAriphmetic.FirstPhase, CommandAriphmetic.Resume), ProcessStateAriphmetic.SecondPhase},
                { new AriphmeticState(ProcessStateAriphmetic.FirstPhase, CommandAriphmetic.AripthmenticResume), ProcessStateAriphmetic.Aripthmetic},
                { new AriphmeticState(ProcessStateAriphmetic.SecondPhase, CommandAriphmetic.Resume), ProcessStateAriphmetic.SecondPhase},
                { new AriphmeticState(ProcessStateAriphmetic.SecondPhase, CommandAriphmetic.Exception), ProcessStateAriphmetic.Error},
                { new AriphmeticState(ProcessStateAriphmetic.SecondPhase, CommandAriphmetic.AripthmenticResume), ProcessStateAriphmetic.Aripthmetic},
                { new AriphmeticState(ProcessStateAriphmetic.Aripthmetic, CommandAriphmetic.Resume), ProcessStateAriphmetic.ThirdPhase},
                { new AriphmeticState(ProcessStateAriphmetic.Aripthmetic, CommandAriphmetic.Exception), ProcessStateAriphmetic.Error},
                { new AriphmeticState(ProcessStateAriphmetic.ThirdPhase, CommandAriphmetic.AripthmenticResume), ProcessStateAriphmetic.Aripthmetic},
                { new AriphmeticState(ProcessStateAriphmetic.ThirdPhase, CommandAriphmetic.Resume), ProcessStateAriphmetic.FourPhase},
                { new AriphmeticState(ProcessStateAriphmetic.ThirdPhase, CommandAriphmetic.Exception), ProcessStateAriphmetic.Error},
                { new AriphmeticState(ProcessStateAriphmetic.ThirdPhase, CommandAriphmetic.Exit), ProcessStateAriphmetic.Terminated},
                { new AriphmeticState(ProcessStateAriphmetic.FourPhase, CommandAriphmetic.Resume), ProcessStateAriphmetic.FourPhase},
                { new AriphmeticState(ProcessStateAriphmetic.FourPhase, CommandAriphmetic.AripthmenticResume), ProcessStateAriphmetic.Aripthmetic},
                { new AriphmeticState(ProcessStateAriphmetic.FourPhase, CommandAriphmetic.Exception), ProcessStateAriphmetic.Error},
                { new AriphmeticState(ProcessStateAriphmetic.FourPhase, CommandAriphmetic.Exit), ProcessStateAriphmetic.Terminated},
            };
        }

        public ProcessStateAriphmetic GetNext(CommandAriphmetic CommandAriphmetic)
        {
            AriphmeticState transition = new AriphmeticState(CurrentState, CommandAriphmetic);
            ProcessStateAriphmetic nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + CommandAriphmetic);
            return nextState;
        }

        public ProcessStateAriphmetic MoveNext(CommandAriphmetic CommandAriphmetic)
        {
            CurrentState = GetNext(CommandAriphmetic);
            Console.WriteLine("State:" + CurrentState.ToString());
            return CurrentState;
        }



    }

    public enum ProcessStateAriphmetic
    {
        Inactive,
        Terminated,
        FirstPhase,
        SecondPhase,
        ThirdPhase,
        FourPhase,
        Aripthmetic,
        Error
    }

    public enum CommandAriphmetic
    {
        Begin,
        Resume,
        AripthmenticResume,
        AfterAripthmeticResume,
        Exit,
        Exception
    }

}
