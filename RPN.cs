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
        List<string> L = new List<string>();

        public void naPostfix(){
            double tmp;

            foreach (string token in this.naTokeny()){
                if(token=="("){
                    S.Push(token);
                }else if (token==")"){
                    while(S.Peek()!="("){
                        Q.Enqueue(S.Pop());
                        S.Pop();
                    }                   
                }else if(D.ContainsKey(token)){
                    while(S.Count>0 && D[token]<=D[S.Peek()]){
                        Q.Enqueue(S.Pop());
                        S.Push(token);
                    }
                }else if(Double.TryParse(token, out tmp) || token=="x"){
                    Q.Enqueue(token);
                }                             
            }

            while (S.Count > 0){
                Q.Enqueue(S.Pop());
            }

            foreach(string tmp1 in Q.ToArray()){
                L.Add(tmp1);
                Console.Write("{0} ",tmp1);
            }
            Console.WriteLine();
        }
       
    }
}