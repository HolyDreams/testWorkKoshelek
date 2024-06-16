using System.ComponentModel.DataAnnotations;

namespace testWorkKoshelek.API.Models
{
    /// <summary>
    /// Период для фильтрации
    /// </summary>
    public class PeriodDTO
    {
        #region Private

        private DateTime _end;

        #endregion

        #region Properties

        /// <summary>
        /// Начало периода
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Дата начала обязательна")]
        public DateTime Start {  get; set; }

        /// <summary>
        /// Окончание периода
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public DateTime? End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value ?? DateTime.Now;
            }
        }

        #endregion
    }
}
