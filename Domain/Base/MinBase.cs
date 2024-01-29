using textileManagment.Entities.Base.IBase;

namespace textileManagment.Entities.Base
{
    public class MinBase : IMinBase
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
