using System;

namespace DoctorSystem.Jobs
{
    public class MyJob
    {
        public MyJob(Type type,string expression)
        {
            Console.WriteLine("EmailJob at {0}", DateTime.Now);

            Type = type;
            Expression = expression;
        }

        public Type Type { get; }
        public string Expression { get; }
    }
}
