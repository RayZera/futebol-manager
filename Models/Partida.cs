public class Partida
{
    public int Id { get; set; }
    public int TimeAId { get; set; }
    public int TimeBId { get; set; }
    public int? TimeVencedorId { get; set; }
    public int? TimePerdedorId { get; set; }
    public string JogoId { get; set; } = string.Empty;
    public DateTime DataPartida { get; set; }
}
