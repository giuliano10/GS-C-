using System;
using System.Collections.Generic;
using System.IO;
using CyberImpactMonitor.Models;

namespace CyberImpactMonitor
{
    class Program
    {
        static List<Subestacao> subestacoes = new List<Subestacao>();
        static List<string> logs = new List<string>();
        static Dictionary<string, string> usuarios = new Dictionary<string, string>();
        static string usuarioLogado = "";

        static void Main(string[] args)
        {
            InicializarUsuarios();
            if (!LoginOuCadastro())
                return;

            InicializarSubestacoes();

            bool sair = false;
            while (!sair)
            {
                Console.Clear();
                MostrarMenu();

                string? opcao = Console.ReadLine();
                switch (opcao?.Trim())
                {
                    case "1": RegistrarFalha(); break;
                    case "2": MostrarStatus(); break;
                    case "3": RestaurarSubestacao(); break;
                    case "4": MostrarLogs(); break;
                    case "5": VerAlertas(); break;
                    case "6": GerarRelatorio(); break;
                    case "7": sair = true; break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

                if (!sair)
                {
                    Console.WriteLine("\nPressione ENTER para continuar...");
                    Console.ReadLine();
                }
            }
        }

        static void InicializarUsuarios()
        {
            usuarios["admin"] = "1234";
        }

        static bool LoginOuCadastro()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Tela de Login / Cadastro ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Cadastrar novo usuário");
                Console.Write("Escolha uma opção (1 ou 2): ");
                string? escolha = Console.ReadLine()?.Trim();

                if (escolha == "1")
                {
                    Console.Write("Usuário: ");
                    string usuario = Console.ReadLine();
                    Console.Write("Senha: ");
                    string senha = Console.ReadLine();

                    if (usuarios.ContainsKey(usuario) && usuarios[usuario] == senha)
                    {
                        usuarioLogado = usuario;
                        Log($"Login realizado por {usuario}");
                        return true;
                    }

                    Console.WriteLine("❌ Credenciais inválidas. Tente novamente.\n");
                    Console.WriteLine("Pressione ENTER para voltar...");
                    Console.ReadLine();
                }
                else if (escolha == "2")
                {
                    Console.Write("Novo usuário: ");
                    string novoUsuario = Console.ReadLine()?.Trim();
                    Console.Write("Senha: ");
                    string novaSenha = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(novoUsuario) || string.IsNullOrWhiteSpace(novaSenha))
                    {
                        Console.WriteLine("❌ Usuário ou senha inválidos.");
                        Console.WriteLine("Pressione ENTER para voltar...");
                        Console.ReadLine();
                        continue;
                    }

                    if (!usuarios.ContainsKey(novoUsuario))
                    {
                        usuarios[novoUsuario] = novaSenha;
                        Console.WriteLine("✅ Usuário cadastrado com sucesso.");
                    }
                    else
                    {
                        Console.WriteLine("❌ Usuário já existe.");
                    }

                    Console.WriteLine("Pressione ENTER para voltar ao menu de login...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("❌ Opção inválida. Digite 1 ou 2.\n");
                    Console.WriteLine("Pressione ENTER para tentar novamente...");
                    Console.ReadLine();
                }
            }
        }



        static void InicializarSubestacoes()
        {
            subestacoes.Add(new Subestacao("Subestação Norte"));
            subestacoes.Add(new Subestacao("Subestação Sul"));
            subestacoes.Add(new Subestacao("Subestação Leste"));
            subestacoes.Add(new Subestacao("Subestação Oeste"));
            subestacoes.Add(new Subestacao("Subestação Central"));
        }

        static void MostrarMenu()
        {
            Console.WriteLine("=== Sistema de Monitoramento ===");
            Console.WriteLine("1. Registrar Falha");
            Console.WriteLine("2. Ver Relatório de Status");
            Console.WriteLine("3. Restaurar Subestação");
            Console.WriteLine("4. Ver Logs de Eventos");
            Console.WriteLine("5. Ver Alertas");
            Console.WriteLine("6. Gerar Relatório");
            Console.WriteLine("7. Sair");
            Console.Write("Escolha uma opção: ");
        }

        static void RegistrarFalha()
        {
            Console.WriteLine("=== Registro de Falha ===");
            ExibirSubestacoes();

            while (true)
            {
                Console.Write("Selecione o número da subestação com falha: ");
                try
                {
                    int escolha = int.Parse(Console.ReadLine()!);
                    if (escolha < 1 || escolha > subestacoes.Count)
                        throw new ArgumentOutOfRangeException();

                    subestacoes[escolha - 1].MarcarFalha();
                    Log($"Falha registrada em {subestacoes[escolha - 1].Nome}");
                    Console.WriteLine("⚠️ Falha registrada com sucesso!");
                    break;
                }
                catch
                {
                    Console.WriteLine("❌ Entrada inválida. Tente novamente.\n");
                }
            }
        }

        static void RestaurarSubestacao()
        {
            Console.WriteLine("=== Restaurar Subestação ===");
            ExibirSubestacoes();

            while (true)
            {
                Console.Write("Selecione o número da subestação a restaurar: ");
                try
                {
                    int escolha = int.Parse(Console.ReadLine()!);
                    if (escolha < 1 || escolha > subestacoes.Count)
                        throw new ArgumentOutOfRangeException();

                    subestacoes[escolha - 1].Restaurar();
                    Log($"Subestação restaurada: {subestacoes[escolha - 1].Nome}");
                    Console.WriteLine("✅ Subestação restaurada com sucesso!");
                    break;
                }
                catch
                {
                    Console.WriteLine("❌ Entrada inválida. Tente novamente.\n");
                }
            }
        }

        static void ExibirSubestacoes()
        {
            for (int i = 0; i < subestacoes.Count; i++)
                Console.WriteLine($"{i + 1}. {subestacoes[i].Nome} - Status: {subestacoes[i].Status}");
        }

        static void MostrarStatus()
        {
            Console.WriteLine("=== Relatório de Status ===");
            foreach (var s in subestacoes)
                Console.WriteLine($"{s.Nome} - Status: {s.Status}");
        }

        static void VerAlertas()
        {
            Console.WriteLine("=== ALERTAS ATIVOS ===");
            foreach (var s in subestacoes)
            {
                if (s.AlertaCritico)
                    Console.WriteLine($"🚨 {s.Nome} está em estado crítico!");
            }
        }

        static void MostrarLogs()
        {
            Console.WriteLine("=== LOGS DO SISTEMA ===");
            foreach (var log in logs)
                Console.WriteLine(log);
        }

        static void GerarRelatorio()
        {
            string nomeArquivo = $"relatorio_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(nomeArquivo))
                {
                    sw.WriteLine("=== RELATÓRIO DE SUBESTAÇÕES ===");
                    foreach (var s in subestacoes)
                        sw.WriteLine($"{s.Nome} - Status: {s.Status}");

                    sw.WriteLine("\n=== LOGS ===");
                    foreach (var log in logs)
                        sw.WriteLine(log);
                }

                Console.WriteLine($"📄 Relatório gerado: {nomeArquivo}");
                Log("Relatório gerado");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao gerar o relatório: " + ex.Message);
            }
        }

        static void Log(string mensagem)
        {
            string entrada = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {mensagem}";
            logs.Add(entrada);
        }
    }
}
