using System.Drawing;
using Console = Colorful.Console;

namespace Area51 {
    class Program {
        static void Main() {
            Introduction();
        }




        static void Introduction() {
            Console.WriteLine("            .-;$HHHHHHX$+;-.\n" +
                              "        ,;X@@X%/;=----=:/%X@@X/,\n" +
                              "      =$@@%=.              .=+H@X:\n" +
                              "    -XMX:                      =XMX=\n" +
                              "   /@@:                          =H@+\n" +
                              "  %@X,                            .$@$\n" +
                              " +@X.                               $@%\n" +
                              "-@@,                                .@@=\n" +
                              "%@%                                  +@$\n" +
                              "H@:                                  :$H\n" +
                              "H@:         :HHHHHHHHHHHHHHHHHHX,    =@H\n" +
                              "%@%         ;@M@@@@@@@@@@@@@@@@@H-   +@$\n" +
                              "=@@,        :@@@@@@@@@@@@@@@@@@@@@= .@@:\n" +
                              " +@X        :@@@@@@@@@@@@@@@M@@@@@@:%@%\n" +
                              "  $@$,      ;@@@@@@@@@@@@@@@@@M@@@@@@$.\n" +
                              "   +@@HHHHHHH@@@@@@@@@@@@@@@@@@@@@@@+\n" +
                              "    =X@@@@@@@@@@@@@@@@@@@@@@@@@@@@X=\n" +
                              "      :$@@@@@@@@@@@@@@@@@@@M@@@@$:\n" +
                              "        ,;@@@@@@@@@@@@@@@@@@@X/-\n" +
                              "           .-;+$XXHHHHHX$+;-.\n", Color.Orange);

            Console.WriteLine("Welcome to Black Mesa Elevator Simulator!");
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Here you can observe our special agents going up and\n" +
                              "down the elevator until they decide to leave. Black\n" +
                              "Mesa, as we've built it here, has different levels each\n" +
                              "with different secirity clearance.\n");
            Console.WriteLine("The levels of the base are as follows:\n" +
                "  - G  - ground floor\n" +
                "  - S  - secret floor with nuclear weapons\n" +
                "  - T1 - secret floor with experimemtal weapons\n" +
                "  - T2 - top-secret floor that stores alien remains\n");

            Console.WriteLine("Our agents and their security clearances are:\n" +
                "  - Barney Calhoun - confidential - level G\n" +
                "  - Dr. Eli Vance  - secret - levels G and S\n" +
                "  - Dr. Gordon Freeman - top-secret - levels G, S, T1 and T2\n");
            Console.ReadLine();
        }
    }
}
