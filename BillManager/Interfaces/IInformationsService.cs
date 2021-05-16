using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillManager.Models.ModelsDTO;

namespace BillManager.Interfaces
{
    public interface IInformationsService
    {
        ResponseDTO AddInformation(InformationDTO informationDTO);
        ResponseDTO EditInformation(InformationDTO informationDTO);
        ResponseDTO DeleteInformation(string mail);
        InformationsDTO GetAllByUser(string userId);
    }
}
