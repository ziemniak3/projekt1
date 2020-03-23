using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace projekt1
{
    public class RPN
    {
        string rownanie;  
        double x, xmin, xmax;
        int n,minus;       

        public RPN (string rownanie, double x, double xmin, double xmax, int n){
            this.rownanie=rownanie;
            this.x=x;
            this.xmin=xmin;
            this.xmax=xmax;
            this.n=n;
        }

        public string[] naTokeny(){
            Regex rx = new Regex (@"\+|\-|\*|\/|\(|\)|\^|(x)|(abs)|(cos)|(exp)|(log)|(sin)|(sqrt)|(tan)|(cosh)|(sinh)|(tanh)|(acos)|(asin)|(atan)|((\d*)(\.)?(\d+))");
            MatchCollection tokeny = rx.Matches(this.rownanie);            
            string[] tabTokenow = new string[tokeny.Count];            
            int i=0;

            foreach (Match token in tokeny){
                tabTokenow[i]= token.Value;                
                i++;
            }

            if(tabTokenow[0]=="-" && tabTokenow[1]=="("){
                this.minus=1;
                tabTokenow[0]="";
            }else if(tabTokenow[0]=="-" && tabTokenow[1]!="("){
                Console.WriteLine("Bledne wyrazenie, cos z minusem!");
                Environment.Exit(0);
            }

            return tabTokenow;
        }

        public void wyswietlTokeny(){
            foreach (string token in this.naTokeny()){               
                Console.Write("{0} ",token);                
            }
            Console.WriteLine();
        }

        Dictionary<string,int> D = new Dictionary<string,int>(){
            {"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},{"tan",4},{"tanh",4},{"acos",4},{"asin",4},{"atan",4},
            {"^",3},
            {"*",2},{"/",2},
            {"+",1},{"-",1},
            {"(",0}
        };

        public void walidacja(){                        
            int i=0;
            int wagaL=0;
            int nawias=0;

            foreach(string token in this.naTokeny()){
                if(D.ContainsKey(token)){
                    if(i==0){
                        wagaL=D[token];
                        i++;
                    }else if(i!=0){
                        if(wagaL==D[token]){
                            Console.WriteLine("Bledne wyraznie, dwa operatory matematyczne obok siebie !");
                            Environment.Exit(0);
                        }
                        wagaL=D[token];                        
                    }                    
                }                
            }

            foreach (string token in this.naTokeny()){
                if(token=="(") nawias++;
                else if(token==")") nawias--;
            }
            if(nawias!=0){
                Console.WriteLine("Bledne wyraznie,cos z nawiasami !");
                Environment.Exit(0);
            }                 
        }
        
        public Stack<string> S = new Stack<string>();
        public Queue<string> Q = new Queue<string>();
        List<string> R = new List<string>();

        public void naPostfix(){
            double tmp;

            foreach (string token in this.naTokeny()){
                if(token=="("){
                    S.Push(token);
                }else if (token==")"){
                    while(S.Peek()!="("){
                        Q.Enqueue(S.Pop());
                    }
                    S.Pop();                                   
                }else if(D.ContainsKey(token)){
                    while(S.Count>0 && D[token]<=D[S.Peek()]){
                        Q.Enqueue(S.Pop());
                    }
                    S.Push(token);                    
                }else if(Double.TryParse(token, out tmp) || token=="x"){
                    Q.Enqueue(token);
                }                             
            }

            while (S.Count > 0){
                Q.Enqueue(S.Pop());
            }

            foreach(string tmp1 in Q.ToArray()){
                R.Add(tmp1);
                Console.Write("{0} ",tmp1);
            }
            
            Console.WriteLine();
        }

        public double oblicz(){
            double tmp2;
            Stack <double> S1 = new Stack<double>();

            foreach(string token in R){
                if(Double.TryParse(token, out tmp2)){
                    S1.Push(tmp2);
                }
                else if (token=="x"){
                    S1.Push(this.x);
                }
                else if(D.ContainsKey(token)){
                    double a=S1.Pop();
                    if(D[token]==4){
                        switch(token){
                            case "abs":
                                a=Math.Abs(a);
                                break;
                            case "cos":
                                a=Math.Cos(a);
                                break;
                            case "exp":
                                a=Math.Exp(a);
                                break;
                            case "log":
                                a=Math.Log(a);
                                break;
                            case "sin":
                                a=Math.Sin(a);
                                break;
                            case "sqrt":
                                a=Math.Sqrt(a);
                                break;
                            case "tan":
                                a=Math.Tan(a);
                                break;
                            case "cosh":
                                a=Math.Cosh(a);
                                break;
                            case "sinh":
                                a=Math.Sinh(a);
                                break;
                            case "tanh":
                                a=Math.Tanh(a);
                                break;
                            case "acos":
                                a=Math.Acos(a);
                                break;
                            case "asin":
                                a=Math.Asin(a);
                                break;
                            case "atan":
                                a=Math.Atan(a);
                                break;
                        }
                    }
                    else{
                        double b=S1.Pop();
                        switch(token){
                            case "+":
                                a=a+b;
                                break;
                            case "-":
                                a=b-a;
                                break;
                            case "*":
                                a=a*b;
                                break;
                            case "/":
                                if (a==0){
                                    Console.WriteLine("Blad, nie mozna dzielic przez 0!");
                                    Environment.Exit(0);
                            }
                                break;
                        } 
                    }
                    S1.Push(a);
                }
            }
            double wynik=S1.Pop();

            if(this.minus==1) wynik=wynik*(-1);

            return wynik;
        }

       public void obliczPrzedzial(){
           double skok = (this.xmax-this.xmin)/(this.n-1);
           this.x = this.xmin;

           for(int i =0;i<n;i++){
               Console.WriteLine("{0} => {1}",this.x,this.oblicz());
               this.x += skok;
           }
       }    
    }
}