namespace TestInterfaceImplementation
{
    public class Class2 : Interface2, Interface3
    {
        int Interface2.Method(int i)
        {
            throw new System.NotImplementedException();
        }

        public int Method2(float i)
        {
            throw new System.NotImplementedException();
        }

        public int Method2(double i)
        {
            throw new System.NotImplementedException();
        }

        int Interface3.Method(int i)
        {
            throw new System.NotImplementedException();
        }
    }
}