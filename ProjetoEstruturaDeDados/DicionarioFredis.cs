using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoEstruturaDeDados
{
    class DicionarioFredis
    {
        public DicionarioFredis(string chave, int valor)
        {
            Chave = chave;
            Valor = valor;
        }
        public string Chave { get; set; }
        public int Valor { get; set; }
    }
}
