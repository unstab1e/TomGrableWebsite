namespace TomGrable.Website.IoC
{
    public static class Container
    {
        private static object _Mutex = new object();
        private static SimpleInjector.Container _Current = null;
        public static SimpleInjector.Container Current
        {
            get
            {
                if (_Current == null)
                {
                    lock (_Mutex)
                    {
                        if (_Current == null)
                        {
                            // Create a new container.
                            _Current = new SimpleInjector.Container();
                        }
                    }
                }

                return _Current;
            }
        }
    }
}
