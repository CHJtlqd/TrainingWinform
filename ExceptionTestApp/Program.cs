using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int x = 100, y = 2, value = 0;


            //Console.WriteLine("value = {0}", value);
            //Console.WriteLine($"value = {value}");
            //Console.WriteLine($"{x} / {y} = {value}");

            try
            {
                value = x / y;
                Console.WriteLine($"{x}/{y}={value}");
                throw new Exception("사용자 에러");      // 프로그래머가 발생시킨 에러
            }
            catch (DivideByZeroException e)
            {

                Console.WriteLine("2. y의 값을 0보다 크게 입력하세요.");
            }
            catch (Exception e)
            {
                Debug.WriteLine("3. "+e.ToString());  //Debug 창에서 확인가능한 출력
                Console.WriteLine("3. "+e.Message);
            }
            finally
            {
                Console.WriteLine("4. 프로그램이 종료했습니다.");
            }

        }
    }
}
