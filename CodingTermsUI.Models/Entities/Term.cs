using CodingTermsUI.Models.Entities.Base;

namespace CodingTermsUI.Models.Entities
{
    public class Term : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
    }
}
