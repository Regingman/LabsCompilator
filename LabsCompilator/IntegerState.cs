using System;
using System.Collections.Generic;
using System.Text;

namespace LabsCompilator
{
    public class Process
    {
        public class IntegerState
        {
            readonly ProcessState CurrentState;
            readonly Command Command;

            public IntegerState(ProcessState processState, Command command)
            {
                CurrentState = processState;
                Command = command;
            }
            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                IntegerState other = obj as IntegerState;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }



        }


        public override string ToString()
        {
            switch (CurrentState)
            {
                case ProcessState.Active: return "Active";
                case ProcessState.Inactive: return "Inactive";
                case ProcessState.FirstPhase: return "FirstPhase";
                case ProcessState.SecondPhase: return "SecondPhase";
                case ProcessState.Terminated: return "Terminated";
                default: return "error";
            }
        }

        Dictionary<IntegerState, ProcessState> transitions;
        public ProcessState CurrentState { get; private set; }

        public Process()
        {
            CurrentState = ProcessState.FirstPhase;
            transitions = new Dictionary<IntegerState, ProcessState>
            {
                { new IntegerState(ProcessState.Inactive, Command.Begin), ProcessState.FirstPhase},
                { new IntegerState(ProcessState.FirstPhase, Command.Resume), ProcessState.SecondPhase},
                { new IntegerState(ProcessState.FirstPhase, Command.Exit), ProcessState.Terminated},
                { new IntegerState(ProcessState.FirstPhase, Command.Exception), ProcessState.Error},
                { new IntegerState(ProcessState.SecondPhase, Command.Resume), ProcessState.SecondPhase},
                { new IntegerState(ProcessState.SecondPhase, Command.Exit), ProcessState.Terminated},
                { new IntegerState(ProcessState.SecondPhase, Command.Exception), ProcessState.Error},
            };
        }

        public ProcessState GetNext(Command command)
        {
            IntegerState transition = new IntegerState(CurrentState, command);
            ProcessState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public ProcessState MoveNext(Command command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }

    }

    public enum ProcessState
    {
        Active,
        Terminated,
        Inactive,
        FirstPhase,
        Error,
        SecondPhase
    }

    public enum Command
    {
        Begin,
        Resume,
        Exit,
        Exception
    }


}
