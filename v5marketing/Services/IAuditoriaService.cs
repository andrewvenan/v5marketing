namespace v5marketing.Services
{
    public interface IAuditoriaService
    {
        Task RegistrarAsync(string acao, string entidade, string descricao);
    }
}