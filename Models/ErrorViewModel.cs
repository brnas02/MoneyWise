namespace DWebProjetoFinal.Models
{
    // Modelo de dados utilizado para apresentar informações em páginas de erro
    public class ErrorViewModel
    {
        // ID da requisição que originou o erro (útil para debugging)
        public string? RequestId { get; set; }

        // Indica se o RequestId deve ser mostrado na view (evita exibir null ou vazio)
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}