using System;
using System.Drawing;
using System.Threading;
using Console = Colorful.Console;

namespace BlackMesa {
    class Elevator {
        public Floor CurrentFloor { get; private set; }

        // set - agent inside
        // released - empty
        public ManualResetEvent eventIsOccupied;

        // set - moving
        // released - not moving -> it can be called either
        //   by the agent inside (if any) or by another from
        //   another floor
        private ManualResetEvent eventIsMoving;

        // set - the door is closed
        // released - the door is closed
        private ManualResetEvent eventIsDoorClosed;

        public Agent CurrentAgent;

        // miscelaneous props
        private Color ConsoleColor = Color.White;
        private object locker = new object();

        public Elevator() {
            CurrentFloor = Floor.G;
            eventIsOccupied = new ManualResetEvent(false);
            eventIsMoving = new ManualResetEvent(false);
            eventIsDoorClosed = new ManualResetEvent(false);
            CurrentAgent = null;
        }

        private void Move(Floor floor) {
            eventIsMoving.Set();
            Console.WriteLine($"Elevator is moving from floor {CurrentFloor.ToString()} to {floor.ToString()}.", ConsoleColor);
            int travelTime = Math.Abs(CurrentFloor - floor) * 2000;
            CurrentFloor = floor;
            Thread.Sleep(travelTime);
            Console.WriteLine($"Elevator has arrived at floor {CurrentFloor.ToString()}.", ConsoleColor);
            eventIsMoving.Reset();
        }

        public void Occupy(Agent agent) {
            if (!IsOccupied) {
                eventIsOccupied.Set();
                CurrentAgent = agent;
                // if the agent has traveled at least once with the 
                // elevator, he's done good job and next time 
                // he arrives at floor G, he can leave.
                CurrentAgent.hasWorked = true;
                Call();
            }
            else {
                Console.WriteLine($"{agent.Name} cannot call the elevator as it is currently occupied by {CurrentAgent.Name}", agent.ConsoleColor);
            }
        }

        public void Call() {
            if (CurrentFloor != CurrentAgent.CurrentFloor) {
                Console.WriteLine($"{CurrentAgent.Name} called the elevator to floor {CurrentAgent.CurrentFloor.ToString()}.", CurrentAgent.ConsoleColor);
                CloseDoor();
                Move(CurrentAgent.CurrentFloor);
                OpenDoor();
            }
            Enter();
        }

        public void Enter() {
            Console.WriteLine($"{CurrentAgent.Name} is entering the elevator", CurrentAgent.ConsoleColor);
            Thread.Sleep(1000);
            GoToFloor(CurrentAgent.GetRandomFloor());
        }

        public void GoToFloor(Floor floor) {
            Console.WriteLine($"{CurrentAgent.Name} chose to go to floor {floor.ToString()}.", CurrentAgent.ConsoleColor);
            CloseDoor();
            Move(floor);
            Console.WriteLine($"{CurrentAgent.Name} has arrived at floor {CurrentFloor}", CurrentAgent.ConsoleColor);

            if (CurrentAgent.CanLeaveAtFloor(CurrentFloor)) {
                OpenDoor();
                Leave();
            }
            else {
                GoToFloor(CurrentAgent.GetRandomFloor());
            }
        }

        public void Leave() {
            Console.WriteLine($"{CurrentAgent.Name} is leaving the elevator at floor {CurrentFloor}", CurrentAgent.ConsoleColor);
            Thread.Sleep(1000);
            CurrentAgent = null;
            eventIsOccupied.Reset();
        }

        // Door actions
        private void OpenDoor() {
            eventIsDoorClosed.Reset();
            Console.WriteLine("Door is opening", ConsoleColor);
            Thread.Sleep(1000);
        }

        private void CloseDoor() {
            eventIsDoorClosed.Set();
            Console.WriteLine("Door is closing", ConsoleColor);
            Thread.Sleep(1000);
        }

        // check elevator status parametres
        public bool IsMoving {
            get {
                lock (eventIsMoving) {
                    return eventIsMoving.WaitOne(0);
                }
            }
        }

        public bool IsOccupied {
            get {
                lock (eventIsOccupied) {
                    return eventIsOccupied.WaitOne(0);
                }
            }
        }

        public bool IsDoorClosed {
            get {
                lock (eventIsDoorClosed) {
                    return eventIsDoorClosed.WaitOne(0);
                }
            }
        }
    }
}
