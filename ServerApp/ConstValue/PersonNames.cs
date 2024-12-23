﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.ConstValue
{
    internal class PersonNames
    {

        public static readonly string[] FirstNames = new string[]
        {
    "Александр", "Максим", "Сергей", "Дмитрий", "Андрей", "Алексей", "Артем", "Иван", "Виктор", "Николай",
    "Игорь", "Олег", "Юрий", "Михаил", "Роман", "Константин", "Владимир", "Евгений", "Павел", "Борис",
    "Валентин", "Антон", "Георгий", "Василий", "Григорий", "Никита", "Егор", "Рустам", "Станислав", "Федор",
    "Женя", "Лев", "Петр", "Вадим", "Геннадий", "Арсен", "Артур", "Богдан", "Данила", "Руслан",
    "Елисей", "Денис", "Тимофей", "Матвей", "Ярослав", "Марсель", "Анатолий", "Глеб", "Даниил", "Филипп",
    "Василиса", "Марк", "Евграф", "Ярослав", "Святослав", "Олеся", "Кирилл", "Радион", "Макар", "Илья",
    "Прохор", "Платон", "Герман", "Родион", "Дамир", "Иннокентий", "Ростислав", "Наум", "Эдуард", "Серафим",
    "Амир", "Лука", "Всеволод", "Тихон", "Дамир", "Аким", "Валерий", "Альберт", "Савелий", "Эрик",
    "Виталий", "Семен", "Эмиль", "Ринат", "Ян", "Аркадий", "Вениамин", "Алан", "Аристарх", "Тарас",
    "Гордей", "Валентин", "Наум", "Юлиан", "Арсений", "Леонид", "Платон", "Федот", "Давид", "Евсей"
        };

        public static readonly string[] MiddleNames = new string[]
        {
    "Иванов", "Смирнов", "Кузнецов", "Попов", "Васильев", "Петров", "Соколов", "Михайлов", "Новиков", "Фёдоров",
    "Морозов", "Волков", "Алексеев", "Лебедев", "Семенов", "Егоров", "Павлов", "Козлов", "Степанов", "Николаев",
    "Орлов", "Андреев", "Макаров", "Никитин", "Захаров", "Зайцев", "Соловьёв", "Борисов", "Яковлев", "Григорьев",
    "Романов", "Воробьёв", "Серов", "Кузьмин", "Фролов", "Александров", "Дмитриев", "Королёв", "Гусев", "Киселёв",
    "Ильин", "Максимов", "Поляков", "Сорокин", "Виноградов", "Ковалёв", "Белов", "Медведев", "Антонов", "Тарасов",
    "Жуков", "Баранов", "Филиппов", "Комаров", "Давыдов", "Беляев", "Герасимов", "Богданов", "Осипов", "Сидоров",
    "Матвеев", "Титов", "Марков", "Миронов", "Крылов", "Куликов", "Карпов", "Власов", "Мельников", "Денисов",
    "Гаврилов", "Тихонов", "Казаков", "Афанасьев", "Данилов", "Савельев", "Тимофеев", "Фомин", "Чернов", "Абрамов",
    "Мартынов", "Ефимов", "Фёдоров", "Лапин", "Соколова", "Кондратьев", "Счетчиков", "Кудрявцев", "Бородин", "Гуляев",
    "Прохоров", "Назаров", "Авдеев", "Крючков", "Сунцов", "Логинов", "Савин", "Свиридов", "Королёв", "Кудрявцев"
        };

        public static readonly string[] LastNames = new string[]
        {
    "Александрович", "Максимович", "Сергеевич", "Дмитриевич", "Андреевич", "Алексеевич", "Артемович", "Иванович", "Викторович", "Николаевич",
    "Игоревич", "Олегович", "Юрьевич", "Михайлович", "Романович", "Константинович", "Владимирович", "Евгеньевич", "Павлович", "Борисович",
    "Валентинович", "Антонович", "Георгиевич", "Васильевич", "Григорьевич", "Никитич", "Егорович", "Рустамович", "Станиславович", "Федорович",
    "Женевич", "Львович", "Петрович", "Вадимович", "Геннадиевич", "Арсенович", "Артурович", "Богданович", "Данилович", "Русланович",
    "Елисеевич", "Денисович", "Тимофеевич", "Матвеевич", "Ярославович", "Марселевич", "Анатольевич", "Глебович", "Даниилович", "Филиппович",
    "Маркевич", "Евграфович", "Святославович", "Олегович", "Кириллович", "Радионович", "Макарович", "Ильич", "Прохорович", "Платонович",
    "Германович", "Родионович", "Дамирович", "Иннокентьевич", "Ростиславович", "Наумович", "Эдуардович", "Серафимович", "Алексеевич", "Тихонович",
    "Амирович", "Лукич", "Всеволодович", "Тарасович", "Акимович", "Валерьевич", "Альбертович", "Савельевич", "Эрикович", "Витальевич",
    "Семёнович", "Эмильевич", "Ринатович", "Янович", "Аркадьевич", "Вениаминович", "Аланович", "Трофимович", "Тимурович", "Яковлевич",
    "Валерьевич", "Юлианович", "Арсеньевич", "Леонидович", "Платонович", "Августович", "Давыдович", "Евсеевич", "Фёдорович", "Никитич"
        };

    }
}
