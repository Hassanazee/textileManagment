namespace textileManagment.Entities.Base.IBase
{
    public interface IGeneralBase :IMinActiveBase
    {
        public void GeneralBase()
        {
            IsDelete = false;
        }
        public bool IsDelete { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedById { get; set; }
    }
}
