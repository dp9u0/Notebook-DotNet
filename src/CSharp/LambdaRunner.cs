using Common;
using System;

namespace CSharp {
    class LambdaRunner : Runner {
        protected override void RunCore() {
            Console.WriteLine("-----------------------");
            Test001();
            Console.WriteLine("-----------------------");
            Test002();
            Console.WriteLine("-----------------------");
            Test003();
            Console.WriteLine("-----------------------");
            Test004();
            Console.WriteLine("-----------------------");
            Test005();
            Console.WriteLine("-----------------------");
            Test006();
            Console.WriteLine("-----------------------");
            Test007();
        }

        private int _global;

        public void Test001() {
            Action<string> DoSomethingLambda = s => { Console.WriteLine(s); };
        }
        public void Test001_V2() {
            Action<string> DoSomethingLambda2 = delegate (string s) {
                Console.WriteLine(s);
            };
        }

        public void Test002() {
            int local = 5;

            Action<string> DoSomethingLambdaClosure = s => { Console.WriteLine(s + local); };

            local += 1;

            _global = local;

            DoSomethingLambdaClosure("Test 1");
            DoSomethingNormalClosure("Test 2");
        }

        public void Test003() {
            int local = 5;

            Action<string> DoSomethingLambdaClosure = s => { Console.WriteLine(s + _global); };

            local += 1;

            _global = local;

            DoSomethingLambdaClosure("Test 1");
            DoSomethingNormalClosure("Test 2");
        }

        public void DoSomethingNormalClosure(string s) {
            Console.WriteLine(s + _global);
        }


        public void DoSomethingNormal(string s) {
            Console.WriteLine(s);
        }


        public void Test004() {
            // Action<string> ask = new Action<string>(LambdaRunner.<>c.<>9.<Test004>b__8_0);
            Action<string> action = (quen) => { Console.WriteLine("The answer to `" + quen + "` is certainly 42!"); };
            //Create anonymous object
            var person = new {
                Name = "Jesse",
                Age = 28,
                //错误  CS0828  无法将 lambda 表达式 赋予匿名类型属性 DelegateLambda
                Ask = action
            };

            var p2 = new Persion();

            //Execute function
            //person.Ask("Why are you doing this?");
        }

        public class Persion {

            public void Ask() {

            }

        }

        public void Test005() {
            var person = new {
                Name = "Florian",
                Age = 28,
                Ask =
                (Action<string>)
                (question => { Console.WriteLine("The answer to `" + question + "` is certainly 42!"); })
            };
            //Execute function
            person.Ask("Why are you doing this?");
        }


        public void Test006() {
            dynamic person = null;
            person = new {
                Name = "Jesse",
                Age = 28,
                Ask =
                (Action<string>)
                (question =>
                {
                    Console.WriteLine("The answer to `" + question + "` is certainly 42! My age is " + person.Age + ".");
                })
            };

            //Execute function
            person.Ask("Why are you doing this?");
        }


        public void Test007() {
            var mother = CoolMother.Activator().Message;
            //mother = "I am the mother"
            var create = new HotDaughter();
            var daughter = HotDaughter.Activator().Message;
            //daughter = "I am the daughter"
            var mother2 = CoolMother.Activator().Message;
            Console.WriteLine(mother);
            Console.WriteLine(daughter);
            Console.WriteLine(mother2);
        }
        internal class CoolMother {
            //We are only doing this to avoid NULL references!
            static CoolMother() {
                Activator = () => new CoolMother();
            }

            public CoolMother() {
                //Message of every mother
                Message = "I am the mother";
            }

            public static Func<CoolMother> Activator { get; protected set; }

            public string Message { get; protected set; }
        }

        internal class HotDaughter : CoolMother {
            public HotDaughter() {
                //Once this constructor has been "touched" we set the Activator ...
                Activator = () => new HotDaughter();
                //Message of every daughter
                Message = "I am the daughter";
            }

        }
    }

}
