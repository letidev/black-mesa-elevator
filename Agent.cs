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
            while (!LeftWork.WaitOne(0)) {
                Elevator.Occupy(this);
                if (InElevator.WaitOne(0)) {
                    InElevator.WaitOne();
                }
                Thread.Sleep(2000);
            }
        }

        public void GoHome() {
            Console.WriteLine($"{Name} has called it a day.", Color.Red);
            LeftWork.Set();
        }

        public bool CanLeaveAtFloor(Floor floor) {
            if ((int)Clearance < (int)floor) {
                Console.WriteLine($"{Name} cannot leave on floor {floor} due to low clearance level", ConsoleColor);
                return false;
            }
            return true;
        }

        public Floor GetRandomFloor() {
            // 10 percent for picking ground floor
            // 30 percent for all other floors
            Random rand = new Random();
            Floor newFloor;
            int val;
            do {
                val = rand.Next(10);
                if (val == 0) {
                    newFloor = Floor.S;
                }
                else if (val < 4) {
                    newFloor = Floor.G;
                }
                else if (val < 7) {
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
