namespace FutebolManager.Models;

public class Jogador
{
    public string Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Posicao { get; set; } // Goleiro, Defesa, Ataque
}
