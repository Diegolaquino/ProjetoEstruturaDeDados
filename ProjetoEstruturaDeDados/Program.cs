using System;
using System.Collections.Generic;

namespace ProjetoEstruturaDeDados
{
    class Program
    {
        static void Main(string[] args)
        {
            bool transacao = false;
            //Queue -> fila
            var dicionarioFredis = new Dictionary<string, string>();

            //Stack<String> teste = new Stack<string>();
            //teste.

            //Stack -> pilha
            while (true)
            {
                Console.Write("> ");
                var entrada = Console.ReadLine().Split(" ");

                switch (entrada[0].ToLower())
                {
                    case "set":
                        if (dicionarioFredis.ContainsKey(entrada[1]))
                        {
                            dicionarioFredis[entrada[1]] = entrada[2];
                        }
                        else
                        {
                            dicionarioFredis.Add(entrada[1], entrada[2]);
                        }

                        Console.WriteLine("Ok!");

                        break;
                    case "get":
                        if(dicionarioFredis.ContainsKey(entrada[1]))
                        {
                            Console.WriteLine(dicionarioFredis[entrada[1]]);
                        }
                        else
                        {
                            Console.WriteLine("(nil)");
                        }
                        break;
                    case "del":
                        dicionarioFredis.Remove(entrada[1]);
                        Console.WriteLine("Ok!");
                        break;
                    case "add":
                        dicionarioFredis.Add(entrada[1].ToLower(), entrada[2].ToLower());
                        break;
                    case "count":
                        Console.WriteLine(dicionarioFredis.Count);
                        break;
                    case "begin":
                        transacao = true;
                        break;
                    default:
                        Console.WriteLine("Comando inválido");
                        break;
                }
            }

        }

        
    }
}
