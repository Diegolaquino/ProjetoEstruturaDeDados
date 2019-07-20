using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEstruturaDeDados
{
    public class DicionarioFredis
    {
        public DicionarioFredis(string chave, string valor)
        {
            Chave = chave;
            Valor = valor;
            Operacao = Operacao.Insercao;
        }

        public override bool Equals(Object obj)
        {
            return (obj as string) == Chave;
        }

        public Transacao Transacao { get; set; }

        public string Chave { get; set; }
        public string Valor { get; set; }

        public Operacao Operacao { get; set; }
    }
}
