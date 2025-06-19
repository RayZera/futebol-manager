using System;
using System.Collections.Generic;
using System.Linq;
using FutebolManager.Services;   // para acessar TimeService
using FutebolManager.Models;

public class PartidaService
{
    private readonly List<Partida> _partidas = new();
    private int _id = 1;

    /* ----------- criar partida ------------ */
    public Partida CriarPartida(int timeAId, int timeBId, string jogoId)
    {
        var p = new Partida
        {
            Id          = _id++,
            TimeAId     = timeAId,
            TimeBId     = timeBId,
            JogoId      = jogoId,
            DataPartida = DateTime.Now
        };
        _partidas.Add(p);
        return p;
    }

    /* ----------- registrar resultado + pontos ----------- */
    /// <param name="resultado">
    /// "A" = TimeA venceu, "B" = TimeB venceu, "E" = empate
    /// </param>
    public void RegistrarResultado(int partidaId,
                               int golsA, int golsB,
                               TimeService timeService)
{
    var p = _partidas.FirstOrDefault(x => x.Id == partidaId);
    if (p == null) { Console.WriteLine("Partida não encontrada."); return; }

    var times = timeService.ObterTodos();
    var timeA = times.First(t => t.Id == p.TimeAId);
    var timeB = times.First(t => t.Id == p.TimeBId);

    if (golsA == golsB)            // empate
    {
        p.TimeVencedorId = null;
        Atualiza(timeA, 1, e:1);
        Atualiza(timeB, 1, e:1);
    }
    else
    {
        var vencedor = golsA > golsB ? timeA : timeB;
        var perdedor = vencedor == timeA ? timeB : timeA;

        p.TimeVencedorId = vencedor.Id;
        Atualiza(vencedor, 3, v:1);
        Atualiza(perdedor, 0, d:1);
    }

    timeService.Salvar();

    void Atualiza(Time t, int pts, int v = 0, int e = 0, int d = 0)
    {
        t.Pontos   += pts;
        t.Vitorias += v;
        t.Empates  += e;
        t.Derrotas += d;
    }
}


    /* ----------- histórico ----------- */
    public void ExibirHistorico()
    {
        if (_partidas.Count == 0)
        {
            Console.WriteLine("Nenhuma partida registrada.");
            return;
        }

        foreach (var p in _partidas)
        {
            var resultado = p.TimeVencedorId == null ? "Empate"
                           : $"Vencedor: {p.TimeVencedorId}";
            Console.WriteLine($"Partida #{p.Id}: {p.TimeAId} x {p.TimeBId} | {resultado}");
        }
    }
}
