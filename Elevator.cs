using System;
using System.Drawing;
using System.Threading;
using Console = Colorful.Console;

namespace BlackMesa {
    class Elevator {
        private Floor CurrentFloor { get; set; }

        // set - agent inside
        // released - empty
        private ManualResetEvent eventIsOccupied;

        private Agent CurrentAgent;

        // miscelaneous props
        private Color ConsoleColor = Color.White;
        private object locker = new object();
        private object consoleLocker = new object();

        public Elevator() {
            CurrentFloor = Floor.G;
            eventIsOccupied = new ManualResetEvent(false);
            CurrentAgent = null;
        }

        public void Occupy(Agent agent) {
            if (!eventIsOccupied.WaitOne(0)) {
                lock (locker) {
                    eventIsOccupied.Set();
                    CurrentAgent = agent;
                    CurrentAgent.InElevator.Set();
                    CurrentAgent.HasWorked.Set();
                    Call();
                }
            }
            else {
                Print($"{agent.Name} cannot call the elevator as it is currently occupied by {CurrentAgent.Name}", agent.ConsoleColor, 1000);
                return;
            }
        }

        private void Call() {
            if (CurrentFloor != CurrentAgent.CurrentFloor) {
                Print($"{CurrentAgent.Name} called the elevator to floor {CurrentAgent.CurrentFloor.ToString()}.", CurrentAgent.ConsoleColor, 200);
                
                CloseDoor();
                Move(CurrentAgent.CurrentFloor);
                OpenDoor();
            }
            Enter();
        }

        private void Enter() {
            Print($"{CurrentAgent.Name} is entering the elevator", CurrentAgent.ConsoleColor, 500);
            GoToFloor(CurrentAgent.GetRandomFloor());
        }

        private void GoToFloor(Floor floor) {
            Print($"{CurrentAgent.Name} chose to go to floor {floor.ToString()}.", CurrentAgent.ConsoleColor, 100);
            CloseDoor();
            Move(floor);
            Print($"{CurrentAgent.Name} has arrived at floor {CurrentFloor}", CurrentAgent.ConsoleColor, 100);
            CurrentAgent.CurrentFloor = CurrentFloor;

            if (CurrentAgent.CanLeaveAtFloor(CurrentFloor)) {
                OpenDoor();
                Leave();
            }
            else {
                GoToFloor(CurrentAgent.GetRandomFloor());
            }
        }

        private void Leave() {
            Print($"{CurrentAgent.Name} is leaving the elevator at floor {CurrentFloor}", CurrentAgent.ConsoleColor, 200);
            CurrentAgent.InElevator.Reset();
            if (CurrentFloor == Floor.G) {
                CurrentAgent.GoHome();
            }
            CurrentAgent = null;
            eventIsOccupied.Reset();

        }

        // door actions and movement
        private void OpenDoor() {
            Print("Door is opening", ConsoleColor, 2000);
        }

        private void CloseDoor() {
            Print("Door is closing", ConsoleColor, 2000);
        }

        private void Move(Floor floor) {
            int travelTime = Math.Abs(CurrentFloor - floor) * 2000;
            Print($"Elevator is moving from floor {CurrentFloor.ToString()} to {floor.ToString()}.", ConsoleColor, travelTime);
            CurrentFloor = floor;
            Print($"Elevator has arrived at floor {CurrentFloor.ToString()}.", ConsoleColor, 100);
        }

        public void Print(string text, Color color, int sleepTime) {
            lock(consoleLocker) {
                Console.WriteLine(text, color);
                Thread.Sleep(sleepTime);
            }
        }
    }
}
