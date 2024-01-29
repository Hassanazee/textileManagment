using textileManagment.Entities.Base.IBase;

namespace textileManagment.Entities.Base
{
    public class MinActiveBase : IMinActiveBase
    {
        public bool IsActive { get; set; }
        public long? CreatedById { get; set; }
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
