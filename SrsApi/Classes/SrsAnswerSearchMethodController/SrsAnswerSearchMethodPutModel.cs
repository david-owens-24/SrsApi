using SrsApi.Classes.SrsAnswerSearchMethodController;
using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemLevelController
{
    public class SrsAnswerSearchMethodPutModel
    {
        public List<SrsAnswerSearchMethodItem> SearchMethods { get; set; }
    }    
}
