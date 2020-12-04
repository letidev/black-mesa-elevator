using System.Drawing;
using System.Threading;
using Console = Colorful.Console;

namespace BlackMesa {
    class Program {

        static Elevator elevator;
        static ManualResetEvent elevatorStarted = new ManualResetEvent(false);
        static ManualResetEvent everyoneLeftWork = new ManualResetEvent(false);

        static void StartAgents() {
            // start the agent threads only after the elevator thread 
            // has started working otherwise they cannot be instantiated
            elevatorStarted.WaitOne();
            
            Thread[] threads = new Thread[3];
            Agent[] agents = new Agent[3];
            agents[0] = new Agent("Dr. Gordon Freeman", Color.Orange, Clearance.TopSecret, elevator);
            agents[1] = new Agent("Dr. Eli Vance", Color.Lime, Clearance.Secret, elevator);
            agents[2] = new Agent("Barney Calhoun", Color.Cyan, Clearance.Confidential, elevator);

            for (int i = 0; i < 3; i++) {
                threads[i] = new Thread(agents[i].GoToWork);
                threads[i].Priority = ThreadPriority.AboveNormal;
            }

            foreach (var t in threads) {
                t.Start();
            }

            foreach (var t in threads) {
                t.Join();
            }

            // everyone has left so the elevator
            // can stop working too
            everyoneLeftWork.Set();
            Console.WriteLine("Everyone left work", Color.Red);
        }

        static void ElevatorThreadWorker() {
            Console.WriteLine("Elevator started working.", Color.Pink);
            
            elevator = new Elevator();
            elevatorStarted.Set();

            // while there are agents at work
            // the elevator will be working
            while (!everyoneLeftWork.WaitOne(0)) {
                // if an agent has "occupied" the elevator
                // the whole chain of actions is started
                if(elevator.eventIsOccupied.WaitOne(0) ) {
                    elevator.Call();
                }
            }

            Console.WriteLine("Elevator stopped working for today.", Color.Red);
        }

        static void Main() {
            // Introduction();

            Thread elevatorThread = new Thread(ElevatorThreadWorker);
            elevatorThread.Start();
            
            StartAgents();
            elevatorThread.Join();
            
            Console.WriteLine("Press ENTER to exit.", Color.White);
            Console.ReadLine();
        }

        static void Introduction() {
            
            BlackMesaLogo();
            Console.Clear();

            Console.WriteLine("Welcome to Black Mesa Elevator Simulator!");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Here you can observe our special agents going up and\n" +
                              "down the elevator until they decide to leave. Black\n" +
                              "Mesa, as we've built it here, has different floors, each\n" +
                              "with different security clearance.\n");
            Console.WriteLine("The floors of the base are as follows:\n" +
                "  - G  - ground floor\n" +
                "  - S  - secret floor with nuclear weapons\n" +
                "  - T1 - secret floor with experimemtal weapons\n" +
                "  - T2 - top-secret floor that stores alien remains\n");

            Console.WriteLine("Our scientists and their security clearances are:");
            Console.WriteLine("  - Barney Calhoun - confidential - level G", Color.Cyan);
            Console.WriteLine("  - Dr. Eli Vance  - secret - levels G and S", Color.Lime);
            Console.WriteLine("  - Dr. Gordon Freeman - top-secret - levels G, S, T1 and T2", Color.Orange);

            Console.WriteLine("At the beginning of each day all our scientists\n" +
                              "start at the ground floor - that's where\n" +
                              "the elevator initially is, too. Somebody\n" +
                              "gets inside and goes to a random floor.\n" +
                              "If their security clearance is not enough,\n" +
                              "the doors won't open and they have to choose\n" +
                              "another floor. If they can leave on this floor,\n" +
                              "they do it. An agent goes home if they've been to\n" +
                              "at least one floor other than G and then return to G.\n");

            Console.WriteLine("Press ENTER to begin the simulation!");
            Console.ReadLine();
            Console.Clear();
        }

        static void BlackMesaLogo() {
            LogoRow("            .-;$HHHHHHX$+;-.");
            LogoRow("        ,;X@@X%/;=----=:/%X@@X/,");
            LogoRow("      =$@@%=.              .=+H@X:");
            LogoRow("    -XMX:                      =XMX=");
            LogoRow("   /@@:                          =H@+");
            LogoRow("  %@X,                            .$@$");
            LogoRow(" +@X.                               $@%");
            LogoRow("-@@,                                .@@=");
            LogoRow("%@%                                  +@$");
            LogoRow("H@:                                  :$H");
            LogoRow("H@:         :HHHHHHHHHHHHHHHHHHX,    =@H");
            LogoRow("%@%         ;@M@@@@@@@@@@@@@@@@@H-   +@$");
            LogoRow("=@@,        :@@@@@@@@@@@@@@@@@@@@@= .@@:");
            LogoRow(" +@X        :@@@@@@@@@@@@@@@M@@@@@@:%@%");
            LogoRow("  $@$,      ;@@@@@@@@@@@@@@@@@M@@@@@@$.");
            LogoRow("   +@@HHHHHHH@@@@@@@@@@@@@@@@@@@@@@@+");
            LogoRow("    =X@@@@@@@@@@@@@@@@@@@@@@@@@@@@X=");
            LogoRow("      :$@@@@@@@@@@@@@@@@@@@M@@@@$:");
            LogoRow("        ,;@@@@@@@@@@@@@@@@@@@X/-");
            LogoRow("           .-;+$XXHHHHHX$+;-.");

            Thread.Sleep(500);
        }

        static void LogoRow(string row) {
            Console.WriteLine(row, Color.Orange);
            Thread.Sleep(500);
        }
    }
}
