﻿namespace APITeste.Models
{
    public class ProdutoInputDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Categoria { get; set; }
    }
}
