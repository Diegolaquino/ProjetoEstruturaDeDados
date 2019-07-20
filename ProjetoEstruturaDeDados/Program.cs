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
             var listaDeTransacoes = new Stack<Transacao>();

            //Queue -> fila
            //var fila = new Queue<DicionarioFredis>();

            //var teste = new List<Stack<DicionarioFredis>>();
            //var dicionarioCentral = new Dictionary<>();

            Action Ok = () => Console.WriteLine("Ok!");
            Action Nil = () => Console.WriteLine("(nil)");


            var primeiraTorre = new Stack<DicionarioFredis>();
            var segundaTorre = new Stack<DicionarioFredis>();
            var terceiraTorre = new Stack<DicionarioFredis>();
            
            while (true)
            {
                Console.Write("> ");
                var entrada = Console.ReadLine().Split(" ");

                switch (entrada[0].ToLower())
                {
                    #region set pronto
                    case "set":
                        if(listaDeTransacoes.Count == 0)
                        {
                            Console.WriteLine("Você deve iniciar pelo menos uma transação");
                            break;
                        }

                        var objNovo = new DicionarioFredis(entrada[1], entrada[2]);

                        if (!primeiraTorre.Any(d => d.Chave == objNovo.Chave))
                        {
                            objNovo.Transacao = listaDeTransacoes.Peek();
                            primeiraTorre.Push(objNovo);
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

                        var chaveValor = primeiraTorre.FirstOrDefault(d => d.Chave == entrada[1]);

                        if (chaveValor == null)
                            Nil();
                        else
                            Console.WriteLine(chaveValor.Chave + " " + chaveValor.Valor);
                       
                        break;
                    #endregion

                    #region del pronto
                    case "del":
                        
                        if (primeiraTorre.Count <= 0 || primeiraTorre == null)
                        {
                            Console.WriteLine("Chave inexistente ou já excluída");
                        }
                        else
                        {
                            var contador = primeiraTorre.Count;
                            for (int i = 0; i < contador; i++)
                            {
                                if(!(primeiraTorre.Peek().Chave == entrada[1]))
                                {
                                    segundaTorre.Push(primeiraTorre.Pop());
                                }
                                else
                                {
                                    var objExcluido = primeiraTorre.Pop();
                                    objExcluido.Operacao = Operacao.Exclusao;
                                    primeiraTorre.Push(objExcluido);
                                    break;
                                }
                            }
                            if(segundaTorre.Count > 0)
                            {
                                var count = segundaTorre.Count;
                                for (int i = 0; i < count; i++)
                                {
                                    primeiraTorre.Push(segundaTorre.Pop());
                                }
                            }
                            Ok();
                        }
                        break;
                    #endregion

                    #region add pronto
                    case "add":
                        var o = new DicionarioFredis(entrada[1], entrada[2]);

                        if (!primeiraTorre.Any(d => d.Chave == o.Chave))
                        {
                            primeiraTorre.Push(o);
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
                        Console.WriteLine(primeiraTorre.Where(p => p.Operacao != Operacao.Exclusao).Count());
                        break;
                    #endregion

                    
                    case "begin":
                        if(listaDeTransacoes.Count > 0)
                        {
                            var objAtualizado = listaDeTransacoes.Pop();
                            objAtualizado.PodeSerExcluida = false;
                            listaDeTransacoes.Push(objAtualizado);
                        }
                       
                       listaDeTransacoes.Push(new Transacao());
                       Console.WriteLine("Transação Aberta");
                        
                        break;
                    case "commit":

                        if (listaDeTransacoes.Count == 0)
                        {
                            Console.WriteLine("Não há transação aberta para fazer commit");
                            break;
                        }

                        var c = primeiraTorre.Count;
                        segundaTorre.Clear();
                        for (int i = 0; i < c; i++)
                        {
                            if(primeiraTorre.Peek().Operacao != Operacao.Exclusao)
                            {
                                segundaTorre.Push(primeiraTorre.Pop());
                            }
                        }

                        //Zerar primeira torre
                        primeiraTorre.Clear();

                        var countStorre = segundaTorre.Count;
                        for(int i = 0; i < countStorre; i++)
                        {
                            terceiraTorre.Push(segundaTorre.Pop());
                        }

                        Ok();

                        break;
                    case "rollback":

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
