using System;
using Tank;

namespace Tank2
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TankGame())
                game.Run();
        }
    }
}
