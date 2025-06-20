// Services/InteressadoService.cs
using FutebolManager.Models;
using System.Collections.Generic;
using System.Linq;

namespace FutebolManager.Services
{
    public class InteressadoService : BaseJsonService<Interessado>
    {
        private int _nextId;

        public InteressadoService() : base("interessados.json")
        {
            _nextId = Items.Any() ? Items.Max(i => i.Id) + 1 : 1;
        }

        public void Adicionar(int jogadorId, string jogoId)
        {
            if (Items.Any(i => i.JogadorId == jogadorId && i.JogoId == jogoId))
            {
                Console.WriteLine("Este jogador já demonstrou interesse neste jogo.");
                return;
            }

            var novoInteressado = new Interessado
            {
                Id = _nextId++,
                JogadorId = jogadorId,
                JogoId = jogoId,
                DataRegistro = DateTime.Now
            };

            Items.Add(novoInteressado);
            Save();
            Console.WriteLine("Interesse registrado com sucesso!");
        }

        public List<Interessado> ObterTodos()
        {
            return Items;
        }

        public List<string> ObterInteressesFormatados()
        {
            if (!Items.Any())
                return new List<string> { "Nenhum interesse registrado." };

            var linhas = new List<string>();
            foreach (var i in Items)
            {
                linhas.Add($"ID: {i.Id} | Jogador ID: {i.JogadorId} | Jogo ID: {i.JogoId} | Data Registro: {i.DataRegistro:dd/MM/yyyy HH:mm}");
            }
            return linhas;
        }

        public void Remover(int id)
        {
            var interessadoParaRemover = Items.FirstOrDefault(i => i.Id == id);
            if (interessadoParaRemover == null)
            {
                Console.WriteLine($"Interesse com ID {id} não encontrado.");
                return;
            }

            Items.Remove(interessadoParaRemover);
            Save();
            Console.WriteLine($"Interesse com ID {id} removido com sucesso.");
        }
    }
}
