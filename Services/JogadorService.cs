using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FutebolManager.Models;

namespace FutebolManager.Services
{
    public class JogadorService
    {
        private readonly string _filePath =
            Path.Combine(AppContext.BaseDirectory, "Database", "jogadores.json");

        private readonly List<Jogador> _jogadores;

        public JogadorService()
        {
            _jogadores = File.Exists(_filePath)
                ? JsonSerializer.Deserialize<List<Jogador>>(File.ReadAllText(_filePath)) ?? new()
                : new();
        }

        /* ----------------- CRUD ----------------- */

        public void Adicionar(Jogador jogador)
        {
            _jogadores.Add(jogador);
            Salvar();
        }

        public void Remover(int indice, TimeService timeService)
{
    if (indice < 0 || indice >= _jogadores.Count)
    {
        Console.WriteLine("Índice inválido.");
        return;
    }

    var jogadorRemovido = _jogadores[indice];
    _jogadores.RemoveAt(indice);
    Salvar();

    // Remove dos times também
    foreach (var time in timeService.ObterTodos())
    {
        time.Jogadores.RemoveAll(j => j.Id == jogadorRemovido.Id);
    }

    timeService.Salvar();

    Console.WriteLine("Jogador removido com sucesso.");
}


        public void Listar()
        {
            if (_jogadores.Count == 0)
            {
                Console.WriteLine("Nenhum jogador cadastrado.");
                return;
            }

            for (int i = 0; i < _jogadores.Count; i++)
            {
                var j = _jogadores[i];
                Console.WriteLine($"[{i}] {j.Nome} - {j.Posicao} - Idade: {j.Idade} - ID: {j.Id}");
            }
        }

        /* ----------- NOVO: expor lista ----------- */
        public List<Jogador> ObterTodos() => _jogadores;

        /* ------------- persistência -------------- */
        private void Salvar()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            File.WriteAllText(
                _filePath,
                JsonSerializer.Serialize(_jogadores,
                    new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
