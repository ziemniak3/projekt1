using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace projekt1
{
    public class RPN
    {
        string rownanie;  
        double x, xmin, xmax;
        int n;

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
                Console.Write("{0} ",token);
                i++;
            }
            Console.WriteLine();
            return tabTokenow;
        }

        Dictionary<string,int> D = new Dictionary<string,int>(){
            {"abs",4},{"cos",4},{"exp",4},{"log",4},{"sin",4},{"sqrt",4},{"tan",4},{"tanh",4},{"acos",4},{"asin",4},{"atan",4},
            {"^",3},
            {"*",2},{"/",2},
            {"+",1},{"-",1},
            {"(",0}
        };
        
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
                    double a =S1.Pop();
                    if(D[token]==4){
                        if(token=="abs") a = Math.Abs(a);
                        else if(token=="cos") a = Math.Cos(a);
                        else if(token=="exp") a = Math.Exp(a);
                        else if(token=="log") a = Math.Log(a);
                        else if(token=="sin") a = Math.Sin(a);
                        else if(token=="sqrt") a = Math.Sqrt(a);
                        else if(token=="tan") a = Math.Tan(a);
                        else if(token=="cosh") a = Math.Cosh(a);
                        else if(token=="sinh") a = Math.Sinh(a);
                        else if(token=="tanh") a = Math.Tanh(a);
                        else if(token=="acos") a = Math.Acos(a);
                        else if(token=="asin") a = Math.Asin(a);
                        else if(token=="atan") a = Math.Atan(a);
                    }
                    else{
                        double b = S1.Pop();
                        if(token=="+") a += b;
                        else if(token=="-") a = b-a;
                        else if(token=="*") a *= b;
                        else if(token=="/") a = b/a;
                        else if(token=="^") a = Math.Pow(b,a);
                    }
                    S1.Push(a);
                }
            }
            return S1.Pop();
        }
       
    }
}