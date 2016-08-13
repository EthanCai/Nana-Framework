using System;
using System.Threading;

namespace Nana.Framework.Async
{
    public class TwoTest<T>
    {
        public delegate T SleepDelegate(int time, T value);

        //可以用这个委托 用委托的时候不用 s_wait
        public SleepDelegate Sleep;

        //也可以直接用这个方法
        public T MethodDo(int time, T value)
        {
            Console.WriteLine(time + "方法里的时间");
            Thread.Sleep(time);

            Console.WriteLine(value.GetType() + "   " + value.ToString());

            return value;
        }

        public TwoTest()
        {
            Sleep = MethodDo;
        }
    }

    public class Resultclass
    {
        public int Reti;
        public string Rets;
    }

    public class AsyncTaskPattern1
    {


        public AsyncTaskDispatcher dispatcher = new AsyncTaskDispatcher(false);
        private static Action<int> s_wait = (second) => Thread.Sleep(second * 1000);
        public void Test()
        {
            //各个要并行的方法的准备
            TwoTest<int> TTint = new TwoTest<int>();
            TwoTest<string> TTStr = new TwoTest<string>();

            //返回值的准备
            Resultclass res = new Resultclass();


            //注册任务
            dispatcher.RegisterTask(
                "TTINT",
                (cb, state, context) => (s_wait.BeginInvoke(0, cb, state)),
                (ar, cancelling, context) =>
                {
                    res.Reti = TTint.MethodDo(100, 2000);
                    s_wait.EndInvoke(ar);
                });

            dispatcher.RegisterTask(
                "TTStr",
                (cb, state, context) => (s_wait.BeginInvoke(0, cb, state)),
                (ar, cancelling, context) =>
                {
                    res.Rets = TTStr.MethodDo(2223, "3333");
                    s_wait.EndInvoke(ar);
                });
            //运行任务
            TaskHelper.Execute(dispatcher);
            //   收集结果
            Console.WriteLine(res.Reti);
            Console.WriteLine(res.Rets);
        }

    }

    public class AsyncTaskPattern2
    {


        public AsyncTaskDispatcher dispatcher = new AsyncTaskDispatcher(false);
   
        public void Test()
        {
            //各个要并行的方法的准备
            TwoTest<int> TTint = new TwoTest<int>();
            TwoTest<string> TTStr = new TwoTest<string>();

            //返回值的准备
            Resultclass res = new Resultclass();


            //注册任务
            dispatcher.RegisterTask(
                "TTINT",
                (cb, state, context) => (TTint.Sleep.BeginInvoke(1000,2000, cb, state)),
                (ar, cancelling, context) =>
                {
                    res.Reti = TTint.Sleep.EndInvoke(ar);
                  
                });

            dispatcher.RegisterTask(
                "TTStr",
                (cb, state, context) => (TTStr.Sleep.BeginInvoke(1000, "2000", cb, state)),
                (ar, cancelling, context) =>
                {
                    res.Rets = TTStr.Sleep.EndInvoke(ar);
                });
            //运行任务
            TaskHelper.Execute(dispatcher);
            //   收集结果
            Console.WriteLine(res.Reti);
            Console.WriteLine(res.Rets);
        }

    }
}