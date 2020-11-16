using System.Drawing;
using System.Threading;
using Console = Colorful.Console;

namespace BlackMesa {
    class Program {
        static void Main() {
            // Introduction();

            Elevator elevator = new Elevator();
            Agent Gordon = new Agent("Dr. Gordon Freeman", Color.Orange, Clearance.TopSecret, elevator);
            Agent Eli = new Agent("Dr. Eli Vance", Color.Lime, Clearance.Secret, elevator);

            Thread t1 = new Thread(Gordon.GoToWork);
            Thread t2 = new Thread(Eli.GoToWork);
            t1.Start();
            t2.Start();

            while (!Gordon.LeftWork.WaitOne(0) || !Eli.LeftWork.WaitOne(0)) { }
            
            Console.WriteLine("Everyone left work", Color.Red);
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }


        static void Introduction() {
            
            BlackMesaLogo();
            Console.Clear();

            Console.WriteLine("Welcome to Black Mesa Elevator Simulator!");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Here you can observe our special agents going up and\n" +
                              "down the elevator until they decide to leave. Black\n" +
                              "Mesa, as we've built it here, has different floors each\n" +
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
                              "the elevator is initially, too. Somebody\n" +
                              "gets inside and goes to a random floor.\n" +
                              "If their security clearance is not enough,\n" +
                              "the doors won't open and they have to choose\n" +
                              "another level or somebody else might be faster\n" +
                              "and call the elevator on another floor. If they\n" +
                              "can leave on this floor, they do it.\n");

            Console.WriteLine("Press ENTER to begin the simulation!");
            Console.ReadLine();
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
