using System;
using System.Drawing;
using System.Threading;
using Console = Colorful.Console;

namespace BlackMesa {
    class Agent {
        public string Name { get; }
        public Color ConsoleColor { get; }
        public Clearance Clearance { get; }
        public Floor CurrentFloor { get; set; }
        public Elevator Elevator { get; }

        ManualResetEvent eventLeftWork = new ManualResetEvent(false);
        Random rand = new Random();

        public Agent(string name, Color color, Clearance clearance, Elevator elevator) {
            Name = name;
            ConsoleColor = color;
            Clearance = clearance;
            CurrentFloor = Floor.G;
            Elevator = elevator;
        }

        public bool LeftWork {
            get {
                return eventLeftWork.WaitOne(0);
            }
        } 

        public void GoToWork() {
            while(!eventLeftWork.WaitOne(0)) {
                // keep calling the elevator until success
                while (Elevator.CurrentFloor != CurrentFloor ) {
                    Elevator.Call(this, CurrentFloor);
                }

                if(!Elevator.IsOccupied) {
                    Elevator.Enter(this);
                    Thread.Sleep(500);
                    EnterElevator();
                }
                else {
                    Console.WriteLine($"Elevator is occupied, {Name} cannot enter.");
                    Thread.Sleep(1000);
                }
            }
        }

        private void EnterElevator() {
            while (true) {
                var floorToGo = GetRandomFloor();
                // must pick a different floor than the current one
                while (floorToGo == CurrentFloor) {
                    floorToGo = GetRandomFloor();
                }

                Elevator.Call(this, floorToGo);

                while(Elevator.IsMoving) {
                    // wait until the elevator arrives at the floor
                }

                if((int)Clearance < (int) Elevator.CurrentFloor) {
                    Console.WriteLine($"{Name} cannot leave on floor {Elevator.CurrentFloor} due to low clearance level", ConsoleColor);
                }
                else {
                    Elevator.Leave(this);
                    CurrentFloor = Elevator.CurrentFloor;
                    Thread.Sleep(500);
                    
                    if(CurrentFloor == Floor.G) {
                        // if agent has decided to go to ground floor
                        // that means the agent has decided to call it a day
                        Console.WriteLine($"{Name} has left work", ConsoleColor);
                        eventLeftWork.Set();
                    }
                    return;
                }
            }
        }

        private Floor GetRandomFloor() {
            // 10 percent for picking ground floor
            // 30 percent for all other floors
            int val = rand.Next(10);

            if(val == 0 ) {
                return Floor.G;
            }
            else if (val < 4) {
                return Floor.S;
            }
            else if (val < 7) {
                return Floor.T1;
            }
            else {
                return Floor.T2;
            }
        }
    }
}
