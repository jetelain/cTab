using System.ComponentModel.DataAnnotations;

namespace cTabWebApp.Messaging
{
    public enum MessageTemplateType
    {
        Generic,
        
        [Display(Name = "Medical Evacuation")]
        MedicalEvacuation,
        
        [Display(Name = "Artillery")]
        Artillery,

        [Display(Name = "Air Support")]
        AirSupport
    }
}