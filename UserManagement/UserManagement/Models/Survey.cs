//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UserManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Survey
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Survey()
        {
            this.Questions = new HashSet<Question>();
        }
    
        [Key]
        public int SurveyId { get; set; }

        [Required(ErrorMessage = " Survey name is Required")]
        public string surveyName { get; set; }

        [Required(ErrorMessage = "Description is Required")]
        public string Description { get; set; }
        public Nullable<int> userId { get; set; }

        [Required(ErrorMessage = "StartDate is Required")]
        public string startData { get; set; }

        [Required(ErrorMessage = "EndDate is Required")]
        public string endDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Question> Questions { get; set; }
        public virtual User User { get; set; }
    }
}
