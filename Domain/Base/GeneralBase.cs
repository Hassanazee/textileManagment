using textileManagment.Entities.Base.IBase;

namespace textileManagment.Entities.Base
{
    public class GeneralBase: IGeneralBase
    {
        public bool IsDelete { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public long? ModifiedById { get; set; }
        public bool IsActive { get; set; }
        public long? CreatedById { get; set; }
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
