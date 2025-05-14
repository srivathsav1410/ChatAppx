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
            User u=new User();
            bool res = repository.AddUser(u);
            Console.WriteLine(res);
        }
    }
    }

