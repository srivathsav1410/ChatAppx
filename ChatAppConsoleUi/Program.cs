using ChatAppDataAccessLayer;
using ChatAppDataAccessLayer.Models;

namespace ChatAppConsoleUi
{
    public class Program
    {
        static ChatAppDbContext context;
        static ChatAppRepository repository;

        static Program()
        {
            context = new ChatAppDbContext();
            repository = new ChatAppRepository(context);
        }

        static void Main(string[] args)
        {
            var users = repository.GetAllCategories();
            foreach (var user in users)
            {
                Console.WriteLine($"User ID: {user.UserId}, Email: {user.Email}");
            }


        }
    }
    }

