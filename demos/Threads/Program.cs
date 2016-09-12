using System;
using System.Threading;

namespace Threads
{
    public class Program
    {
        public static void Main(string[] args)
        {
            example10();
            System.Console.WriteLine("end of Main");
        }

        public static void example10(){
            //run this example with
            //dotnet run -f NET451
            //also check the project.json: we had to specify that we wanted to be compiled
            //not only for .net core but also for .net 4.51
#if NET451
            AsyncMethodCaller caller = new AsyncMethodCaller(testMethod);
            IAsyncResult result = caller.BeginInvoke(3000, 10, (asyncResult)=>{
                string returnValue = caller.EndInvoke(asyncResult);
                System.Console.WriteLine(returnValue);
            }, null);
#endif
        }

        private delegate string AsyncMethodCaller(int a, int b);
        public static void example09(){
            //run this example with
            //dotnet run -f NET451
            //also check the project.json: we had to specify that we wanted to be compiled
            //not only for .net core but also for .net 4.51
#if NET451
            AsyncMethodCaller caller = new AsyncMethodCaller(testMethod);
            //OPERATION NOT SUPPORTED ON THIS PLATFORM if coreapp10!!!
            IAsyncResult result = caller.BeginInvoke(3000, 10, null, null);
            using(WaitHandle handle = result.AsyncWaitHandle){
                result.AsyncWaitHandle.WaitOne();
                string returnValue = caller.EndInvoke(result);
                System.Console.WriteLine(returnValue);
            }
#endif
        }

        private static string testMethod(int x, int y){
            Thread.Sleep(1000);
            return $"You passed me {x} and {y}";
        }
        public static void example08(){
            for (int t = 0; t < 50; t++)
            {
                ThreadPool.QueueUserWorkItem((o)=>{
                    System.Console.WriteLine($"{o} - {Thread.CurrentThread.ManagedThreadId}");
                },t);    
            }
        }

        private static void example07(){
            Semaphore s = new  Semaphore(3,3);// empty, with a max of 3 running threads
            for (int i = 0; i < 20; i++)
            {
                new Thread(()=>{
                    System.Console.WriteLine($"\t{Thread.CurrentThread.ManagedThreadId} WAITING");
                    s.WaitOne();
                    System.Console.WriteLine($"\t{Thread.CurrentThread.ManagedThreadId} STARTED");
                    for (int counter = 0; counter < 10; counter++)
                    {
                        System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} {counter}");    
                    }
                    System.Console.WriteLine($"\t{Thread.CurrentThread.ManagedThreadId} ENDED");
                    s.Release();    
                    System.Console.WriteLine($"\t{Thread.CurrentThread.ManagedThreadId} RELEASED");
                }).Start();
            }
        }
        private static void example06(){
            EventWaitHandle[] waitHandles = new EventWaitHandle[]{new AutoResetEvent(false), new AutoResetEvent(false), new AutoResetEvent(false)};
            Random r = new Random();

            new Thread(()=>{
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
                    Thread.Sleep(r.Next(100));
                }
                waitHandles[0].Set();
                System.Console.WriteLine("0 SET");
            }).Start();

            new Thread(()=>{
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
                    Thread.Sleep(r.Next(100));
                }
                waitHandles[1].Set();
                System.Console.WriteLine("1 SET");
            }).Start();

            new Thread(()=>{
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
                    Thread.Sleep(r.Next(100));
                }
                waitHandles[2].Set();
                System.Console.WriteLine("2 SET");
            }).Start();

            //System.Console.WriteLine($"{EventWaitHandle.WaitAny(waitHandles)} FINISHED FIRST!");
            EventWaitHandle.WaitAll(waitHandles); 
            System.Console.WriteLine("All threads done");
        }

        private static void example05(){
            Buffer buf = new Buffer();
            new Thread(()=>{
                for (int i = 0; i < 10; i++) {
                    buf.Add(i.ToString());
                }
            }).Start();
            
            new Thread(()=>{
                for (int i = 0; i < 10; i++) {
                    System.Console.WriteLine(buf.Remove());
                }
            }).Start();

            
        }

        private static void example04(){
            BankAccount b1 = new BankAccount(){Id=1};
            BankAccount b2 = new BankAccount(){Id=2};
            
            Bank bank = new Bank();
            b1.Deposit(500);

            System.Console.WriteLine($"b1.Saldo: {b1.Saldo}");
            System.Console.WriteLine($"b2.Saldo: {b2.Saldo}");
            
            //this causes no problems
            //bank.Transfer(b1,b2,200);
            //bank.Transfer(b2,b1,100);


            //we could lose money here!
            // new Thread(()=>{
            //     bank.Transfer(b1,b2,200);    
            // }).Start();

            // new Thread(()=>{
            //     bank.Transfer(b2,b1,100);    
            // }).Start();
            

            //possible deadlock here!
            // new Thread(()=>{
            //     bank.TransferDeadLock(b1,b2,200);    
            // }).Start();

            // new Thread(()=>{
            //     bank.TransferDeadLock(b2,b1,100);    
            // }).Start();
            
            new Thread(()=>{
                bool ok = bank.TransferTryEnter(b1,b2,200);
                System.Console.WriteLine($"First try went {(ok ? "ok" : "not ok!")}"); 
            }).Start();

            new Thread(()=>{
                bool ok = bank.TransferTryEnter(b2,b1,100);    
                System.Console.WriteLine($"Second try went {(ok ? "ok" : "not ok!")}");
            }).Start();

        }
        private static void example03(){
            new Thread(DoLocked).Start();
            new Thread(DoLocked).Start();
        }
        private static void example02(){
            new Thread(Do).Start();
            new Thread(Do).Start();
        }
        private static void example01()
        {
            Thread t = new Thread(longFunction);
            t.Start();

            Thread t2 = new Thread(longFunctionWithStuff);
            t2.Start("Hi!");
        }

        public static void longFunction() { 
            for (int i = 0; i < 10; i++)
            {
                System.Console.WriteLine("Long Function " + i);
                Thread.Sleep(500);
            }
        }

        public static void longFunctionWithStuff(object stuff) { 
            for (int i = 0; i < 10; i++)
            {
                System.Console.WriteLine($"Long Function With Stuff ({stuff.ToString()}) {i}");
                Thread.Sleep(500);
            }
        }

        private static bool done;

        private static void Do(){
            if(!done){
                
                System.Console.WriteLine("Done!");
                done=true;
            }
        }

        private static void DoLocked(){
            lock(typeof(Program)){
                if(!done){    
                    System.Console.WriteLine("Done!");
                    done=true;
                }
            }
        }
    }
}
