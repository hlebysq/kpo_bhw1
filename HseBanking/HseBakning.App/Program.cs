using HseBanking.HseBanking.Application.Commands;
using HseBanking.HseBanking.Application.ImportExport;
using HseBanking.HseBanking.Application.Services;
using HseBanking.HseBanking.Domain.Enums;
using HseBanking.HseBanking.Domain.Models;
using HseBanking.HseBanking.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

class Program
    {
        private static IFinancialFacade _facade = null!;
        private static List<Category> _categories = new();
        private static List<BankAccount> _accounts = new();

        static void Main()
        {
            var serviceProvider = DependencyConfig.ConfigureServices();
            _facade = serviceProvider.GetRequiredService<IFinancialFacade>();
            
            InitializeSampleData();
            DisplayMainMenu();
        }

        private static void InitializeSampleData()
        {
            _categories = new List<Category>
            {
                new() { Type = OperationType.Expense, Name = "Продукты" },
                new() { Type = OperationType.Expense, Name = "Транспорт" },
                new() { Type = OperationType.Income, Name = "Зарплата" },
                new() { Type = OperationType.Income, Name = "Инвестиции" }
            };

            var mainAccountId = _facade.AddAccount("Основной счет", 5000);
            var savingsAccountId = _facade.AddAccount("Накопительный счет", 10000);
            
            _accounts = new List<BankAccount>
            {
                new() { Id = mainAccountId, Name = "Основной счет", Balance = 5000 },
                new() { Id = savingsAccountId, Name = "Накопительный счет", Balance = 10000 }
            };

            AnsiConsole.MarkupLine("[green]Инициализированы начальные данные:[/]");
            AnsiConsole.MarkupLine($"- 2 счета ({_accounts[0].Name}, {_accounts[1].Name})");
            AnsiConsole.MarkupLine($"- 4 категории ({string.Join(", ", _categories.Select(c => c.Name))})");
            AnsiConsole.WriteLine();
        }

        private static void DisplayMainMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Write(
                    new FigletText("HSE Banking")
                        .Color(Color.Blue));
                
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Выберите действие:")
                        .PageSize(10)
                        .AddChoices(new[] {
                            "Создать счет",
                            "Создать категорию",
                            "Добавить операцию",
                            "Показать баланс",
                            "Экспорт данных (JSON)",
                            "Аналитика",
                            "Выход"
                        }));
                
                switch (choice)
                {
                    case "Создать счет":
                        CreateAccount();
                        break;
                    case "Создать категорию":
                        CreateCategory();
                        break;
                    case "Добавить операцию":
                        AddOperation();
                        break;
                    case "Показать баланс":
                        ShowBalance();
                        break;
                    case "Экспорт данных (JSON)":
                        ExportData();
                        break;
                    case "Аналитика":
                        ShowAnalytics();
                        break;
                    case "Выход":
                        AnsiConsole.MarkupLine("[red]Выход из приложения...[/]");
                        return;
                }
            }
        }

        private static void CreateAccount()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Создание нового счёта[/]"));
            
            var name = AnsiConsole.Ask<string>("Название счёта:");
            var balance = AnsiConsole.Ask<decimal>("Начальный баланс:", 0m);
            
            var accountId = _facade.AddAccount(name, balance);
            _accounts.Add(new BankAccount { Id = accountId, Name = name, Balance = balance });
            
            AnsiConsole.MarkupLine($"[green]Счёт '{name}' создан! ID: {accountId}[/]");
            AnsiConsole.Ask<string>("Наберите несколько символов и нажмите Enter для продолжения...");
        }

        private static void CreateCategory()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Создание новой категории[/]"));
            
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<OperationType>()
                    .Title("Тип категории:")
                    .AddChoices(OperationType.Income, OperationType.Expense));
            
            var name = AnsiConsole.Ask<string>("Название категории:");
            
            var category = new Category
            {
                Type = type,
                Name = name
            };
            
            _categories.Add(category);
            AnsiConsole.MarkupLine($"[green]Категория '{name}' создана![/]");
            AnsiConsole.Ask<string>("Наберите несколько символов и нажмите Enter для продолжения...");
        }

        private static void AddOperation()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Добавление операции[/]"));
            
            var account = AnsiConsole.Prompt(
                new SelectionPrompt<BankAccount>()
                    .Title("Выберите счет:")
                    .UseConverter(a => a.Name)
                    .AddChoices(_accounts));
            
            var type = AnsiConsole.Prompt(
                new SelectionPrompt<OperationType>()
                    .Title("Тип операции:")
                    .AddChoices(OperationType.Income, OperationType.Expense));
            
            var category = AnsiConsole.Prompt(
                new SelectionPrompt<Category>()
                    .Title("Выберите категорию:")
                    .UseConverter(c => c.Name)
                    .AddChoices(_categories.Where(c => c.Type == type)));
            
            var amount = AnsiConsole.Ask<decimal>("Сумма:");
            
            var description = AnsiConsole.Ask<string>("Описание (необязательно):", string.Empty);
            
            var command = _facade.CreateAddOperationCommand(
                type, account.Id, category.Id, amount, description);
            
            var timedCommand = new TimedCommandDecorator(
                command, 
                msg => AnsiConsole.MarkupLine($"[grey]Время выполнения: {msg}[/]"));
            
            timedCommand.Execute();
            
            var accountIndex = _accounts.FindIndex(a => a.Id == account.Id);
            if (accountIndex != -1)
            {
                _accounts[accountIndex].Balance = type == OperationType.Income
                    ? _accounts[accountIndex].Balance + amount
                    : _accounts[accountIndex].Balance - amount;
            }
            
            AnsiConsole.MarkupLine($"[green]Операция добавлена![/]");
            AnsiConsole.Ask<string>("Наберите несколько символов и нажмите Enter для продолжения...");
        }

        private static void ShowBalance()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Баланс счетов[/]"));
            
            var table = new Table();
            table.AddColumn("Счет");
            table.AddColumn("Баланс");
            table.AddColumn("ID");
            
            foreach (var account in _accounts)
            {
                var balanceColor = account.Balance >= 0 ? "green" : "red";
                table.AddRow(
                    account.Name,
                    $"[{balanceColor}]{account.Balance:C}[/]",
                    account.Id.ToString());
            }
            
            AnsiConsole.Write(table);
            AnsiConsole.Ask<string>("Наберите несколько символов и нажмите Enter для продолжения...");
        }

        private static void ExportData()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Экспорт данных[/]"));
            
            var exporter = new JsonExporter();
            var operations = _facade.GetAllOperations();
            var json = exporter.Export(operations);
            
            var fileName = $"export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            File.WriteAllText(fileName, json);
            
            AnsiConsole.MarkupLine($"[green]Данные экспортированы в файл:[/] [underline]{fileName}[/]");
            AnsiConsole.WriteLine(json);
            AnsiConsole.Ask<string>("Наберите несколько символов и нажмите Enter для продолжения...");
        }

        private static void ShowAnalytics()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Финансовая аналитика[/]"));
            
            // Выбор счета для аналитики
            var account = AnsiConsole.Prompt(
                new SelectionPrompt<BankAccount>()
                    .Title("Выберите счет для анализа:")
                    .UseConverter(a => a.Name)
                    .AddChoices(_accounts));
            
            var balance = _facade.CalculateBalance(account.Id);
            var balanceColor = balance >= 0 ? "green" : "red";
            
            AnsiConsole.MarkupLine($"Текущий баланс: [{balanceColor}]{balance:C}[/]");
            
            var operations = _facade.GetAllOperations()
                .Where(op => op.BankAccountId == account.Id)
                .ToList();
            
            if (!operations.Any())
            {
                AnsiConsole.MarkupLine("[yellow]На этом счету нет операций[/]");
                AnsiConsole.Ask<string>("Наберите несколько символов и нажмите Enter для продолжения...");
                return;
            }
            
            var categoryStats = operations
                .GroupBy(op => op.CategoryId)
                .Select(g => new {
                    CategoryId = g.Key,
                    CategoryName = _categories.FirstOrDefault(c => c.Id == g.Key)?.Name ?? "Unknown",
                    Total = g.Sum(op => op.Amount),
                    Type = g.First().Type
                })
                .OrderByDescending(s => s.Total)
                .ToList();
            
            var barChart = new BarChart()
                .Width(60)
                .Label("[green]Расходы по категориям[/]");
            
            foreach (var stat in categoryStats.Where(s => s.Type == OperationType.Expense))
            {
                barChart.AddItem(stat.CategoryName, (double)stat.Total, Color.Red);
            }
            
            AnsiConsole.Write(barChart);
            
            var incomeTable = new Table()
                .Title("Доходы")
                .BorderColor(Color.Green)
                .AddColumn("Категория")
                .AddColumn("Сумма");
            
            foreach (var stat in categoryStats.Where(s => s.Type == OperationType.Income))
            {
                incomeTable.AddRow(stat.CategoryName, $"[green]{stat.Total:C}[/]");
            }
            
            AnsiConsole.Write(incomeTable);
            
            AnsiConsole.Ask<string>("Наберите несколько символов и нажмите Enter для продолжения...");
        }
    }