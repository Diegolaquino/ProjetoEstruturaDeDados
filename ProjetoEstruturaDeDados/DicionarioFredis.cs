using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEstruturaDeDados
{
    public class DicionarioFredis
    {
        public DicionarioFredis(string chave, string valor, Operacao operacao, Transacao transacao = null)
        {
            Chave = chave;
            Valor = valor;
            Transacao = transacao;
            Operacao = operacao;
            Historico = new Stack<Registro>();
        }

        public override bool Equals(Object obj)
        {
            return (obj as string) == Chave;
        }

        public Transacao Transacao { get; set; }

        public string Chave { get; set; }
        public string Valor { get; set; }
        public string ValorAntigo { get; set; }

        public Operacao Operacao { get; set; }

        public Stack<Registro> Historico;

        public override int GetHashCode()
        {
            return HashCode.Combine(Transacao, Chave, Valor, Operacao);
        }
    }

    public class Registro
    {
        public readonly Transacao transacao;
        public readonly Operacao? operacao;
        public readonly string valor;

        public Registro(Transacao t, Operacao o, string v)
        {
            transacao = t;
            operacao = o;
            valor = v;
        }
    }
}
