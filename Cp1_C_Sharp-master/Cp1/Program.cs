using System;

class Program
{
    static Random r = new Random();
    static int chanceRoleta = 6; // Começa com 1 em 6 chances de perder

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("😀 Olá! Vamos jogar Jokenpô versão Round 6?");
        Console.WriteLine("1 - Sim ou 0 - Não");

        char escolha;
        do
        {
            escolha = Console.ReadKey().KeyChar;
            Console.WriteLine();
            if (escolha != '1' && escolha != '0')
            {
                Console.WriteLine("Entrada inválida. Digite 1 para jogar ou 0 para sair.");
            }
        } while (escolha != '1' && escolha != '0');

        if (escolha == '1')
        {
            bool continuar = true;
            while (continuar)
            {
                continuar = JogarRodada();
            }
        }
        Console.WriteLine("👋 Tchau! Até a próxima");
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

        if (resultado == 0) Console.WriteLine("Empate!");
        else if (resultado == 1) Console.WriteLine("Você venceu esta rodada!");
        else Console.WriteLine("Computador venceu esta rodada!");

        if (resultado != 0)
        {
            Console.WriteLine("💀 Iniciando a Roleta Russa..."); 
            if (RoletaRussa())
            {
                Console.WriteLine("🔫 Perdeu no Jokenpô e na Roleta Russa! Jogo encerrado.");
                return false;
            }
            else
            {
                Console.WriteLine("😰 Sobreviveu à Roleta Russa! Continuamos...");
                chanceRoleta--; // Diminui as chances de sobrevivência a cada rodada
                if (chanceRoleta < 1) chanceRoleta = 1; // Garante que haja um perdedor no maximo apos 6 rodadas
            }
        }

        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        return true;
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
            if (Vence(mao, maosJogador[0]) && Vence(mao, maosJogador[1]))
                return mao;
        }
        foreach (int mao in maosComputador)
        {
            if (mao == maosJogador[0] || mao == maosJogador[1])
                return mao;
        }
        return maosComputador[r.Next(0, 2)];
    }

    static int DeterminarVencedor(int jogador, int computador)
    {
        if (jogador == computador) return 0;
        if (Vence(jogador, computador)) return 1;
        return -1;
    }

    static bool Vence(int m1, int m2)
    {
        return (m1 == 0 && m2 == 2) || (m1 == 1 && m2 == 0) || (m1 == 2 && m2 == 1);
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
