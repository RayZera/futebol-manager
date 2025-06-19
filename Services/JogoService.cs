using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using FutebolManager.Models;

namespace FutebolManager.Services
{
    public class JogoService
    {
        private readonly string _filePath;
        private readonly List<Jogo> _jogos;

        public JogoService()
        {
            
            _filePath = Path.Combine(AppContext.BaseDirectory,
                                     "Database", "jogos.json");

            _jogos = Carregar();
        }

        

        private List<Jogo> Carregar()
        {
            if (!File.Exists(_filePath)) return new();

            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<Jogo>>(json) ?? new();
        }

        private void Salvar()
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            var json = JsonConvert.SerializeObject(_jogos,
                                                   Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        

        public void Adicionar(Jogo jogo)
        {
            _jogos.Add(jogo);
            Salvar();
            Console.WriteLine("Jogo adicionado.");
        }

        public void Listar()
        {
            if (_jogos.Count == 0)
            {
                Console.WriteLine("Nenhum jogo cadastrado.");
                return;
            }

            for (int i = 0; i < _jogos.Count; i++)
            {
                var j = _jogos[i];
                Console.WriteLine(
                    $"[{i}] {j.TimeCasa} x {j.TimeVisitante}  " +
                    $"- {j.Data:dd/MM/yyyy HH:mm}");
            }
        }

        public void Remover(int indice)
        {
            if (indice < 0 || indice >= _jogos.Count)
            {
                Console.WriteLine("Índice de jogo inválido.");
                return;
            }

            _jogos.RemoveAt(indice);
            Salvar();
            Console.WriteLine("Jogo removido.");
        }

        public void AdicionarInteressado(int indiceJogo, string jogadorId)
        {
            if (indiceJogo < 0 || indiceJogo >= _jogos.Count)
            {
                Console.WriteLine("Índice de jogo inválido.");
                return;
            }

            var jogo = _jogos[indiceJogo];

            if (!jogo.InteressadosIds.Contains(jogadorId))
            {
                jogo.InteressadosIds.Add(jogadorId);
                Salvar();
                Console.WriteLine("Jogador adicionado à lista de interessados!");
            }
            else
            {
                Console.WriteLine("Jogador já está na lista.");
            }
        }
    }
}
