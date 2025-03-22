using System;
using System.Collections.Generic;

class Program
{
    static Random r = new Random();
    static int chanceRoleta = 6;
    static Dictionary<string, (int vitorias, int partidas)> ranking = new Dictionary<string, (int, int)>();
    static string jogadorAtual;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Write("Digite seu nome: ");
        while (true)
        {
            jogadorAtual = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(jogadorAtual))
                break;
            Console.WriteLine("Nome inválido. Por favor, digite um nome válido.");
        }

        bool continuar = true;
        while (continuar)
        {
            continuar = JogarRodada();
        }
    }

    static bool JogarRodada()
    {
        Console.Clear();
        Console.WriteLine("\nEscolha suas duas mãos: 0 - Pedra ✊, 1 - Papel ✋, 2 - Tesoura ✌");

        int[] maosJogador = EscolherMaosJogador();
        int[] maosComputador = { r.Next(0, 3), r.Next(0, 3) };

        Console.WriteLine($"\nSuas mãos: {ConverterParaNome(maosJogador[0])} e {ConverterParaNome(maosJogador[1])}");
        Console.WriteLine($"Mãos do Computador: {ConverterParaNome(maosComputador[0])} e {ConverterParaNome(maosComputador[1])}");

        Console.WriteLine("Escolha qual mão jogar (0 - Primeira, 1 - Segunda): ");
        int escolhaJogador = LerEntradaValida(0, 1);
        int maoEscolhidaJogador = maosJogador[escolhaJogador];

        int maoEscolhidaComputador = EscolherMaoComputador(maosComputador, maosJogador);
        Console.WriteLine($"\nComputador escolheu: {ConverterParaNome(maoEscolhidaComputador)}");

        int resultado = DeterminarVencedor(maoEscolhidaJogador, maoEscolhidaComputador);
        AtualizarRanking(resultado);

        if (resultado == 0) Console.WriteLine("Empate!");
        else if (resultado == 1) Console.WriteLine("Você venceu esta rodada!");
        else Console.WriteLine("Computador venceu esta rodada!");

        if (resultado != 0)
        {
            Console.WriteLine("💀 Iniciando a Roleta Russa...");
            if (RoletaRussa())
            {
                Console.WriteLine("🔫 Perdeu na Roleta Russa! Fim de jogo para {0}.", jogadorAtual);
                ExibirRanking();
                NovoJogador();
            }
            else
            {
                Console.WriteLine("😰 Sobreviveu à Roleta Russa! Continuamos...");
                chanceRoleta--;
                if (chanceRoleta < 1) chanceRoleta = 1;
            }
        }

        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        return true;
    }

    static void AtualizarRanking(int resultado)
    {
        if (!ranking.ContainsKey(jogadorAtual))
            ranking[jogadorAtual] = (0, 0);

        var (vitorias, partidas) = ranking[jogadorAtual];
        ranking[jogadorAtual] = (vitorias + (resultado == 1 ? 1 : 0), partidas + 1);
    }

    static void ExibirRanking()
    {
        Console.WriteLine("\n🏆 Ranking:");
        foreach (var jogador in ranking)
        {
            double porcentagem = jogador.Value.partidas > 0 ? (double)jogador.Value.vitorias / jogador.Value.partidas * 100 : 0;
            Console.WriteLine($"{jogador.Key}: {porcentagem:F2}% de vitórias ({jogador.Value.vitorias}/{jogador.Value.partidas})");
        }
    }

    static void NovoJogador()
    {
        Console.Write("Digite o nome do próximo jogador: ");
        while (true)
        {
            jogadorAtual = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(jogadorAtual))
                break;
            Console.WriteLine("Nome inválido. Por favor, digite um nome válido.");
        }
        chanceRoleta = 6;
    }

    static int[] EscolherMaosJogador()
    {
        int[] maos = new int[2];
        for (int i = 0; i < 2; i++)
        {
            Console.Write($"Escolha a {i + 1}ª mão: ");
            maos[i] = LerEntradaValida(0, 2);
        }
        return maos;
    }

    static int LerEntradaValida(int min, int max)
    {
        int valor;
        while (true)
        {
            char entrada = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (char.IsDigit(entrada))
            {
                valor = entrada - '0';
                if (valor >= min && valor <= max)
                {
                    return valor;
                }
            }
            Console.WriteLine($"Entrada inválida. Escolha um número entre {min} e {max}.");
        }
    }

    static int EscolherMaoComputador(int[] maosComputador, int[] maosJogador)
    {
        foreach (int mao in maosComputador)
        {
            if (Vence(mao, maosJogador[0]) || Vence(mao, maosJogador[1]))
                return mao;
        }
        return maosComputador[r.Next(0, 2)];
    }

    static bool Vence(int a, int b)
    {
        return (a == 0 && b == 2) || (a == 1 && b == 0) || (a == 2 && b == 1);
    }

    static int DeterminarVencedor(int jogador, int computador)
    {
        if (jogador == computador) return 0;
        return Vence(jogador, computador) ? 1 : -1;
    }

    static bool RoletaRussa()
    {
        return r.Next(1, chanceRoleta + 1) == 1;
    }

    static string ConverterParaNome(int mao)
    {
        return mao == 0 ? "Pedra ✊" : mao == 1 ? "Papel ✋" : "Tesoura ✌";
    }
}
