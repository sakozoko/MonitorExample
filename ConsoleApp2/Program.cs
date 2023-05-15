namespace ConsoleApp2
{
    class Program
    {
        private static Semaphore _semaphore = new Semaphore(1,1);

        private static object _lock = new object();

        private static int _balance = 0;

        static void Main(string[] args)
        {
            for (int i = 1; i <= 16; i++)
            {
                Thread t = new Thread(Worker);
                t.Start(i);
            }

            Console.ReadLine();
        }

        private static void Worker(object num)
        {
            Random rnd = new Random();
            int amount = rnd.Next(1, 100);

            if (amount % 2 == 0)
            {
                _semaphore.WaitOne();
                deposit(amount);
                _semaphore.Release();
            }
            else
            {
                while (true)
                {
                    _semaphore.WaitOne();
                    var result = withdraw(amount);
                    _semaphore.Release();
                    if (!result)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    break;
                }
            }
        }

        // A method that deposits money to the account
        private static bool deposit(int amount)
        {

            _balance += amount;
            Console.WriteLine(Environment.CurrentManagedThreadId+ " - Deposited {0} to the account. Balance: {1}", amount, _balance);
            return true;

        }

        private static bool withdraw(int amount)
        {
            if (_balance >= amount)
            {
                _balance -= amount;
                Console.WriteLine(Environment.CurrentManagedThreadId+ " - Withdrew {0} from the account. Balance: {1}", amount, _balance);
                return true;
            }
            Console.WriteLine(Environment.CurrentManagedThreadId+ " - Cannot withdraw {0} from the account. Balance: {1}", amount, _balance); 
            return false;
        }
    }
}
