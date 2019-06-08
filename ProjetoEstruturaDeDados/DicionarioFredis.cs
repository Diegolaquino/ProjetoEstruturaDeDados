using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEstruturaDeDados
{
    class DicionarioFredis
    {
        public DicionarioFredis(string chave, string valor)
        {
            Chave = chave;
            Valor = valor;
        }
        public string Chave { get; set; }
        public string Valor { get; set; }
    }
}
