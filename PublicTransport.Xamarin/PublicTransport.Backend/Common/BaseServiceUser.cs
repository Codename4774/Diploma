namespace PublicTransport.Backend.Common
{
    public abstract class BaseServiceUser
    {
        public BaseServiceUser()
        {
            InitializeServices();
        }


        protected abstract void InitializeServices();
    }
}
