using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSystemTest
{
   
    public class TestUtil
    {
   
        public static bool EvaluateEquality<T>(T returnObject, T testObject)
        {
            string returnObjectJson = JsonConvert.SerializeObject(returnObject);
            string testObjectJson = JsonConvert.SerializeObject(testObject);
            return returnObjectJson == testObjectJson;
        }
    }
   
}
