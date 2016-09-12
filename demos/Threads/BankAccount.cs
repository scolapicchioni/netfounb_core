using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Threads
{
    public class BankAccount
    {
        public BankAccount()
        {
        }
        public int Id { get; set; }
        public double Saldo { get; private set; }
        public double Deposit(double amount) {
            if(amount<0) amount=0;
            Saldo += amount;
            return amount;
        }

        public double Withdraw(double amount) {
            if(amount<0) amount=0;
            if(amount>Saldo) amount=Saldo;  
            Saldo -= amount;
            return amount;
        }

        public override string ToString(){
            return $"{this.Id} - Saldo: {this.Saldo}";
        } 
    }
}
