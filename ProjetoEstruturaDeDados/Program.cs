using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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

            Console.WriteLine(@" " + Environment.NewLine + "Menu:" + Environment.NewLine +
                            " " + Environment.NewLine +
                           "0 - Zerar dados para novo teste" + Environment.NewLine +
                           " clear " + Environment.NewLine +
                            " " + Environment.NewLine +
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
                           "6 - Confirmando todas as operações realizadas" + Environment.NewLine +

                           "  commit" + Environment.NewLine +
                           " " + Environment.NewLine +
                           "7 - Desfazendo todas as operações da última " +
                           "transação" + Environment.NewLine +

                           "   rollback" + Environment.NewLine +

                           " " + Environment.NewLine +
                           "8 - Printar valores da pilha " + Environment.NewLine +
                           " print" + Environment.NewLine +

                           " " + Environment.NewLine +
                           "9 - Sair do programa " + Environment.NewLine +
                           " exit"
                       );


            bool naoSair = true;
            while (naoSair)
            {
                Console.Write("> ");
                var entrada = Console.ReadLine().Split(" ");

                switch (entrada[0].ToLower())
                {
                    #region set pronto
                    case "set":

                        try
                        {
                            adicionarValor(entrada, ref torrePrincipal, ref listaDeTransacoes);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro no set: " + e.Message);
                        }

                        break;
                    #endregion

                    #region get pronto
                    case "get":

                        try
                        {
                            var chaveValor = torrePrincipal.FirstOrDefault(d => d.Chave == entrada[1] && d.Operacao != Operacao.Exclusao);

                            if (chaveValor == null)
                                Nil();
                            else
                                Console.WriteLine("Chave -> " + chaveValor.Chave + " com valor -> " + chaveValor.Valor);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro no get: " + e.Message);

                        }

                        break;
                    #endregion

                    #region del
                    case "del":
                        try
                        {
                            if (torrePrincipal.Where(d => d.Chave == entrada[1]) == null)
                            {
                                Nil();
                                break;
                            }
                            if (listaDeTransacoes.Any())
                                delChave(entrada[1], ref torrePrincipal, listaDeTransacoes.Peek());
                            else
                                delChave(entrada[1], ref torrePrincipal);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro no deletar: " + e.Message);
                        }

                        break;
                    #endregion

                    #region add pronto
                    case "add":

                        try
                        {
                            var o = new DicionarioFredis(entrada[1], entrada[2], Operacao.Insercao, listaDeTransacoes.Count == 0 ? null : listaDeTransacoes.Peek());

                            if (!torrePrincipal.Any(d => d.Chave == o.Chave))
                            {
                                torrePrincipal.Push(o);
                                Ok();
                            }
                            else
                            {
                                Console.WriteLine("Já existe uma chave valor");
                                Console.WriteLine("Caso Queira sobrescrever, utilize o set no lugar do add.");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro no add: " + e.Message);
                        }
                        break;
                    #endregion

                    #region count pronto
                    case "count":
                        var totalChaves = torrePrincipal.Where(df => df.Operacao == Operacao.Insercao).Count();
                        Console.WriteLine(totalChaves);
                        break;
                    #endregion

                    #region begin
                    case "begin":
                        listaDeTransacoes.Push(new Transacao(geradorIndiceTransacao));
                        geradorIndiceTransacao++;
                        break;
                    #endregion

                    #region commit
                    case "commit":

                        try
                        {
                            if (listaDeTransacoes == null || listaDeTransacoes.Count == 0)
                            {
                                Console.WriteLine("(no transactions)");
                                break;
                            }

                            commitTransacoes(ref torrePrincipal, ref listaDeTransacoes);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro no commit: " + e.Message);
                        }

                        break;
                    #endregion

                    #region rollback
                    case "rollback":
                        try
                        {
                            if (listaDeTransacoes == null || listaDeTransacoes.Count == 0)
                            {
                                Console.WriteLine("(no transactions)");
                                break;
                            }
                            rollbackStack(ref torrePrincipal, ref listaDeTransacoes);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro no rollback: " + e.Message);
                        }

                        break;
                    #endregion

                    #region print pronto
                    case "print":
                        try
                        {
                            if (torrePrincipal.Count == 0)
                            {
                                Console.WriteLine("Não há dados");
                            }
                            else
                            {
                                foreach (var item in torrePrincipal)
                                {
                                    if (item.Operacao == Operacao.Exclusao)
                                        Console.WriteLine("Item chave {0} valor {1} excluído", item.Chave, item.Valor);
                                    else
                                        Console.WriteLine("Item chave {0} valor {1}", item.Chave, item.Valor);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Erro no print: " + e.Message);
                        }

                        break;
                    #endregion
                    case "clear":
                        listaDeTransacoes.Clear();
                        torrePrincipal.Clear();
                        break;

                    case "exit":
                        naoSair = false;
                        Console.WriteLine("O programa será desligado...");
                        Thread.Sleep(2000);
                        break;
                    default:
                        Console.WriteLine("Comando inválido");
                        break;
                }
            }

        }

        private static void adicionarValor(string[] entrada, ref Stack<DicionarioFredis> torrePrincipal, ref Stack<Transacao> listaDeTransacoes)
        {

            if (!torrePrincipal.Any(d => d.Chave == entrada[1]))
            {
                var objNovo = new DicionarioFredis(entrada[1], entrada[2], Operacao.Insercao, listaDeTransacoes.Count == 0 ? null : listaDeTransacoes.Peek());
                torrePrincipal.Push(objNovo);
                Ok();
            }
            else
            {
                adicionaValorQueJaExiste(entrada, ref torrePrincipal, ref listaDeTransacoes);
            }
        }

        private static void adicionaValorQueJaExiste(string[] entrada, ref Stack<DicionarioFredis> torrePrincipal, ref Stack<Transacao> listaDeTransacoes)
        {
            foreach (var item in torrePrincipal)
            {
                if (item.Chave == entrada[1])
                {
                    item.Valor = entrada[2];

                    if (listaDeTransacoes.Count > 0)
                    {
                        item.Transacao = listaDeTransacoes.Peek();
                        item.Historico.Push(new Registro(listaDeTransacoes.Peek(), item.Operacao, item.Valor));
                    }
                    else
                    {
                        var historico = item.Historico.Pop();
                        historico.valor = entrada[2];
                        item.Historico.Push(historico);
                    }
                }
            }
        }

        private static void delChave(string chave, ref Stack<DicionarioFredis> torrePrincipal, Transacao ultimaTransacao = null)
        {
            var temp = new Stack<DicionarioFredis>();

            if (ultimaTransacao == null)
            {
                foreach (var item in torrePrincipal)
                {
                    if (item.Chave != chave)
                        temp.Push(item);
                }

                torrePrincipal.Clear();

                foreach (var item in temp)
                {
                    torrePrincipal.Push(item);
                }
            }
            else
            {
                foreach (var item in torrePrincipal)
                {
                    if (item.Chave == chave)
                    {
                        item.Operacao = Operacao.Exclusao;
                        item.Transacao = ultimaTransacao;
                        item.Historico.Push(new Registro(ultimaTransacao, item.Operacao, item.Valor));
                    }
                }
            }

        }

        private static void commitTransacoes(ref Stack<DicionarioFredis> torrePrincipal, ref Stack<Transacao> listaDeTransacoes)
        {
            var ultimaTransacao = listaDeTransacoes.Peek();

            foreach (var item in torrePrincipal)
            {
                if (item.Transacao != null && item.Transacao.Equals(ultimaTransacao))
                {
                    // seta transacao null, porque depois de commitar volta ao estagio inicial sem transação
                    item.Transacao = null;


                    var countHistorico = item.Historico.Count;

                    // elimina o histórico de transações relativos a ultima transação do commit 
                    for (int i = 0; i < countHistorico; i++)
                    {
                        item.Historico.Pop();
                    }
                }
            }

            listaDeTransacoes.Pop();
        }

        private static void rollbackStack(ref Stack<DicionarioFredis> torrePrincipal, ref Stack<Transacao> listaDeTransacoes)
        {
            if (listaDeTransacoes.Count > 0)
            {
                var temp = new Stack<DicionarioFredis>();

                for (int i = 0; i < torrePrincipal.Count; i++)
                {
                    var obj = torrePrincipal.Pop();

                    if (obj.Transacao != null && obj.Transacao.Equals(listaDeTransacoes.Peek()) && obj.Historico.Count == 1)
                    {
                        continue;
                    }

                    if (obj.Transacao != null && obj.Transacao.Equals(listaDeTransacoes.Peek()) && obj.Historico.Count > 1)
                    {
                        obj.Historico.Pop();
                        obj.Operacao = obj.Historico.Peek().operacao;
                        obj.Valor = obj.Historico.Peek().valor;
                        obj.Transacao = obj.Historico.Peek().transacao;
                    }

                    temp.Push(obj);
                }

                var count = temp.Count;
                for (int i = 0; i < count; i++)
                {
                    torrePrincipal.Push(temp.Pop());
                }

                listaDeTransacoes.Pop();
            }
            else
            {
                Console.WriteLine("Ok!(transactions left: {0})", listaDeTransacoes.Count);
            }


        }


    }
}
