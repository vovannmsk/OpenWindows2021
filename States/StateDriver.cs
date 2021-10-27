using System;
using OpenGEWindows;

namespace States
{
    public class StateDriver : IEquatable<IState>
    {
        private botWindow botwindow;
        private IState currentState;    //текущее состояние
        private IState endState;        //конечное состояние
        private int[] counterState = new int[10000];

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="botwindow"> это все данные проокно бота </param>
        /// <param name="currentState"> текущее состояние </param>
        public StateDriver(botWindow botwindow, IState currentState, IState endState)
        {
            this.botwindow = botwindow;
            this.currentState = currentState;
            this.endState = endState;
            for (int i = 1; i < counterState.Length ; i++)
            { counterState[i] = 0; }     //когда запускаем новый движок состояний, то обнуляем массив счетчиков
        }

        /// <summary>
        /// задаем метод Equals для данного объекта для получения возможности сравнения объектов State
        /// </summary>
        /// <param name="other"> объект для сравнения </param>
        /// <returns> true, если номера состояний объектов равны </returns>
        public bool Equals(IState other)
        {
            //bool result = false;
            //if (!(other == null))            //если other не null, то проверяем на равенство
            //    if (this.getTekStateInt() == other.getTekStateInt()) result = true;
            //return result;
            return currentState.Equals(other);      //делегируем сравнение состояний в текущее состояние (в нем переопределён метод Equals)
        }

        /// <summary>
        /// метод осуществляет действия для перехода из состояния GT01 в GT02
        /// </summary>
        public void run()                // переход к следующему состоянию
        {
            int ii = getTekStateInt();
            counterState[ii]++;  //прибавляется единица к счетчику этого состояния, 
            currentState.run();
        }
        
        /// <summary>
        /// метод осуществляет действия для перехода к запасному состоянию, если не удался переход из состояния GT01 в GT02
        /// </summary>
        public void elseRun()
        {
            currentState.elseRun();
        }

        /// <summary>
        /// проверяет, получилось ли перейти к состоянию GT02
        /// </summary>
        /// <returns> true, если получилось перейти к состоянию GT02 </returns>
        public bool isAllCool()          // получилось ли перейти к следующему состоянию. true, если получилось
        {
            return currentState.isAllCool();
        }

        /// <summary>
        /// возвращает следующее состояние, если переход осуществился
        /// </summary>
        /// <returns> следующее состояние </returns>
        public IState StateNext()         // возвращает следующее состояние, если переход осуществился
        {
            return currentState.StateNext();
        }

        /// <summary>
        /// возвращает запасное состояние, если переход не осуществился
        /// </summary>
        /// <returns> запасное состояние </returns>
        public IState StatePrev()         // возвращает запасное состояние, если переход не осуществился
        {
            IState result = currentState.StatePrev();
             
            if (counterState[getTekStateInt()] >= 5) //если счетчик этого состояния больше или равен пяти, то значит мы застряли на этом состоянии
                if ((getTekStateInt() != 72) && 
                    (getTekStateInt() != 73) && 
                    (getTekStateInt() != 80) &&
                    (getTekStateInt() != 85) &&
                    (getTekStateInt() != 92) &&
                    (getTekStateInt() != 224) &&
                    (getTekStateInt() != 232) &&
                    (getTekStateInt() != 236) &&
                    (getTekStateInt() != 239) &&
                    (getTekStateInt() != 301) &&
                    (getTekStateInt() != 305))
                    //(на состоянии 72 и 73 не делать ничего (там долгая чиповка и бьем отит и кукурузу)) 
                    // состояние 92 - алхимия 
                    // 224 - применяем карточки опыта
                    // 232 - применяем карточки опыта
                    // 236 - применяем подарки
                    // 239 - ищем флинтлок в списке
                    // 301 - увеличиваем казарму у Диего, пока есть барак слоты

                    result = this.endState;                 // тогда присваиваем движку конечное состояние, чтобы остановить его
            return result;
//            return currentState.StatePrev();
        }

        /// <summary>
        /// изменение текущего состояния. Если переход прошел, то  StateNext(). Иначе StatePrev()
        /// </summary>
        public void setState()
        {
            if (isAllCool())
            { currentState = StateNext(); }
            else
            {
                elseRun();
                currentState = StatePrev();
            }
        }

        /// <summary>
        /// геттер. возвращает номер текущего состояния
        /// </summary>
        /// <returns> номер состояния </returns>
        public int getTekStateInt()
        {
            return currentState.getTekStateInt();
        }

        /// <summary>
        /// геттер, возвращает текущее состояние
        /// </summary>
        /// <returns></returns>
        public IState getTekState()
        {
            return currentState.getTekState();
        }













    }
}
