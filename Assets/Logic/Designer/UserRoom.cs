using System;

namespace Logic.Designer
{
    [Serializable]
    public class UserRoom
    {
        public string Json; // Сам гсон с данными о комнате
        public byte[] Preview; // Набор байтов в котором будет представленна картинка превьюшки
        public string Name; // Имя комнаты 
        public int RoomId; // ID комнаты для обращения в БД.
    }
}