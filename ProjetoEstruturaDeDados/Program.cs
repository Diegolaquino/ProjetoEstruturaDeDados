using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoEstruturaDeDados
{
    class Program
    {
        static void Main(string[] args)
        {

            #region PrimeiraVersao
            // var listaDeTransacoes = new Queue<bool>();

            //Queue -> fila
            //var fila = new Queue<DicionarioFredis>();


            var primeiraTorre = new Stack<DicionarioFredis>();
            var segundaTorre = new Stack<DicionarioFredis>();

            //Stack -> pilha
            while (true)
            {
                Console.Write("> ");
                var entrada = Console.ReadLine().Split(" ");

                switch (entrada[0].ToLower())
                {
                    case "set":
                        break;
                    case "get":

                        var chaveValor = primeiraTorre.FirstOrDefault(df => df.Chave == entrada[1]);

                        chaveValor = chaveValor ?? new DicionarioFredis("0", "0");

                        Console.WriteLine(chaveValor.Chave + " " + chaveValor.Valor);
                       
                        break;
                    case "del":
                        break;
                    case "add":
                        primeiraTorre.Push(new DicionarioFredis(entrada[1], entrada[2]));
                        break;
                    case "count":
                        Console.WriteLine(primeiraTorre.Count()); 
                        break;
                    case "begin":
                        break;
                    case "commit":
                        foreach (var item in primeiraTorre)
                        {
                            segundaTorre.Push(item);
                        }

                        primeiraTorre.Clear();
                        break;
                    default:
                        Console.WriteLine("Comando inválido");
                        break;
                }
            }

            #endregion

        }


    }
}
