using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //parallelFor();
            //parallelForEach();
            //parallelInvoke();
            //task04();
            //plinq01();
            //blockingCollections01();
            countdown();
            System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} DONE");
        }

        private static void countdown()
        {
            CountdownEvent countdown = new CountdownEvent(3);
            Task.Run(async ()=>{
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} STARTED");
                await Task.Delay(500);
                countdown.Signal();
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} DONE!");
            });

            Task.Run(async ()=>{
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} STARTED");
                await Task.Delay(300);
                countdown.Signal();
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} DONE!");                
            });

            Task.Run(async ()=>{
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} STARTED");
                await Task.Delay(800);
                countdown.Signal();
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} DONE!");
            });
            countdown.Wait();
            System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} THE END");
        }

        private static void blockingCollections01()
        {
            BlockingCollection<string> buf = new BlockingCollection<string>();
            Task.Run(()=>{
                for (int i = 0; i < 10; i++) {
                    System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} Adding {i}");
                    buf.Add(i.ToString());
                }
            });
            
            Task.Run(()=>{
                for (int i = 0; i < 10; i++) {
                    string res = buf.Take();
                    System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} Taken {res}");
                }
            });
        }

        private static void plinq01(){
            var numbers = Enumerable.Range(1, 10000);
             
            var evenNumbers = from number in numbers.AsParallel()
                            where number % 2 == 0
                            select number;

            foreach (var number in evenNumbers) {
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} {number}");
            }
        }
        private static void task04(){
            CancellationTokenSource cts = new CancellationTokenSource();
            
            CancellationToken token = cts.Token;

            
            
             
            Task myTask = Task.Factory.StartNew(() =>
            {
                while(true) {
                    token.ThrowIfCancellationRequested();
                    System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} Working");
                }
            }, token);

            System.Console.ReadLine();
            cts.Cancel();
        
        }
        private static void task03(){
            Task<double>[] tasks = new Task<double>[]
            {
                Task<double>.Factory.StartNew(method2),
                Task<double>.Factory.StartNew(method2),
            };
             
            int indexOfFinishedTask = Task.WaitAny(tasks, 10000);
            System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} - indexOfFinishedTask == {indexOfFinishedTask} - result is {tasks[indexOfFinishedTask].Result}");
            bool allReturned = Task.WaitAll(tasks, 5000);
            System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} - all returned == {allReturned}");
            
        }

        private static double method2(){
            System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} - 42");
            return 42;
        }
        private static void task02(){
            Task<int> task = new Task<int>( () => {
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} - 42");   
                return 42;
            });
            task.Start();
            // Accessing Result will block
            // until Task has completed
            Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} - {task.Result}");
        }

        private static void task01()
        {
            Task task = new Task( () => {
                for (int i = 0; i < 10; i++)
                {
                    System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} - {i}");    
                }
            });
            task.Start();

        }

        private static void parallelInvoke()
        {
            Parallel.Invoke(method1, method1, method1);
        }

        private static void parallelForEach()
        {
            int[] collection = new int[]{0,1,2,3,4,5,6,7,8,9};
            foreach (var item in collection)
            {
                process(item);
            }

            // Parallel 
            Parallel.ForEach(collection, item => process(item));
        }

        private static void parallelFor()
        {
            for (int index = 0; index < 10; index++)
            {
                process(index);
            }

            // Parallel 
            Parallel.For(0, 10, (index) => process(index));
        }

        private static void process(int i){
            System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} - Counter: {i}");
        }

        private static void method1(){
            for (int i = 0; i < 10; i++)
            {
                System.Console.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId} - Counter: {i}");
            }
        }
    }
}
