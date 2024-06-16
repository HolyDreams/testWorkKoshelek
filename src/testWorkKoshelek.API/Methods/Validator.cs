using testWorkKoshelek.API.Models;

namespace testWorkKoshelek.API.Methods
{
    public static class Validator
    {
        /// <summary>
        /// Проверка входящего сообщения
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        public static bool IsValidMessage(this MessageDTO message)
        {
            return message.Text.Length < 128;
        }

        /// <summary>
        /// Проверка периода
        /// </summary>
        /// <returns></returns>
        public static bool IsValidPeriod(DateTime start, DateTime end)
        {
            return start < end;
        }
    }
}
