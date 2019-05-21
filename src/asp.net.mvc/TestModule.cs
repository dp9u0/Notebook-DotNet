using System.Web;

namespace asp.net.mvc
{

    public class TestModule : IHttpModule
    {

        public void Init(HttpApplication context)
        {
            //Thread.Sleep(30000);
            //try
            //{
            //    throw new System.NotImplementedException();
            //} catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
        }

        public void Dispose() { }

    }

}