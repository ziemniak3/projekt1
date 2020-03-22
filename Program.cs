using System;

namespace projekt1
{
    class Program
    {
        static void Main(string[] args)
        {
            string rownanie;  
            double x,xmin, xmax;
            int n;

            rownanie = args[0];
            x=double.Parse(args[1]);
            xmin=double.Parse(args[2]);
            xmax=double.Parse(args[3]);            
            n=int.Parse(args[4]);

            RPN przyklad = new RPN(rownanie,x,xmin,xmax,n);
            //przyklad.naTokeny();
            przyklad.naPostfix();
            Console.WriteLine(przyklad.oblicz());
        }
    }
}
