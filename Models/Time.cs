using FutebolManager.Models;

public class Time
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public List<Jogador> Jogadores { get; set; } = new();
    public string JogoId { get; set; } = string.Empty;

    public int Vitorias { get; set; }
    public int Empates  { get; set; }
    public int Derrotas { get; set; }
    public int Pontos   { get; set; }
}
