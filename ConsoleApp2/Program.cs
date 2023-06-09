﻿using System;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {

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
                deposit(amount);
            }
            else
            {
                withdraw(amount);
            }
        }

        // A method that deposits money to the account
        private static void deposit(int amount)
        {
            lock (_lock)
            {
                _balance += amount;
                Console.WriteLine(Environment.CurrentManagedThreadId+ " - Deposited {0} to the account. Balance: {1}", amount, _balance);
                Monitor.PulseAll(_lock);
            }
            
        }

        // A method that withdraws money from the account
        private static void withdraw(int amount)
        {
            lock (_lock)
            {
                if (_balance >= amount)
                {
                    _balance -= amount;
                    
                    Console.WriteLine(Environment.CurrentManagedThreadId+ " - Withdrew {0} from the account. Balance: {1}", amount, _balance);
                }
                else
                {
                    Console.WriteLine(Environment.CurrentManagedThreadId+ " - Cannot withdraw {0} from the account. Balance: {1}", amount, _balance);
                    Monitor.Wait(_lock);
                    withdraw(amount);
                }
            }
        }
    }
}