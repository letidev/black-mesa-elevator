using System;
using System.Drawing;
using System.Threading;

namespace BlackMesa {
    class Agent {
        public string Name { get; }
        public Color ConsoleColor { get; }
        public Clearance Clearance { get; }
        public Floor CurrentFloor { get; set; }
        public Elevator Elevator { get; }

        public ManualResetEvent LeftWork = new ManualResetEvent(false);
        public ManualResetEvent InElevator = new ManualResetEvent(false);
        public ManualResetEvent HasWorked = new ManualResetEvent(false);

        public Agent(string name, Color color, Clearance clearance, Elevator elevator) {
            Name = name;
            ConsoleColor = color;
            Clearance = clearance;
            CurrentFloor = Floor.G;
            Elevator = elevator;
        }

        public void GoToWork() {
            Elevator.Print($"{Name} has arrived to work.", Color.Pink, 100);
            while (!LeftWork.WaitOne(0)) {
                if (!InElevator.WaitOne(0)) {
                    Elevator.Occupy(this);  
                }

                // wait for a random period of time between 3 and 5 seconds
                // before attempting to call the elevator again
                Random rand = new Random();
                int time = 3 + rand.Next(3);
                Thread.Sleep(time*1000);
            }
        }

        public void GoHome() {

            Elevator.Print($"{Name} has called it a day.", Color.Red, 100);

            LeftWork.Set();
        }

        public bool CanLeaveAtFloor(Floor floor) {
            if ((int)Clearance < (int)floor) {
                Elevator.Print($"{Name} cannot leave on floor {floor} due to low clearance level", ConsoleColor, 100);
                return false;
            }
            return true;
        }

        public Floor GetRandomFloor() {
            Random rand = new Random();
            Floor newFloor;
            int val;
            // percentages are 30, 20, 30, 20
            do {
                val = rand.Next(10);
                if (val < 3) {
                    newFloor = Floor.G;
                }
                else if (val < 5) {
                    newFloor = Floor.S;
                }
                else if (val < 8) {
                    newFloor = Floor.T1;
                }
                else {
                    newFloor = Floor.T2;
                }
            }
            while (newFloor == CurrentFloor);

            return newFloor;
        }
    }
}
