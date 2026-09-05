namespace FaturamentoService.Models
{
    public class ItemNotaFiscal
    {
        public int Id { get; set; }
        public int ProdutoCodigo { get; set; }
        public int NotaFiscalNumeroSequencial { get; set; }
        public int Quantidade { get; set; } = 0;
    }
}
