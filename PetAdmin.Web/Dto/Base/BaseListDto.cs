using System.Collections.Generic;

namespace PetAdmin.Web.Dto.Base
{
    public class BaseListDto<TEntityOrDto>
    {
        public ICollection<TEntityOrDto> Result { get; set; }
        public int Total { get; set; }
    }
}
