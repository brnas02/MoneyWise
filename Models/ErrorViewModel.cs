namespace DWebProjetoFinal.Models
{
    // Modelo de dados utilizado para apresentar informa��es em p�ginas de erro
    public class ErrorViewModel
    {
        // ID da requisi��o que originou o erro (�til para debugging)
        public string? RequestId { get; set; }

        // Indica se o RequestId deve ser mostrado na view (evita exibir null ou vazio)
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}