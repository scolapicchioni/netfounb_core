using System;
using System.Threading;

namespace Threads
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Start of Main");
            example08();
            System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} End of Main");
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
            CountdownEvent ce = new CountdownEvent(50);
            for (int t = 0; t < 50; t++)
            {
                ThreadPool.QueueUserWorkItem((o)=>{
                    System.Console.WriteLine($"{o} - {Thread.CurrentThread.ManagedThreadId}");
                    ce.Signal();
                },t);    
            }
            ce.Wait();
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

        private static void example06_2(){
            CountdownEvent ce = new CountdownEvent(3);
            Random r = new Random();

            new Thread(()=>{
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
                    Thread.Sleep(r.Next(100));
                }
                ce.Signal();
                System.Console.WriteLine("0 SET");
            }).Start();

            //waitHandles[0].WaitOne();
            new Thread(()=>{
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
                    Thread.Sleep(r.Next(100));
                }
                ce.Signal();
                System.Console.WriteLine("1 SET");
            }).Start();

            new Thread(()=>{
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
                    Thread.Sleep(r.Next(100));
                }
                ce.Signal();
                System.Console.WriteLine("2 SET");
            }).Start();

            System.Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: is going to wait...");
            ce.Wait();
            System.Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: 3 Times Set (All threads done)");
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

            //waitHandles[0].WaitOne();
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
            // Do();
            // Do();

            new Thread(Do).Start();
            new Thread(Do).Start();
        }
        private static void example01()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Example 01");
            //longFunction();
            // Thread t = new Thread(longFunction);
            // t.IsBackground = false;
            // t.Start();
            // Thread t2 = new Thread(longFunctionWithStuff);
            // t2.Start("Hi!");
            // t2.IsBackground = false;
            
            // Thread t2 = new Thread(anotherMethod);
            // t.Start();
            
            // Thread t3 = new Thread(methodToInvokeAMethodWithTwoParams);
            // t3.Start(new ClassForExample01(){X = 5, Y = 10});

            int x = 5;
            int y = 10;
            
            Thread t4 = new Thread( () => {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} methodToInvokeAMethodWithTwoParams x: {x}, y: {y}");
                int result = anotherMethod(x, y);
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} end of methodToInvokeAMethodWithTwoParams x: {x}, y: {y}");
            });
            t4.Start();
            t4.Join();
            //Console.WriteLine(result);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} End of Example 01");
            
        }

        public static void methodToInvokeAMethodWithTwoParams(object value){
            ClassForExample01 val = (ClassForExample01)value;
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} methodToInvokeAMethodWithTwoParams x: {val.X}, y: {val.Y}");
            int result = anotherMethod(val.X, val.Y);
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} end of methodToInvokeAMethodWithTwoParams x: {val.X}, y: {val.Y}");
        }

        public static int anotherMethod(int x, int y){
            return x + y;
        }

        public static void aMethodWithTwoParams(int x, int y){
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} aMethodWithTwoParams x: {x}, y: {y}");
        }

        public static void longFunction() { 
            for (int i = 0; i < 10; i++)
            {
                System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Long Function " + i);
                Thread.Sleep(500);
            }
        }

        public static void longFunctionWithStuff(object stuff) { 
            for (int i = 0; i < 10; i++)
            {
                System.Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Long Function With Stuff ({stuff.ToString()}) {i}");
                Thread.Sleep(500);
            }
        }

        private static bool done;

        private static void Do(){
            if(!done){
                System.Console.WriteLine("Done!");
                //Thread.Sleep(1);
                done=true;
            }
        }

        static object key = new object();
        private static void DoLocked(){
            //Monitor.Enter(key);
            lock(key){
                if(!done){    
                    System.Console.WriteLine("Done!");
                    done=true;
                }
            }
            //Monitor.Exit(key);
        }
    }
}
