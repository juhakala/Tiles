namespace WpfTiles.Model
{
    sealed class ModelMainController
    {
        private static readonly object initlock = new object ();
        private static ModelMainController instance = null;
        public static ModelMainController Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (initlock)
                        {
                            if (instance == null)
                            {
                                instance = new ModelMainController();
                            }
                        }
                }
                return instance;
            }
        }
        private ModelMainController()
        {
            GameController = new ModelGameController();
        }

        public ModelGameController GameController { get; set; }
    }
}
