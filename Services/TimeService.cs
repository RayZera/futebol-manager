using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using FutebolManager.Models;

public class TimeService
{
    private readonly string _filePath;
    private List<Time> _times;
    private int _idContador = 1;

    public TimeService()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, "Database", "times.json");

        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            _times = JsonSerializer.Deserialize<List<Time>>(json) ?? new List<Time>();
            if (_times.Count > 0)
                _idContador = _times.Max(t => t.Id) + 1;
        }
        else
        {
            _times = new List<Time>();
        }
    }

    public void Salvar()
    {
        string dir = Path.GetDirectoryName(_filePath)!;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonSerializer.Serialize(_times, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public void Adicionar(Time time)
    {
        time.Id = _idContador++;
        _times.Add(time);
        Salvar();
    }

    public void Listar()
{
    if (_times.Count == 0)
    {
        Console.WriteLine("Nenhum time cadastrado.");
        return;
    }

    foreach (var t in _times)
    {
        
        Console.WriteLine($"[{t.Id}] {t.Nome}  (Jogo #{t.JogoId})");
        foreach (var j in t.Jogadores)
            Console.WriteLine($"   • {j.Nome} ({j.Posicao})");
    }
}



    public void Remover(int id)
    {
        var time = _times.FirstOrDefault(t => t.Id == id);
        if (time == null)
        {
            Console.WriteLine("Time não encontrado.");
            return;
        }

        _times.Remove(time);
           if (_times.Count == 0) 
        _idContador = 1;

        Salvar();
        Console.WriteLine("Time removido com sucesso!");
    }

    public List<Time> ObterTodos() => _times;
}
