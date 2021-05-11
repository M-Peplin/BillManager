using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillManager.Models.ModelsDTO;

namespace BillManager.Interfaces
{
    public interface IBillsService
    {
        ResponseDTO AddBill(BillDTO billDTO);
        ResponseDTO EditBIll(BillDTO billDTO);
        ResponseDTO DeleteBill(string mail);
        BillsDTO GetAllBillByUser(string mail);
    }
}
