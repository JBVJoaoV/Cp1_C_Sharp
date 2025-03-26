using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

class Program
{
    static Random r = new Random(); //Crianção do objeto Random
    static int chanceRoleta = 6; // Definindo chance inicial do roleta de 6

    //  Dicionário para armazenar o ranking de jogadores (nome -> (vitórias, partidas jogadas))
    static Dictionary<string, (int vitorias, int partidas)> ranking = new Dictionary<string, (int, int)>();
    static string jogadorAtual; //Variavel para armazenar o nome do jogador atual

    // Iniciando metodo principal
    static void Main()
    { // Estilizando o console
        Console.Title = "💣 JOKEMPO MENOS 1 - O JOGO"; // titulo do console
        Console.BackgroundColor = ConsoleColor.DarkBlue; // Cor do plano de fundo
        Console.ForegroundColor = ConsoleColor.White; // Cor do texto
        Console.Clear(); // Aplica as cores em todo o terminal
        Console.OutputEncoding = System.Text.Encoding.UTF8; // possibilita o uso de emojis
        // Arte ASCII gerada por meio do site https://patorjk.com/software/taag usando a fonte "3-D ASCII", usamos o @ antes da arte em si para evitar quebra de linhas 
        Console.WriteLine(@"

                ___  ________  ___  __    _______   ________   ________  ________                         _____     
               |\  \|\   __  \|\  \|\  \ |\  ___ \ |\   ___  \|\   __  \|\   __  \                       / __  \    
               \ \  \ \  \|\  \ \  \/  /|\ \   __/|\ \  \\ \  \ \  \|\  \ \  \|\  \        ____________ |\/_|\  \   
             __ \ \  \ \  \\\  \ \   ___  \ \  \_|/_\ \  \\ \  \ \   ____\ \  \\\  \      |\____________\|/ \ \  \  
            |\  \\_\  \ \  \\\  \ \  \\ \  \ \  \_|\ \ \  \\ \  \ \  \___|\ \  \\\  \     \|____________|    \ \  \ 
            \ \________\ \_______\ \__\\ \__\ \_______\ \__\\ \__\ \__\    \ \_______\                        \ \__\
             \|________|\|_______|\|__| \|__|\|_______|\|__| \|__|\|__|     \|_______|                         \|__|
                                          


                            Bem-vindo ao jogo JOKEMPO MENOS 1!
                            Derrote o computador... e sobreviva à Roleta Russa 💀🔫                                                        
        ");

        Thread.Sleep(3000); // Espera 3 segundos antes de continuar com o jogo


        Console.Write("Digite seu nome: "); //solicitando o nome do usuario

        //Loop para verificar se o nome foi digitado
        while (true)
        {
            jogadorAtual = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(jogadorAtual))
                break;
            Console.WriteLine("Nome inválido. Por favor, digite um nome válido.");
        }

        // Loop para continuar enquanto a roleta russa não chegar ao fim
        bool continuar = true;
        while (continuar)
        {
            continuar = JogarRodada();
        }
    }

    // Metodo para executar o jogo
    static bool JogarRodada()
    {
        Console.Clear();
        Console.WriteLine("\nEscolha suas duas mãos: 0 - Pedra ✊, 1 - Papel ✋, 2 - Tesoura ✌");

        // Escolhe as duas mãos do usuario e do computador
        int[] maosJogador = EscolherMaosJogador();
        int[] maosComputador = GerarMaoComputador();

        // Exibe quais são as mãos em jogo
        Console.WriteLine($"\nSuas mãos: {ConverterParaNome(maosJogador[0])} e {ConverterParaNome(maosJogador[1])}");
        Console.WriteLine($"Mãos do Computador: {ConverterParaNome(maosComputador[0])} e {ConverterParaNome(maosComputador[1])}");

        // Solicita a escolha de mão do usuario
        Console.WriteLine("Escolha qual mão jogar (0 - Primeira, 1 - Segunda): ");
        int escolhaJogador = LerEntradaValida(0, 1); // Valida se o valor é valido
        int maoEscolhidaJogador = maosJogador[escolhaJogador];

        // O computador escolhe de forma estrategica qual mão será jogada
        int maoEscolhidaComputador = EscolherMaoComputador(maosComputador, maosJogador);
        Console.WriteLine($"\nComputador escolheu: {ConverterParaNome(maoEscolhidaComputador)}");

        // Com base nas mãos escolhidas, determina o vencedor
        int resultado = DeterminarVencedor(maoEscolhidaJogador, maoEscolhidaComputador);
        AtualizarRanking(resultado); // Atualização do rankig

        // Exibição do vencedor
        if (resultado == 0) Console.WriteLine("Empate!");
        else if (resultado == 1) Console.WriteLine("Você venceu esta rodada!");
        else Console.WriteLine("Computador venceu esta rodada!");

        // Estrutura condicional para a rolera russa
        if (resultado != 0)
        {
            Console.WriteLine("💀 Iniciando a Roleta Russa...");
            if (RoletaRussa()) // Se perdeu na roleta russa...
            {
                Console.WriteLine("🔫 Perdeu na Roleta Russa! Fim de jogo para {0}.", jogadorAtual);
                ExibirRanking(); // Mostra o ranking
                NovoJogador(); // Solicita um novo jogador
            }
            else // Se sobrevivel a roleta russa...
            {
                Console.WriteLine("😰 Sobreviveu à Roleta Russa! Continuamos...");
                chanceRoleta--; //Diminui a chance de ganhar na roleta
                if (chanceRoleta < 1) chanceRoleta = 1; //Limita a chance minima da roleta de 1/1
            }
        }

        // Intervalo para o usuario poder ler o terminal
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        return true;
    }

    // Metodo para a atualização do ranking
    static void AtualizarRanking(int resultado)
    {
        // Adiciona o jogador ao ranking caso ele não esteja
        if (!ranking.ContainsKey(jogadorAtual))
            ranking[jogadorAtual] = (0, 0);

        var (vitorias, partidas) = ranking[jogadorAtual]; // Desestrutura as vitorias e partidas
        ranking[jogadorAtual] = (vitorias + (resultado == 1 ? 1 : 0), partidas + 1); // Adiciona ao ranking com base no resultado
    }

    // Metodo para exibir o ranking
    static void ExibirRanking()
    {
        Console.WriteLine("\n🏆 Ranking:");
        if (ranking.Count == 0)
        {
            Console.WriteLine("Ainda não há jogadores no ranking.");
            return;
        }

        // Ordena o ranking com base na porcentagem de vitorias
        var rankingOrdenado = ranking.OrderByDescending(j => (double)j.Value.vitorias / j.Value.partidas)
                                      .ThenByDescending(j => j.Value.vitorias)
                                      .ToList();

        // Limita a exibição ao top 3
        int maxExibir = Math.Min(3, rankingOrdenado.Count);
        string[] medalhas = { "🥇", "🥈", "🥉" };

        // Exibe os colocados
        for (int i = 0; i < maxExibir; i++)
        {
            var jogador = rankingOrdenado[i];
            double porcentagem = (double)jogador.Value.vitorias / jogador.Value.partidas * 100;
            Console.WriteLine($"{medalhas[i]} {jogador.Key}: {porcentagem:F2}% de vitórias ({jogador.Value.vitorias}/{jogador.Value.partidas})");
        }

        // Verifica se a posição do jogador atual está no top 3
        int posicaoJogadorAtual = rankingOrdenado.FindIndex(j => j.Key == jogadorAtual) + 1;
        // Se não tiver, ele mostra a posição do jogador atual
        if (posicaoJogadorAtual > 3)
        {
            var jogador = ranking[jogadorAtual];
            double porcentagem = (double)jogador.vitorias / jogador.partidas * 100;
            Console.WriteLine($"📌 Seu ranking: {posicaoJogadorAtual}º lugar com {porcentagem:F2}% de vitórias ({jogador.vitorias}/{jogador.partidas}).");
        }
    }

    //Metodo para receber um novo jogador
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
        chanceRoleta = 6; // Reinicia a chance da roleta para 1/6
    }

    // Metodo para o jogador escolher qual das duas mãos vai jogar
    static int[] EscolherMaosJogador()
    {
        int[] maos = new int[2];
        for (int i = 0; i < 2; i++)
        {
            Console.Write($"Escolha a {i + 1}ª mão: ");
            maos[i] = LerEntradaValida(0, 2);
        }
        return maos; // Retorna a mão escolhida
    }

    // Metodo que define um range de valores validos
    static int LerEntradaValida(int min, int max)
    {
        int valor;
        while (true) // Loop para que o metodo não pare até ser colocado um valor valido
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

    //Metodo para garantir que o computador não vai ter duas mãos iguais
    static int[] GerarMaoComputador()
    {
        int mao1 = r.Next(0, 3); // Gera a primeira mão aleatória
        int mao2;

        do
        {
            mao2 = r.Next(0, 3); // Gera uma nova mão para a segunda mão
        } while (mao2 == mao1); // Verifica se a segunda mão é igual à primeira

        return new int[] { mao1, mao2 }; // Retorna as duas mãos do computador
    }

    // Metodo para o computador escolher a sua mão de forma estrategica
    static int EscolherMaoComputador(int[] maosComputador, int[] maosJogador)
    {
        // Analisando as mãos do computador e atribuindo um nível de 1 a 6 para cada uma
        int nivelMaoComputador1 = AnalisarMao(maosComputador[0], maosJogador);
        int nivelMaoComputador2 = AnalisarMao(maosComputador[1], maosJogador);

        // Analisa as mãos do jogador e atribui um nivel para elas
        int nivelMaoJogador1 = AnalisarMao(maosJogador[0], maosComputador);
        int nivelMaoJogador2 = AnalisarMao(maosJogador[1], maosComputador);

        int melhorMaoComputador = 0;
        int piorMaoComputador = 0;
        int melhorMaoJogador = 0;
        int maoEscolhida = 0;

        // Comparar os níveis das duas mãos do computador e escolher a melhor
        if (nivelMaoComputador1 < nivelMaoComputador2) // Se a mão 1 for melhor (nível menor)
        {
            melhorMaoComputador = maosComputador[0];
            piorMaoComputador = maosComputador[1];
        }
        else if (nivelMaoComputador1 > nivelMaoComputador2) // Se a mão 2 for melhor
        {
            melhorMaoComputador = maosComputador[1];
            piorMaoComputador = maosComputador[0];
        }
        else // Se ambas as mãos tiverem o mesmo nível, escolher aleatoriamente
        {
            melhorMaoComputador = maosComputador[0];
            piorMaoComputador = maosComputador[1];
        }

        // Comparar os níveis das duas mãos do jogador e escolher a melhor
        if (nivelMaoJogador1 < nivelMaoJogador2) // Se a mão 1 for melhor (nível menor)
        {
            melhorMaoJogador = maosJogador[0];
        }
        else if (nivelMaoJogador1 > nivelMaoJogador2) // Se a mão 2 for melhor
        {
            melhorMaoJogador = maosJogador[1];
        }
        else // Se ambas as mãos tiverem o mesmo nível, escolher aleatoriamente
        {
            melhorMaoJogador = maosJogador[r.Next(0, 2)];
        }


        if (Vence(melhorMaoComputador, melhorMaoJogador))
        {
            maoEscolhida = melhorMaoComputador;
        } 
        else if (Vence(piorMaoComputador, melhorMaoJogador))
        {
            maoEscolhida = piorMaoComputador;
        } 
        else if (melhorMaoComputador == melhorMaoJogador)
        {
            maoEscolhida = melhorMaoComputador;
        }
        else if (piorMaoComputador == melhorMaoJogador)
        {
            maoEscolhida = piorMaoComputador;
        }
        else
        {
            maoEscolhida = maosComputador[r.Next(0, 2)];
        }

        // Retorna a melhor mão do computador
        return maoEscolhida;
    }

    // Analisando o nivel de uma mão do computador em relação às mãos do jogador
    private static int AnalisarMao(int maoAnalisada, int[] maosJogadas)
    {
        // Inicializamos o nível como 0
        // Sendo o nivel 1 o melhor e 6 o pior
        int nivel = 0;

        // Comparar a mão do computador com a mão 1 do jogador
        if (Vence(maoAnalisada, maosJogadas[0])) // Se vencer, compara com a segunda mão
        {
            if (Vence(maoAnalisada, maosJogadas[1])) // Se ganhar dar duas tem o melhor nivel
                nivel = 1;
            else if (maoAnalisada == maosJogadas[1]) // Se empatar para a segunda, tem nivel 2
                nivel = 2;
            else // Se perdeu para a segunda, tem nivel 4
            {
                nivel = 4;
            }
        }

        else if (maoAnalisada == maosJogadas[0]) // Se empatar, compara com a segunda mão
        {
            if (maoAnalisada == maosJogadas[1]) // Se empatou nas duas, é nivel 3
                nivel = 3;
            else if (Vence(maoAnalisada, maosJogadas[1])) // Se ganhou da segunda, nivel 2
                nivel = 2;
            else // Se perdeu para a segunda, nivel 5
            {
                nivel = 5;
            }
        }
        else // Se perdeu para a primeira 
        {
            if (maoAnalisada == maosJogadas[1]) // Se empata da segunda mão, nivel 5
                nivel = 5;
            else if (Vence(maoAnalisada, maosJogadas[1])) // Se vence da segunda mão, nivel 4
                nivel = 4;
            else
                nivel = 6;  // Se perde para ambas as mãos, nivel 6
        }

        return nivel; // Retorna o nível calculado para essa mão
    }


    // Metodo que determia qual mão vence 
    static bool Vence(int a, int b) => (a == 0 && b == 2) || (a == 1 && b == 0) || (a == 2 && b == 1);

    // Metodo que determina o vencedor da rodada
    static int DeterminarVencedor(int jogador, int computador) => jogador == computador ? 0 : (Vence(jogador, computador) ? 1 : -1);

    // Metodo para a roleta russa caso
    static bool RoletaRussa() => r.Next(1, chanceRoleta + 1) == 1;

    // Converte a escolha numerica pra nome
    static string ConverterParaNome(int mao) => mao == 0 ? "Pedra ✊" : mao == 1 ? "Papel ✋" : "Tesoura ✌";
}