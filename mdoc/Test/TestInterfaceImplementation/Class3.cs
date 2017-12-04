namespace TestInterfaceImplementation
{
    public class Class3 : Interface2, Interface3
    {
        public int Method(int i)
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
    }
}