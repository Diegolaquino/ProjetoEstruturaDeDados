﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoEstruturaDeDados
{
    class Program
    {

        static Action Ok = () => Console.WriteLine("Ok!");
        static Action Nil = () => Console.WriteLine("(nil)");

        static void Main(string[] args)
        {
            int geradorIndiceTransacao = 0;

            var listaDeTransacoes = new Stack<Transacao>();



            var torrePrincipal = new Stack<DicionarioFredis>();

            Console.WriteLine(@"Menu:" + Environment.NewLine +
                           "1 - Adicionando dados:" + Environment.NewLine +

                           "  Add:" + Environment.NewLine +
                           "   Add chave valor." + Environment.NewLine +
                           "  Set:" + Environment.NewLine +
                           "   Set chave valor." + Environment.NewLine +
                           " " + Environment.NewLine +
                           "2 - Pegando dados: " + Environment.NewLine +

                           "   get chave" + Environment.NewLine +
                           " " + Environment.NewLine +
                           "3 - Deletando dados" + Environment.NewLine +

                           "   del chave" + Environment.NewLine +
                           " " + Environment.NewLine +
                           "4 - Retornando quantidade de chaves" + Environment.NewLine +

                           "   count" + Environment.NewLine +
                           " " + Environment.NewLine +
                           "5 - Iniciando uma transação" + Environment.NewLine +

                           "  begin" + Environment.NewLine +
                           " " + Environment.NewLine +
                           "6 - Confirmand todas as operações realizadas" + Environment.NewLine +

                           "  commit" + Environment.NewLine +
                           " " + Environment.NewLine +
                           "7 - Desfazendo todas as operações da última " +
                           "transação" + Environment.NewLine +

                           "   rollback" + Environment.NewLine
                       );

            while (true)
            {
                Console.Write("> ");
                var entrada = Console.ReadLine().Split(" ");

                switch (entrada[0].ToLower())
                {
                    #region set pronto
                    case "set":
                        if (listaDeTransacoes.Count == 0)
                        {
                            Console.WriteLine("Você deve iniciar pelo menos uma transação");
                            break;
                        }

                        var objNovo = new DicionarioFredis(entrada[1], entrada[2], listaDeTransacoes.Peek(), Operacao.Insercao);

                        if (!torrePrincipal.Any(d => d.Chave == objNovo.Chave))
                        {
                            objNovo.Transacao = listaDeTransacoes.Peek();
                            torrePrincipal.Push(objNovo);
                            Ok();
                        }
                        else
                        {
                            Console.WriteLine("Já existe uma chave valor");
                        }

                        break;
                    #endregion

                    #region get pronto
                    case "get":

                        var chaveValor = torrePrincipal.FirstOrDefault(d => d.Chave == entrada[1]);

                        if (chaveValor == null)
                            Nil();
                        else
                            Console.WriteLine("Chave -> " + chaveValor.Chave + " com valor -> " + chaveValor.Valor);

                        break;
                    #endregion

                    #region del
                    case "del":
                        if (torrePrincipal.Where(d => d.Chave == entrada[1]) == null)
                        {
                            Nil();
                            break;
                        }

                        delChave(entrada[1], ref torrePrincipal, listaDeTransacoes.Peek());

                        break;
                    #endregion

                    #region add pronto
                    case "add":

                        if (listaDeTransacoes.Count == 0)
                        {
                            Console.WriteLine("Você deve iniciar pelo menos uma transação");
                            break;
                        }

                        var o = new DicionarioFredis(entrada[1], entrada[2], listaDeTransacoes.Peek(), Operacao.Insercao);

                        if (!torrePrincipal.Any(d => d.Chave == o.Chave))
                        {
                            torrePrincipal.Push(o);
                            Ok();
                        }
                        else
                        {
                            Console.WriteLine("Já existe uma chave valor");
                        }
                        break;
                    #endregion

                    #region count pronto
                    case "count":
                        var totalChaves = torrePrincipal.Where(df => df.Operacao == Operacao.Insercao).Count();
                        Console.WriteLine(totalChaves);
                        break;
                    #endregion


                    case "begin":
                        listaDeTransacoes.Push(new Transacao(geradorIndiceTransacao));
                        geradorIndiceTransacao++;
                        break;
                    case "commit":

                        if (listaDeTransacoes == null)
                        {
                            Console.WriteLine("(no transactions)");
                            break;
                        }

                        //versao 1

                        commitTransacoes(ref torrePrincipal, ref listaDeTransacoes);


                        break;
                    case "rollback":

                        if (listaDeTransacoes == null)
                        {
                            Console.WriteLine("(no transactions)");
                            break;
                        }

                        var ultimaTransacao = listaDeTransacoes.Pop();
                        var temp = new Stack<DicionarioFredis>();
                        var totalTorre = torrePrincipal.Count;

                        for (int i = 0; i < totalTorre; i++)
                        {
                            if(torrePrincipal.Peek().Operacao == Operacao.Exclusao && torrePrincipal.Peek().Transacao.Equals(ultimaTransacao))
                            {
                                var objTemp = torrePrincipal.Pop();
                                objTemp.Transacao = listaDeTransacoes.Peek();
                                objTemp.Operacao = Operacao.Insercao;
                                temp.Push(objTemp);
                            }
                            else if(torrePrincipal.Peek().Operacao == Operacao.Insercao && torrePrincipal.Peek().Transacao.Equals(ultimaTransacao))
                            {
                                torrePrincipal.Pop();
                            }
                            else
                            {
                                temp.Push(torrePrincipal.Pop());
                            }
                            
                        };

                        var totaltemp = temp.Count;

                        for (int i = 0; i < totaltemp; i++)
                        {
                            torrePrincipal.Push(temp.Pop());
                        }

                        listaDeTransacoes.Clear();
                        Console.WriteLine("Ok!(transactions left: 0)");

                        break;
                    default:
                        Console.WriteLine("Comando inválido");
                        break;
                }
            }

        }

        private static void delChave(string chave, ref Stack<DicionarioFredis> torrePrincipal, Transacao ultimaTransacao)
        {
            var temp = new Stack<DicionarioFredis>();
            var total = torrePrincipal.Count;

            if (torrePrincipal.Any(t => t.Chave == chave))
            {
                for (int i = 0; i < total; i++)
                {
                    var obj = torrePrincipal.Pop();

                    if (obj.Chave == chave)
                    {
                        obj.Operacao = Operacao.Exclusao;
                        obj.Transacao = ultimaTransacao;

                        torrePrincipal.Push(obj);

                        var totalTemp = temp.Count;

                        if (totalTemp != 0)
                        {
                            for (int j = 0; j < totalTemp; j++)
                            {
                                torrePrincipal.Push(temp.Pop());
                            }
                        }

                        break;
                    }
                    else
                    {
                        temp.Push(obj);
                    }
                }
            }
            else
            {
                Nil();
            }
        }

        private static void commitTransacoes(ref Stack<DicionarioFredis> torrePrincipal, ref Stack<Transacao> listaDeTransacoes)
        {
            #region versao_1

            //var temp1 = new Stack<DicionarioFredis>();
            //var totalTorre = torrePrincipal.Count;
            //for (int i = 0; i < totalTorre; i++)
            //{
            //    if (torrePrincipal.Peek().Operacao == Operacao.Insercao)
            //    {
            //        temp1.Push(torrePrincipal.Pop());
            //    }
            //    else
            //    {
            //        torrePrincipal.Pop();
            //    }
            //}
            //var totalTemp1 = temp1.Count;
            //for (int i = 0; i < totalTemp1; i++)
            //{
            //    torrePrincipal.Push(temp1.Pop());
            //}

            #endregion

            #region versao_2

            var temp = new Stack<DicionarioFredis>();


            var ultimaTransacao = listaDeTransacoes.Pop();

            while (torrePrincipal.Any(o => o.Transacao.Equals(ultimaTransacao)))
            {
                var obj = torrePrincipal.Pop();
                if ((obj.Operacao == Operacao.Insercao && obj.Transacao.Equals(ultimaTransacao)) || !obj.Transacao.Equals(ultimaTransacao))
                {
                    temp.Push(obj);
                }
            };

            var countTemp = temp.Count;
            if (countTemp != 0)
            {
                for (int i = 0; i < countTemp; i++)
                {
                    torrePrincipal.Push(temp.Pop());
                }
            }

            Console.WriteLine("Ok!(transactions left: {0})", listaDeTransacoes.Count);
            #endregion
        }
    }
}
