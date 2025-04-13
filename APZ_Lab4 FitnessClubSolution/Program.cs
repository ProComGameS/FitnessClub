using FitnessClub.DAL;
using FitnessClub.BLL.Services;
using Microsoft.EntityFrameworkCore;
using FitnessClub.DAL.Models;


namespace FitnessClub.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Налаштування контексту бази даних
            var optionsBuilder = new DbContextOptionsBuilder<FitnessClubContext>();
            optionsBuilder.UseSqlite("Data Source=fitnessclub.db");

            using (var context = new FitnessClubContext(optionsBuilder.Options))
            {
                context.Database.EnsureCreated(); // Гарантує створення бази

                IUnitOfWork unitOfWork = new UnitOfWork(context);
                ClubService clubService = new ClubService(unitOfWork);
                MemberService memberService = new MemberService(unitOfWork);

                SeedData(context, clubService, memberService);
                ShowMenu(clubService, memberService);
            }
        }

        private static void SeedData(FitnessClubContext context, ClubService clubService, MemberService memberService)
        {
            if (context.Clubs.Any()) return;

            Club club1 = new Club { Name = "Fitness Club A", Location = "Центр" };
            Club club2 = new Club { Name = "Fitness Club B", Location = "Північ" };
            clubService.CreateClub(club1);
            clubService.CreateClub(club2);

            Member member1 = new Member { FullName = "Іван Іванов", Email = "ivan@example.com" };
            memberService.CreateMember(member1);

            ClassSession session1 = new ClassSession { SessionDateTime = DateTime.Now.AddHours(2), Capacity = 20, ClubId = club1.Id };
            ClassSession session2 = new ClassSession { SessionDateTime = DateTime.Now.AddDays(1), Capacity = 15, ClubId = club1.Id };

            context.ClassSessions.AddRange(session1, session2);
            context.SaveChanges();

            Console.WriteLine("Дані початкової ініціалізації додано.");
        }

        private static void ShowMenu(ClubService clubService, MemberService memberService)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n------ Фітнес клуб ------");
                Console.WriteLine("1. Переглянути клуби");
                Console.WriteLine("2. Зареєструватись на заняття");
                Console.WriteLine("3. Купити абонемент");
                Console.WriteLine("4. Вихід");
                Console.Write("Оберіть пункт: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1": ShowClubs(clubService); break;
                    case "2": RegisterSession(clubService); break;
                    case "3": BuySubscription(clubService); break;
                    case "4": exit = true; break;
                    default: Console.WriteLine("Невірний пункт меню. Спробуйте знову."); break;
                }
            }
        }

        private static void ShowClubs(ClubService clubService)
        {
            Console.WriteLine("\nСписок клубів:");
            var clubs = clubService.GetAllClubs();
            foreach (var club in clubs)
            {
                Console.WriteLine($"ID: {club.Id}, Назва: {club.Name}, Локація: {club.Location}");
                if (club.Sessions != null)
                {
                    foreach (var session in club.Sessions)
                    {
                        Console.WriteLine($"\tСесія ID: {session.Id}, Час: {session.SessionDateTime}, Місткість: {session.Capacity}");
                    }
                }
            }
        }

        private static void RegisterSession(ClubService clubService)
        {
            Console.Write("Введіть ID сесії: ");
            if (!int.TryParse(Console.ReadLine(), out int sessionId))
            {
                Console.WriteLine("Невірний формат ID.");
                return;
            }

            Console.Write("Введіть ID вашої клубної картки: ");
            if (!int.TryParse(Console.ReadLine(), out int cardId))
            {
                Console.WriteLine("Невірний формат ID.");
                return;
            }

            bool result = clubService.RegisterToSession(sessionId, cardId);
            Console.WriteLine(result ? "Реєстрація пройшла успішно." : "Не вдалося зареєструватись. Перевірте умови заняття.");
        }

        private static void BuySubscription(ClubService clubService)
        {
            Console.Write("Введіть ID клубу для купівлі абонементу: ");
            if (!int.TryParse(Console.ReadLine(), out int clubId))
            {
                Console.WriteLine("Невірний формат ID.");
                return;
            }

            Console.Write("Введіть ID члена клубу: ");
            if (!int.TryParse(Console.ReadLine(), out int memberId))
            {
                Console.WriteLine("Невірний формат ID.");
                return;
            }

            Console.WriteLine("Оберіть тип абонементу: 1 - OneTime, 2 - Subscription, 3 - NetworkSubscription");
            string choice = Console.ReadLine();
            CardType cardType = choice switch
            {
                "1" => CardType.OneTime,
                "2" => CardType.Subscription,
                "3" => CardType.NetworkSubscription,
                _ => throw new InvalidOperationException("Невірний вибір.")
            };

            bool result = clubService.BuySubscription(clubId, memberId, cardType);
            Console.WriteLine(result ? "Абонемент куплено успішно." : "Помилка при купівлі абонементу.");
        }
    }
}
