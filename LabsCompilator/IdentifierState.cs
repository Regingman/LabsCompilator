using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LabsCompilator
{
    public class ProcessIdentifier
    {
        static Regex regexLetter = new Regex(@"^[\p{L}]+$");
        static Regex regexNumber = new Regex(@"^[\p{N}]+$");

        public class IdentifierState
        {
            readonly ProcessStateIdentifier CurrentState;
            readonly CommandIdentifier CommandIdentifier;

            public IdentifierState(ProcessStateIdentifier ProcessStateIdentifier, CommandIdentifier CommandIdentifier)
            {
                CurrentState = ProcessStateIdentifier;
                this.CommandIdentifier = CommandIdentifier;
            }
            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * CommandIdentifier.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                IdentifierState other = obj as IdentifierState;
                return other != null && this.CurrentState == other.CurrentState && this.CommandIdentifier == other.CommandIdentifier;
            }



        }


        public override string ToString()
        {
            switch (CurrentState)
            {
                case ProcessStateIdentifier.Inactive: return "Inactive";
                case ProcessStateIdentifier.FirstPhase: return "FirstPhase";
                case ProcessStateIdentifier.SecondPhase: return "SecondPhase";
                case ProcessStateIdentifier.ThirdPhase: return "ThirdPhase";
                case ProcessStateIdentifier.Terminated: return "Terminated";
                default: return "error";
            }
        }

        Dictionary<IdentifierState, ProcessStateIdentifier> transitions;
        public ProcessStateIdentifier CurrentState { get; private set; }

        public ProcessIdentifier()
        {
            CurrentState = ProcessStateIdentifier.Inactive;
            transitions = new Dictionary<IdentifierState, ProcessStateIdentifier>
            {
                { new IdentifierState(ProcessStateIdentifier.Inactive, CommandIdentifier.Begin), ProcessStateIdentifier.FirstPhase},
                { new IdentifierState(ProcessStateIdentifier.Inactive, CommandIdentifier.Exception), ProcessStateIdentifier.Error},
                { new IdentifierState(ProcessStateIdentifier.FirstPhase, CommandIdentifier.Exit), ProcessStateIdentifier.Terminated},
                { new IdentifierState(ProcessStateIdentifier.FirstPhase, CommandIdentifier.Exception), ProcessStateIdentifier.Error},
                { new IdentifierState(ProcessStateIdentifier.FirstPhase, CommandIdentifier.ResumeLetter), ProcessStateIdentifier.SecondPhase},
                { new IdentifierState(ProcessStateIdentifier.FirstPhase, CommandIdentifier.ResumeNumeral), ProcessStateIdentifier.ThirdPhase},
                { new IdentifierState(ProcessStateIdentifier.SecondPhase, CommandIdentifier.ResumeLetter), ProcessStateIdentifier.SecondPhase},
                { new IdentifierState(ProcessStateIdentifier.SecondPhase, CommandIdentifier.ResumeNumeral), ProcessStateIdentifier.ThirdPhase},
                { new IdentifierState(ProcessStateIdentifier.SecondPhase, CommandIdentifier.Exit), ProcessStateIdentifier.Terminated},
                { new IdentifierState(ProcessStateIdentifier.SecondPhase, CommandIdentifier.Exception), ProcessStateIdentifier.Error},
                { new IdentifierState(ProcessStateIdentifier.ThirdPhase, CommandIdentifier.Exception), ProcessStateIdentifier.Error},
                { new IdentifierState(ProcessStateIdentifier.ThirdPhase, CommandIdentifier.Exit), ProcessStateIdentifier.Terminated},
                { new IdentifierState(ProcessStateIdentifier.ThirdPhase, CommandIdentifier.ResumeLetter), ProcessStateIdentifier.SecondPhase},
                { new IdentifierState(ProcessStateIdentifier.ThirdPhase, CommandIdentifier.ResumeNumeral), ProcessStateIdentifier.ThirdPhase},
            };
        }

        public ProcessStateIdentifier GetNext(CommandIdentifier CommandIdentifier)
        {
            IdentifierState transition = new IdentifierState(CurrentState, CommandIdentifier);
            ProcessStateIdentifier nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + CommandIdentifier);
            return nextState;
        }

        public ProcessStateIdentifier MoveNext(CommandIdentifier CommandIdentifier)
        {
            CurrentState = GetNext(CommandIdentifier);
            return CurrentState;
        }

      

    }

    public enum ProcessStateIdentifier
    {
        Inactive,
        Terminated,
        FirstPhase,
        SecondPhase,
        ThirdPhase,
        Error
    }

    public enum CommandIdentifier
    {
        Begin,
        ResumeLetter,
        ResumeNumeral,
        Exit,
        Exception
    }

}
