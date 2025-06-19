
using System;
using System.Globalization;
using FutebolManager.Models;
using FutebolManager.Services;

var jogadorService = new JogadorService();
var jogoService = new JogoService();
var timeService = new TimeService();
var partidaService = new PartidaService();



while (true)
{
    Console.Clear();
    Console.WriteLine("=== MENU FUTEBOL MANAGER ===");
    Console.WriteLine("1 - Adicionar Jogador");
    Console.WriteLine("2 - Listar Jogadores");
    Console.WriteLine("3 - Remover Jogador");
    Console.WriteLine("4 - Adicionar Jogo");
    Console.WriteLine("5 - Listar Times");
    Console.WriteLine("6 - Remover Time");
    Console.WriteLine("7 - Registrar Interesse em Jogo");
    Console.WriteLine("8 - Registrar Partida");
    Console.WriteLine("9 - Adicionar Jogador a um Time");
    Console.WriteLine("10 - Tabela de Pontos");
    Console.WriteLine("0 - Sair");
    Console.Write("Escolha: ");
    var opcao = Console.ReadLine();

    switch (opcao)
    {
        /* ------------------------- 1. Adicionar Jogador ------------------------- */
        case "1":
            Console.Write("Nome: ");
            var nome = Console.ReadLine()!;

            Console.Write("Idade: ");
            if (!int.TryParse(Console.ReadLine(), out int idade))
            {
                Console.WriteLine("Idade inválida!");
                Console.ReadKey(); break;
            }

            Console.Write("Posição (Goleiro, Defesa, Ataque): ");
            var posicao = Console.ReadLine()!;

            var jogador = new Jogador
            {
                Id      = Guid.NewGuid().ToString(),
                Nome    = nome,
                Idade   = idade,
                Posicao = posicao
            };

            jogadorService.Adicionar(jogador);
            Console.WriteLine("Jogador adicionado!");
            Console.ReadKey();
            break;

        /* ------------------------- 2. Listar Jogadores -------------------------- */
        case "2":
            jogadorService.Listar();
            Console.ReadKey();
            break;

        /* ------------------------- 3. Remover Jogador --------------------------- */
        case "3":
    jogadorService.Listar();
    Console.Write("\nDigite o número do jogador a remover: ");

    if (int.TryParse(Console.ReadLine(), out int indice))
        jogadorService.Remover(indice, timeService); // ← passando timeService
    else
        Console.WriteLine("Entrada inválida!");

    Console.ReadKey();
    break;

        /* ------------------------- 4. Adicionar Jogo ---------------------------- */
        case "4":
    Console.Write("Time da casa: ");
    var timeCasa = Console.ReadLine()!;

    Console.Write("Time visitante: ");
    var timeVisitante = Console.ReadLine()!;

    Console.Write("Data do jogo (dd/MM/yyyy HH:mm): ");
    if (!DateTime.TryParseExact(Console.ReadLine(),
                                "dd/MM/yyyy HH:mm",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime dataJogo))
    {
        Console.WriteLine("Data inválida!");
        Console.ReadKey();
        break;
    }

    
    var jogo = new Jogo
    {
        TimeCasa      = timeCasa,
        TimeVisitante = timeVisitante,
        Data          = dataJogo
    };
    jogoService.Adicionar(jogo);

    
    string NomeComSufixo(string baseName)
    {
        int rep = timeService.ObterTodos()
                  .Count(t => t.Nome.StartsWith(baseName, StringComparison.OrdinalIgnoreCase));
        return rep > 0 ? $"{baseName} JOGO{rep + 1}" : baseName;
    }

    timeService.Adicionar(new Time
    {
        Nome   = NomeComSufixo(timeCasa),
        JogoId = jogo.Id
    });

    timeService.Adicionar(new Time
    {
        Nome   = NomeComSufixo(timeVisitante),
        JogoId = jogo.Id
    });

    Console.WriteLine("Jogo e times cadastrados!");
    Console.ReadKey();
    break;

        case "5":
            timeService.Listar();
            Console.ReadKey();
            break;

        case "6":
            timeService.Listar();
            Console.Write("Digite o ID do time a remover: ");
            if (int.TryParse(Console.ReadLine(), out int idTime))
            {
            timeService.Remover(idTime);
            }
            else
            {
            Console.WriteLine("ID inválido.");
            }
            Console.ReadKey();
            break;

            /* ---------- 7. Registrar Partida ---------- */
case "7":
    timeService.Listar();
    Console.Write("ID do Time A: ");
    int idTimeA = int.Parse(Console.ReadLine()!);

    Console.Write("ID do Time B: ");
    int idTimeB = int.Parse(Console.ReadLine()!);

    var partida = partidaService.CriarPartida(idTimeA, idTimeB, "");

    Console.Write("Gols do Time A: ");
    int golsA = int.Parse(Console.ReadLine()!);

    Console.Write("Gols do Time B: ");
    int golsB = int.Parse(Console.ReadLine()!);

    partidaService.RegistrarResultado(partida.Id, golsA, golsB, timeService);

    Console.WriteLine("Partida registrada!");
    Console.ReadKey();
    break;

/* ---------- 8. Registrar Partida ---------- */
case "8":
{
    timeService.Listar();

    Console.Write("ID do Time A: ");
    int idTimeAa = int.Parse(Console.ReadLine()!);

    Console.Write("Gols do Time A: ");
    int golsAa = int.Parse(Console.ReadLine()!);

    Console.Write("ID do Time B: ");
    int idTimeBb = int.Parse(Console.ReadLine()!);

    Console.Write("Gols do Time B: ");
    int golsBb = int.Parse(Console.ReadLine()!);

    var novaPartida = partidaService.CriarPartida(idTimeAa, idTimeBb, "");

    partidaService.RegistrarResultado(novaPartida.Id, golsAa, golsBb, timeService);

    Console.WriteLine("Partida registrada!");
    Console.ReadKey();
    break;
}


/* ---------- 9. Adicionar Jogador a um Time ---------- */
case "9":
{
    var listaJogadores = jogadorService.ObterTodos();
    jogadorService.Listar(); // mostra [índice] + dados

    Console.Write("Digite o número (índice) do jogador: ");
    if (!int.TryParse(Console.ReadLine(), out int idxJog) ||
        idxJog < 0 || idxJog >= listaJogadores.Count)
    {
        Console.WriteLine("Índice de jogador inválido.");
        Console.ReadKey(); break;
    }
    var jogadorSel = listaJogadores[idxJog];

    timeService.Listar();
    Console.Write("ID do Time destino: ");
    if (!int.TryParse(Console.ReadLine(), out int idTimeDestino))
    {
        Console.WriteLine("ID de time inválido.");
        Console.ReadKey(); break;
    }

    var timeDest = timeService.ObterTodos()
                  .FirstOrDefault(t => t.Id == idTimeDestino);

    if (timeDest == null)
    {
        Console.WriteLine("Time não encontrado.");
    }
    else
    {
        timeDest.Jogadores.Add(jogadorSel);
        timeService.Salvar();
        Console.WriteLine($"Jogador {jogadorSel.Nome} adicionado ao time {timeDest.Nome}!");
    }
    Console.ReadKey();
    break;
}

    /* ---------- 10. Tabela de Pontos ---------- */
case "10":
    var tabela = timeService.ObterTodos()
                 .OrderByDescending(t => t.Pontos)
                 .ThenByDescending(t => t.Vitorias);

    Console.WriteLine("\nTime              P  V  E  D");
    foreach (var t in tabela)
        Console.WriteLine($"{t.Nome,-16} {t.Pontos,2} {t.Vitorias,2} {t.Empates,2} {t.Derrotas,2}");
    Console.ReadKey();
    break;



        /* ------------------------------ 0. Sair --------------------------------- */
        case "0":
            return;

        default:
            Console.WriteLine("Opção inválida!");
            Console.ReadKey();
            break;
    }
}
