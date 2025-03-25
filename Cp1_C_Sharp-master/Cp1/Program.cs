using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Random r = new Random();
    static byte chanceRoleta = 6;
    static Dictionary<string, (byte vitorias, byte partidas)> ranking = new Dictionary<string, (byte, byte)>();
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

        byte[] maosJogador = EscolherMaosJogador();
        byte[] maosComputador = EscolherMaoComputador();

        Console.WriteLine($"\nSuas mãos: {ConverterParaNome(maosJogador[0])} e {ConverterParaNome(maosJogador[1])}");
        Console.WriteLine($"Mãos do Computador: {ConverterParaNome(maosComputador[0])} e {ConverterParaNome(maosComputador[1])}");

        Console.WriteLine("Escolha qual mão jogar (0 - Primeira, 1 - Segunda): ");
        byte escolhaJogador = LerEntradaValida(0, 1);
        byte maoEscolhidaJogador = maosJogador[escolhaJogador];

        byte maoEscolhidaComputador = EscolherMaoComputador(maosComputador, maosJogador);
        Console.WriteLine($"\nComputador escolheu: {ConverterParaNome(maoEscolhidaComputador)}");

        byte resultado = DeterminarVencedor(maoEscolhidaJogador, maoEscolhidaComputador);
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

    static void AtualizarRanking(byte resultado)
    {
        if (!ranking.ContainsKey(jogadorAtual))
            ranking[jogadorAtual] = (0, 0);

        byte (vitorias, partidas) = ranking[jogadorAtual];
        ranking[jogadorAtual] = (vitorias + (resultado == 1 ? 1 : 0), partidas + 1);
    }

    static void ExibirRanking()
    {
        Console.WriteLine("\n🏆 Ranking:");
        if (ranking.Count == 0)
        {
            Console.WriteLine("Ainda não há jogadores no ranking.");
            return;
        }

        var rankingOrdenado = ranking.OrderByDescending(j => (double)j.Value.vitorias / j.Value.partidas)
                                      .ThenByDescending(j => j.Value.vitorias)
                                      .ToList();

        byte maxExibir = Math.Min(3, rankingOrdenado.Count);
        string[] medalhas = { "🥇", "🥈", "🥉" };

        for (byte i = 0; i < maxExibir; i++)
        {
            var jogador = rankingOrdenado[i];
            double porcentagem = (double)jogador.Value.vitorias / jogador.Value.partidas * 100;
            Console.WriteLine($"{medalhas[i]} {jogador.Key}: {porcentagem:F2}% de vitórias ({jogador.Value.vitorias}/{jogador.Value.partidas})");
        }

        byte posicaoJogadorAtual = rankingOrdenado.FindIndex(j => j.Key == jogadorAtual) + 1;
        if (posicaoJogadorAtual > 3)
        {
            byte jogador = ranking[jogadorAtual];
            double porcentagem = (double)jogador.vitorias / jogador.partidas * 100;
            Console.WriteLine($"📌 Seu ranking: {posicaoJogadorAtual}º lugar com {porcentagem:F2}% de vitórias ({jogador.vitorias}/{jogador.partidas}).");
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
        byte[] maos = new byte[2];
        for (byte i = 0; i < 2; i++)
        {
            Console.Write($"Escolha a {i + 1}ª mão: ");
            maos[i] = LerEntradaValida(0, 2);
        }
        return maos;
    }

    static int LerEntradaValida(byte min, byte max)
    {
        byte valor;
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

    static byte[] EscolherMaoComputador()
    {
        byte mao1 = r.Next(0, 3); // Escolhe a primeira mão aleatória
        byte mao2;

        do
        {
            mao2 = r.Next(0, 3); // Gera uma nova mão para a segunda mão
        } while (mao2 == mao1); // Verifica se a segunda mão é igual à primeira

        return new byte[] { mao1, mao2 }; // Retorna as duas mãos do computador
    }

    static byte EscolherMaoComputador(byte[] maosComputador, byte[] maosJogador)
    {
        foreach (byte mao in maosComputador)
        {
            if (Vence(mao, maosJogador[0]) || Vence(mao, maosJogador[1]))
                return mao;
        }
        return maosComputador[r.Next(0, 2)];
    }

    static bool Vence(byte a, byte b) => (a == 0 && b == 2) || (a == 1 && b == 0) || (a == 2 && b == 1);
    static int DeterminarVencedor(byte jogador, byte computador) => jogador == computador ? 0 : (Vence(jogador, computador) ? 1 : -1);
    static bool RoletaRussa() => r.Next(1, chanceRoleta + 1) == 1;
    static string ConverterParaNome(byte mao) => mao == 0 ? "Pedra ✊" : mao == 1 ? "Papel ✋" : "Tesoura ✌";
}
