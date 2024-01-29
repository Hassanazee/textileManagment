namespace textileManagment.Entities.Base.IBase
{
    public interface IMinActiveBase : IMinBase
    {
        public bool IsActive { get; set; }
        public long? CreatedById { get; set; }
    }
}
