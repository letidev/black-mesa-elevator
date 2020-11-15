using System;
using System.Threading;
using Console = Colorful.Console;

namespace BlackMesa {
    class Elevator {
        public Floor CurrentFloor { get; set; }

        // set - agent inside
        // released - empty
        private ManualResetEvent eventIsOccupied;

        // set - moving
        // released - not moving -> it can be called either
        //   by the agent inside (if any) or by another from
        //   another floor
        private ManualResetEvent eventIsMoving;

        // set - the door is closed
        // released - the door is open and will be
        //   for one second  so that the agent inside (if any)
        //   can leave and the one one the floor (if any) can enter 
        private ManualResetEvent eventIsDoorClosed;

        private object locker = new object();

        public Elevator() {
            CurrentFloor = Floor.G;
            eventIsOccupied = new ManualResetEvent(false);
            eventIsMoving = new ManualResetEvent(false);
        }

        // movement actions
        private void Move(Floor floor) {
            eventIsMoving.Set();
            Console.WriteLine($"Elevator is moving from floor {CurrentFloor.ToString()} to {floor.ToString()}.");
            int travelTime = Math.Abs(CurrentFloor - floor) * 1000;
            CurrentFloor = floor;
            Thread.Sleep(travelTime);
            eventIsMoving.Reset();
        }

        public bool Call(Agent agent, Floor floor) {
            if (!eventIsMoving.WaitOne(0)) {
                Console.WriteLine($"{agent.Name} called the elevator to floor {floor.ToString()}.", agent.ConsoleColor);
                Move(floor);
                return true;
            }
            else {
                return false;
            }
        }

        // enter/leave actions
        public void Enter(Agent agent) {
            lock (locker) {
                if (!IsOccupied) {
                    eventIsOccupied.Set();
                    Console.WriteLine($"{agent.Name} is entering the elevator", agent.ConsoleColor);
                }
            }
        }


        public void Leave(Agent agent) {
            lock (eventIsOccupied) {
                eventIsOccupied.Reset();
                Console.WriteLine($"{agent.Name} is leaving the elevator", agent.ConsoleColor);
            }
        }

        // check elevator status parametres
        public bool IsMoving {
            get { return eventIsMoving.WaitOne(0); }
        }

        public bool IsOccupied {
            get { return eventIsOccupied.WaitOne(0); }
        }
    }
}
