using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Threads
{
    public class Bank
    {
        public Bank()
        {
        }

        public void Transfer(BankAccount @from, BankAccount to, double amount){
            System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Transferring {amount} from {@from} to {to}");
            amount = @from.Withdraw(amount);
            System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Withdrawn {amount} from {@from}");
            to.Deposit(amount);
            System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Deposited. from: {@from} to: {to}");
        }

        public void TransferDeadLock(BankAccount @from, BankAccount to, double amount){
            System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} started. locking {@from.Id}...");
            lock(@from){
                System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} {@from.Id} locked. locking {to.Id}...");
                lock(to){
                    System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Transferring {amount} from {@from} to {to}");
                    amount = @from.Withdraw(amount);
                    System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Withdrawn {amount} from {@from}");
                    to.Deposit(amount);
                    System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Deposited. from: {@from} to: {to}");
                }
            }
        }

        public bool TransferTryEnter(BankAccount @from, BankAccount to, double amount){
            bool from_taken = false;
            bool to_taken = false;
            System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} started. locking {@from.Id}...");
            
            Monitor.TryEnter(@from,500,ref from_taken);
            if(from_taken){
                System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} {@from.Id} locked. locking {to.Id}...");
                Monitor.TryEnter(to,500,ref to_taken);
                if(to_taken){
                    System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Transferring {amount} from {@from} to {to}");
                    amount = @from.Withdraw(amount);
                    System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Withdrawn {amount} from {@from}");
                    to.Deposit(amount);
                    System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Deposited. from: {@from} to: {to}");
                    Monitor.Exit(to);
                }else{
                    return false;
                }
                Monitor.Exit(@from);
            }else{
                return false; 
            }
            return true;
        }
    }
}
