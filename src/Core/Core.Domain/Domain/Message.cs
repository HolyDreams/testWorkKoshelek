namespace Core.Domain.Domain
{
    public class Message
    {
        #region Private

        private readonly Guid _id;
        private string _text = "";
        private DateTime _createdDate;

        #endregion

        public Message() 
        { 
            _id = Guid.NewGuid();
        }

        public Message(string text)
        {
            _id = Guid.NewGuid();

            _text = text;
            _createdDate = DateTime.Now;
        }

        public Message(string text, DateTime createdDate)
        {
            _text = text;
            _createdDate = createdDate;
        }

        public Message(Guid id, string text, DateTime createdDate)
        {
            _id = id;
            _text = text;
            _createdDate = createdDate;
        }

        #region Properties

        public Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        /// <summary>
        /// Дата создания сообщения
        /// </summary>
        public DateTime CreatedDate
        {
            get
            {
                return _createdDate;
            }
            set
            {
                _createdDate = value;
            }
        }

        #endregion
    }
}
