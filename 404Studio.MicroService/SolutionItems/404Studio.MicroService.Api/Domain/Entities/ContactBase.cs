namespace YH.Etms.Settlement.Api.Domain.Entities
{
    public abstract class ContactBase
    {
        private int _id;
        public virtual int Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }

        public virtual string Name { get; protected set; }

        public virtual string MobilePhone { get; protected set; }

        public virtual string Fax { get; protected set; }

        public virtual string Phone { get; protected set; }

        public virtual string Email { get; protected set; }

        public virtual string Remark { get; protected set; }

        public virtual bool Enable { get; protected set; } = false;

        public virtual bool IsDelete { get; protected set; } = false;

    }
}
